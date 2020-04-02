#pragma once
#include "Common.h"
#include "Midi.h"
#include "framework.h"

#include <chrono>
#include <future>
#include <map>
#include <memory>
#include <thread>

class MidiOut
{
  public:
	using second_t = std::chrono::duration<double, std::ratio<1>>;
	MidiOut();

	static std::shared_ptr<HMIDIOUT> GetDevice();
	void							 Set(Midi::Timbre instrument, uint8_t channel);
	void							 Play(uint8_t channel, Midi::Key key, uint8_t velocity);

	static second_t note_on_burn_in;
	static second_t note_off_duration;

  private:
	DWORD GetCommand(uint8_t channel, Midi::ChannelVoiceMessage command, Midi::Timbre instrument);

	DWORD GetCommand(uint8_t channel, Midi::ChannelVoiceMessage command, uint8_t key, uint8_t velocity);

	std::shared_ptr<HMIDIOUT> midi_device;

	enum class NoteState
	{
		Pending,
		Aborting,
		Running,
		Finished
	};

	std::map<std::tuple<uint8_t, Midi::Key>,
			 std::tuple<std::chrono::high_resolution_clock::time_point, NoteState, uint8_t>>
		keys;
};
