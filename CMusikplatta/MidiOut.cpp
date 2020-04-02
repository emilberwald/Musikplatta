#include "MidiOut.h"

#include <chrono>
#include <future>
#include <thread>

MidiOut::second_t MidiOut::note_off_duration = second_t{ 2.0 };
MidiOut::second_t MidiOut::note_on_burn_in	 = second_t{ 0.001 };

MidiOut::MidiOut(): midi_device(GetDevice()) {}

std::shared_ptr<HMIDIOUT> MidiOut::GetDevice()
{
	return std::shared_ptr<HMIDIOUT>(
		std::invoke([]() {
			HMIDIOUT* midiDevice = new HMIDIOUT{};
			MP_THROW_IF(midiOutGetNumDevs() <= 0, std::runtime_error);
			MP_THROW_IF(midiOutOpen(midiDevice, 0, 0, 0, CALLBACK_NULL) != MMSYSERR_NOERROR, std::runtime_error);
			return midiDevice;
		}),
		[](HMIDIOUT* p) {
			if(p != nullptr)
			{
				MP_ERROR_IF(midiOutReset(*p) != MMSYSERR_NOERROR);
				MP_ERROR_IF(midiOutClose(*p) != MMSYSERR_NOERROR);
			}
			delete p;
		});
}

void MidiOut::Play(uint8_t channel, Midi::Key key, uint8_t velocity)
{
	if(this->midi_device)
	{
		auto lookup = std::make_tuple(channel, key);
		if(this->keys.count(lookup) <= 0)
		{
			if(velocity > 0)
			{
				auto noteTimestamp = std::chrono::high_resolution_clock::now();
				auto noteState	   = static_cast<uint8_t>(key);
				auto noteVelocity  = velocity;
				this->keys[lookup] = std::make_tuple(noteTimestamp, NoteState::Pending, noteVelocity);
			}
		}
		else
		{
			auto noteTimestamp = std::get<0>(this->keys[lookup]);
			auto noteState	   = std::get<1>(this->keys[lookup]);
			auto noteVelocity  = std::get<2>(this->keys[lookup]);

			if(noteState == NoteState::Pending)
			{
				//build up
				if(std::chrono::high_resolution_clock::now() - noteTimestamp < note_on_burn_in)
				{
					if(velocity > noteVelocity)
						this->keys[lookup] = std::make_tuple(noteTimestamp, noteState, velocity);
					return;
				}

				midiOutShortMsg(
					*this->midi_device.get(),
					GetCommand(channel, Midi::ChannelVoiceMessage::NoteOn, static_cast<uint8_t>(key), noteVelocity));
				spdlog::info(MP_HERE + " NoteOn: {:x} {:x} {:x}", channel, (int)key, noteVelocity);
				this->keys[lookup] = std::make_tuple(noteTimestamp, NoteState::Running, noteVelocity);
				return;
			}

			if(noteState == NoteState::Running)
			{
				if(velocity == 0)
				{
					this->keys.clear();
					return;
				}
				if(this->keys.size() > 1)
				{
					auto me = this->keys[lookup];
					this->keys.clear();
					this->keys[lookup] = me;
					spdlog::info(MP_HERE + " Aborting.");
					return;
				}

				//re-trigger pause
				if(std::chrono::high_resolution_clock::now() - noteTimestamp < note_off_duration) { return; }
				midiOutShortMsg(
					*this->midi_device.get(),
					GetCommand(channel, Midi::ChannelVoiceMessage::NoteOff, static_cast<uint8_t>(key), velocity));
				spdlog::info(MP_HERE + " NoteOff: {:x} {:x} {:x}", channel, (int)key, velocity);
				this->keys.erase(lookup);
			}
		}
	}
}

void MidiOut::Set(Midi::Timbre instrument, uint8_t channel)
{
	if(this->midi_device)
	{
		spdlog::info(MP_HERE + " ProgramChange: {:x} {:x}", (int)instrument, channel);
		midiOutShortMsg(*this->midi_device.get(),
						GetCommand(channel, Midi::ChannelVoiceMessage::ProgramChange, instrument));
	}
}

DWORD MidiOut::GetCommand(uint8_t channel, Midi::ChannelVoiceMessage command, Midi::Timbre instrument)
{
	return (0b0000'0000'0000'0000'0000'0000'0000'1111 & (channel))
		   | (0b0000'0000'0000'0000'0000'0000'1111'0000 & (static_cast<uint8_t>(command) << 4))
		   | (0b0000'0000'0000'0000'1111'1111'0000'0000 & (static_cast<uint8_t>(instrument) << 8));
}

DWORD MidiOut::GetCommand(uint8_t channel, Midi::ChannelVoiceMessage command, uint8_t key, uint8_t velocity)
{
	return (0b0000'0000'0000'0000'0000'0000'0000'1111 & (channel))
		   | (0b0000'0000'0000'0000'0000'0000'1111'0000 & (static_cast<uint8_t>(command) << 4))
		   | (0b0000'0000'0000'0000'1111'1111'0000'0000 & (key << 8))
		   | (0b0000'0000'1111'1111'0000'0000'0000'0000 & (velocity << 16));
}
