#pragma once
#include "Common.h"
#include "Midi.h"
#include "framework.h"

#include <chrono>
#include <memory>
#include <thread>

class MidiOut
{
  public:
	MidiOut();

	static std::shared_ptr<HMIDIOUT> GetDevice();
	void Play(std::chrono::duration<double, std::ratio<1>> duration, uint8_t channel, Midi::Key key, uint8_t velocity);
	void Set(Midi::Timbre instrument, uint8_t channel);
	void NoteOn(uint8_t channel, Midi::Key key, uint8_t velocity);

  private:
	DWORD GetCommand(uint8_t channel, Midi::ChannelVoiceMessage command, Midi::Timbre instrument);

	DWORD GetCommand(uint8_t channel, Midi::ChannelVoiceMessage command, uint8_t key, uint8_t velocity);

	std::shared_ptr<HMIDIOUT> midi_device;
};
