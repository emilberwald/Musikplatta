#include "MidiOut.h"

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

void MidiOut::NoteOn(uint8_t channel, Midi::Key key, uint8_t velocity)
{
	if(this->midi_device)
	{
		spdlog::info(MP_HERE + " NoteOn: {:x} {:x} {:x}", channel, (int)key, velocity);
		midiOutShortMsg(*this->midi_device.get(),
						GetCommand(channel, Midi::ChannelVoiceMessage::NoteOn, static_cast<uint8_t>(key), velocity));
	}
}

void MidiOut::Play(std::chrono::duration<double, std::ratio<1>> duration,
				   uint8_t										channel,
				   Midi::Key									key,
				   uint8_t										velocity)
{
	if(this->midi_device)
	{
		spdlog::info(MP_HERE + " NoteOn: {} {:x} {:x} {:x}", duration.count(), channel, (int)key, velocity);
		midiOutShortMsg(*this->midi_device.get(),
						GetCommand(channel, Midi::ChannelVoiceMessage::NoteOn, static_cast<uint8_t>(key), velocity));
		std::this_thread::sleep_for(duration);
		spdlog::info(MP_HERE + " NoteOff: {} {:x} {:x} {:x}", duration.count(), channel, (int)key, velocity);
		midiOutShortMsg(*this->midi_device.get(),
						GetCommand(channel, Midi::ChannelVoiceMessage::NoteOn, static_cast<uint8_t>(key), velocity));
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
