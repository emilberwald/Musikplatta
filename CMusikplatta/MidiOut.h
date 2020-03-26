#pragma once
#include "Common.h"
#include "framework.h"

#include <chrono>
#include <memory>
#include <thread>

class MidiOut
{
  public:
	MidiOut(): midi_device(GetDevice()) {}

	static std::shared_ptr<HMIDIOUT> GetDevice()
	{
		return std::shared_ptr<HMIDIOUT>(
			std::invoke([]() {
				auto midiDevice = new HMIDIOUT{};
				MP_THROW_IF(midiOutGetNumDevs() <= 0, std::runtime_error);
				MP_THROW_IF(midiOutOpen(midiDevice, 0, 0, 0, CALLBACK_NULL) != MMSYSERR_NOERROR, std::runtime_error);
				return midiDevice;
			}),
			[](auto p) {
				if(p != nullptr)
				{
					MP_ERROR_IF(midiOutReset(*p) != MMSYSERR_NOERROR);
					MP_ERROR_IF(midiOutClose(*p) != MMSYSERR_NOERROR);
				}
				delete p;
			});
	}

	enum ChannelVoiceMessage : uint8_t
	{
		NoteOff							= 0b1000,
		NoteOn							= 0b1001,
		PolyphonicKeyPressureAfterTouch = 0b1010,
		ControlChange					= 0b1011,
		ChannelModeMessage				= 0b1011,
		ProgramChange					= 0b1100,
		ChannelPressureAfterTouch		= 0b1101,
		PitchBendChange					= 0b1110,
	};

	enum SystemCommonMessage : uint8_t
	{
		BeginSystemExclusive	 = 0b11110000,
		MIDITimeCodeQuarterFrame = 0b11110001,
		SongPositionPointer		 = 0b11110010,
		SongSelect				 = 0b11110011,
		Reserved				 = 0b11110100,
		Reserved				 = 0b11110101,
		TuneRequest				 = 0b11110110,
		EndSystemExclusive		 = 0b11110111,
	};

	enum SystemRealtimeMessage : uint8_t
	{
		TimingClock	  = 0b11111000,
		Reserved	  = 0b11111001,
		Start		  = 0b11111010,
		Continue	  = 0b11111011,
		Stop		  = 0b11111100,
		Reserved	  = 0b11111101,
		ActiveSensing = 0b11111110,
		Reset		  = 0b11111111,
	};

	enum Instrument : uint8_t
	{
		AcousticGrandPiano	 = 0,
		BrightAcousticPiano	 = 1,
		ElectricGrandPiano	 = 2,
		HonkyTonkPiano		 = 3,
		ElectricPiano1		 = 4,
		ElectricPiano2		 = 5,
		Harpsichord			 = 6,
		Clavi				 = 7,
		Celesta				 = 8,
		Glockenspiel		 = 9,
		MusicBox			 = 10,
		Vibraphone			 = 11,
		Marimba				 = 12,
		Xylophone			 = 13,
		TubularBells		 = 14,
		Dulcimer			 = 15,
		DrawbarOrgan		 = 16,
		PercussiveOrgan		 = 17,
		RockOrgan			 = 18,
		ChurchOrgan			 = 19,
		ReedOrgan			 = 20,
		Accordion			 = 21,
		Harmonica			 = 22,
		TangoAccordion		 = 23,
		AcousticGuitar_nylon = 24,
		AcousticGuitar_steel = 25,
		ElectricGuitar_jazz	 = 26,
		ElectricGuitar_clean = 27,
		ElectricGuitar_muted = 28,
		OverdrivenGuitar	 = 29,
		DistortionGuitar	 = 30,
		Guitarharmonics		 = 31,
		AcousticBass		 = 32,
		ElectricBass_finger	 = 33,
		ElectricBass_pick	 = 34,
		FretlessBass		 = 35,
		SlapBass1			 = 36,
		SlapBass2			 = 37,
		SynthBass1			 = 38,
		SynthBass2			 = 39,
		Violin				 = 40,
		Viola				 = 41,
		Cello				 = 42,
		Contrabass			 = 43,
		TremoloStrings		 = 44,
		PizzicatoStrings	 = 45,
		OrchestralHarp		 = 46,
		Timpani				 = 47,
		StringEnsemble1		 = 48,
		StringEnsemble2		 = 49,
		SynthStrings1		 = 50,
		SynthStrings2		 = 51,
		ChoirAahs			 = 52,
		VoiceOohs			 = 53,
		SynthVoice			 = 54,
		OrchestraHit		 = 55,
		Trumpet				 = 56,
		Trombone			 = 57,
		Tuba				 = 58,
		MutedTrumpet		 = 59,
		FrenchHorn			 = 60,
		BrassSection		 = 61,
		SynthBrass1			 = 62,
		SynthBrass2			 = 63,
		SopranoSax			 = 64,
		AltoSax				 = 65,
		TenorSax			 = 66,
		BaritoneSax			 = 67,
		Oboe				 = 68,
		EnglishHorn			 = 69,
		Bassoon				 = 70,
		Clarinet			 = 71,
		Piccolo				 = 72,
		Flute				 = 73,
		Recorder			 = 74,
		PanFlute			 = 75,
		BlownBottle			 = 76,
		Shakuhachi			 = 77,
		Whistle				 = 78,
		Ocarina				 = 79,
		Lead1_square		 = 80,
		Lead2_sawtooth		 = 81,
		Lead3_calliope		 = 82,
		Lead4_chiff			 = 83,
		Lead5_charang		 = 84,
		Lead6_voice			 = 85,
		Lead7_fifths		 = 86,
		Lead8_basslead		 = 87,
		Pad1_newage			 = 88,
		Pad2_warm			 = 89,
		Pad3_polysynth		 = 90,
		Pad4_choir			 = 91,
		Pad5_bowed			 = 92,
		Pad6_metallic		 = 93,
		Pad7_halo			 = 94,
		Pad8_sweep			 = 95,
		FX1_rain			 = 96,
		FX2_soundtrack		 = 97,
		FX3_crystal			 = 98,
		FX4_atmosphere		 = 99,
		FX5_brightness		 = 100,
		FX6_goblins			 = 101,
		FX7_echoes			 = 102,
		FX8_scifi			 = 103,
		Sitar				 = 104,
		Banjo				 = 105,
		Shamisen			 = 106,
		Koto				 = 107,
		Kalimba				 = 108,
		Bagpipe				 = 109,
		Fiddle				 = 110,
		Shanai				 = 111,
		TinkleBell			 = 112,
		Agogo				 = 113,
		SteelDrums			 = 114,
		Woodblock			 = 115,
		TaikoDrum			 = 116,
		MelodicTom			 = 117,
		SynthDrum			 = 118,
		ReverseCymbal		 = 119,
		GuitarFretNoise		 = 120,
		BreathNoise			 = 121,
		Seashore			 = 122,
		BirdTweet			 = 123,
		TelephoneRing		 = 124,
		Helicopter			 = 125,
		Applause			 = 126,
		Gunshot				 = 127,
	};

	void Play(std::chrono::duration<double, std::milli> duration, uint8_t channel, uint8_t key, uint8_t volume)
	{
		if(this->midi_device)
		{
			midiOutShortMsg(*this->midi_device.get(), GetCommand(channel, ChannelVoiceMessage::NoteOn, key, volume));
			std::this_thread::sleep_for(duration);
			midiOutShortMsg(*this->midi_device.get(), GetCommand(channel, ChannelVoiceMessage::NoteOn, key, volume));
		}
	}

	DWORD GetCommand(uint8_t channel, ChannelVoiceMessage command, Instrument instrument)
	{
		return (0b0000'0000'0000'0000'0000'0000'0000'1111 & (channel))
			   | (0b0000'0000'0000'0000'0000'0000'1111'0000 & (command << 4))
			   | (0b0000'0000'0000'0000'1111'1111'0000'0000 & (instrument << 8));
	}

	DWORD GetCommand(uint8_t channel, ChannelVoiceMessage command, uint8_t key, uint8_t velocity)
	{
		return (0b0000'0000'0000'0000'0000'0000'0000'1111 & (channel))
			   | (0b0000'0000'0000'0000'0000'0000'1111'0000 & (command << 4))
			   | (0b0000'0000'0000'0000'1111'1111'0000'0000 & (key << 8))
			   | (0b0000'0000'1111'1111'0000'0000'0000'0000 & (velocity << 16));
	}

	//// Step 2. Set instrument for a channel (0..15)
	//DWORD command = (0x000000C0 | CHANNEL) | (INSTRUMENT << 8);
	//midiOutShortMsg(midi_device, command);
	//
	//// Step 3. "Touch" a key, sleep some, and "Release" then.
	//DWORD command = (0x00000090 | CHANNEL) | (KEY << 8) | (VOLUME << 16);
	//midiOutShortMsg(midi_device, command);
	//Sleep(100);
	//command &= 0x0000FFFF;
	//midiOutShortMsg(midi_device, command);
	//
	//// Step 4. Reset and Close the MIDI device as no longer needed
	//midiOutReset(midi_device);
	//midiOutClose(midi_device);

	std::shared_ptr<HMIDIOUT> midi_device;
};
