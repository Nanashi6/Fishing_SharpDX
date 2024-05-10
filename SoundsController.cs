using System;
using System.Collections.Generic;
using System.IO;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace Fishing_SharpDX
{
    public class SoundsController : IDisposable
    {
        //путь к музыке
        private string _musicPath;
        //путь к звукам объектов
        private string[] _soundsPath;
        //путь к звукам шагов
        private string[] _steps;

        //ядро для воспроизведения музыки
        private XAudio2 _audioMusic;
        private MasteringVoice _voiceMusic;

        //ядро для воспроизведения звуков
        private XAudio2 _audioSounds;
        private MasteringVoice _voiceSounds;

        private SourceVoice _music;
        private SourceVoice _sound;

        private SoundStream _sourceMusic;

        private SoundStream _floaterOn;
        private SoundStream _floaterOff;

        private SoundStream _sourceStep1;
        private SoundStream _sourceStep2;
        private SoundStream _sourceStep3;
        private SoundStream _sourceStep4;

        private List<SoundStream> _sourceStep;

        private AudioBuffer _bufferMusic;
        private List<AudioBuffer> _bufferSounds;
        private List<AudioBuffer> _bufferSteps;

        private Random _rd;

        private float _timeStep;
        private float _timeStepDuration;

        public SoundsController(float musicVolume, float soundsVolume)
        {
            _musicPath = "Sounds/ozero.wav";

            _soundsPath = new string[2];
            _soundsPath[0] = "Sounds/floater/poplavok_on.wav";
            _soundsPath[1] = "Sounds/floater/poplavok_off.wav";

            _steps = new string[4];
            _steps[0] = "Sounds/steps/step1.wav";
            _steps[1] = "Sounds/steps/step2.wav";
            _steps[2] = "Sounds/steps/step3.wav";
            _steps[3] = "Sounds/steps/step4.wav";


            _rd = new Random();
            _sourceStep = new List<SoundStream>();
            _bufferSounds = new List<AudioBuffer>();
            _bufferSteps = new List<AudioBuffer>();

            _audioMusic = new XAudio2();

            _voiceMusic = new MasteringVoice(_audioMusic);
            _voiceMusic.SetVolume(musicVolume);
            _audioMusic.StartEngine();

            _audioSounds = new XAudio2();

            _voiceSounds = new MasteringVoice(_audioSounds);
            _voiceSounds.SetVolume(soundsVolume);
            _audioSounds.StartEngine();

            _timeStep = 0;
            _timeStepDuration = 0.5f;

            MusicLoad();
            SoundLoad();
            StepLoad();
        }

        private void MusicLoad()
        {
            using (_sourceMusic = new SoundStream(File.OpenRead(_musicPath)))
            {

                _bufferMusic = new AudioBuffer
                {
                    Stream = _sourceMusic.ToDataStream(),
                    AudioBytes = (int)_sourceMusic.Length,
                    Flags = BufferFlags.EndOfStream
                };
            }
        }

        private void SoundLoad()
        {
            using (_floaterOn = new SoundStream(File.OpenRead(_soundsPath[0])))
            {

                _bufferSounds.Add(new AudioBuffer
                {
                    Stream = _floaterOn.ToDataStream(),
                    AudioBytes = (int)_floaterOn.Length,
                    Flags = BufferFlags.EndOfStream
                });
            }

            using (_floaterOff = new SoundStream(File.OpenRead(_soundsPath[1])))
            {

                _bufferSounds.Add(new AudioBuffer
                {
                    Stream = _floaterOff.ToDataStream(),
                    AudioBytes = (int)_floaterOff.Length,
                    Flags = BufferFlags.EndOfStream
                });
            }
        }

        private void StepLoad()
        {
            using (_sourceStep1 = new SoundStream(File.OpenRead(_steps[0])))
            {
                _bufferSteps.Add(new AudioBuffer
                {
                    Stream = _sourceStep1.ToDataStream(),
                    AudioBytes = (int)_sourceStep1.Length,
                    Flags = BufferFlags.EndOfStream
                });
                _sourceStep.Add(_sourceStep1);
            }


            using (_sourceStep2 = new SoundStream(File.OpenRead(_steps[1])))
            {
                _bufferSteps.Add(new AudioBuffer
                {
                    Stream = _sourceStep2.ToDataStream(),
                    AudioBytes = (int)_sourceStep2.Length,
                    Flags = BufferFlags.EndOfStream
                });
                _sourceStep.Add(_sourceStep2);
            }

            using (_sourceStep3 = new SoundStream(File.OpenRead(_steps[2])))
            {
                _bufferSteps.Add(new AudioBuffer
                {
                    Stream = _sourceStep3.ToDataStream(),
                    AudioBytes = (int)_sourceStep3.Length,
                    Flags = BufferFlags.EndOfStream
                });
                _sourceStep.Add(_sourceStep3);
            }

            using (_sourceStep4 = new SoundStream(File.OpenRead(_steps[3])))
            {
                _bufferSteps.Add(new AudioBuffer
                {
                    Stream = _sourceStep4.ToDataStream(),
                    AudioBytes = (int)_sourceStep4.Length,
                    Flags = BufferFlags.EndOfStream
                });
                _sourceStep.Add(_sourceStep4);
            }

        }


        public void Music()
        {
            _music = new SourceVoice(_audioMusic, _sourceMusic.Format);
            _music.SubmitSourceBuffer(_bufferMusic, _sourceMusic.DecodedPacketsInfo);
            _music.Start();
        }

        public void Step(float deltaT)
        {
            _timeStep += deltaT;

            if (_timeStep >= _timeStepDuration)
            {
                SoundStream source = _sourceStep[_rd.Next(_sourceStep.Count)];
                AudioBuffer buffer = _bufferSteps[_rd.Next(_sourceStep.Count)];
                _sound = new SourceVoice(_audioSounds, source.Format);
                _sound.SubmitSourceBuffer(buffer, source.DecodedPacketsInfo);
                _sound.Start();
                _timeStep = 0;
            }
        }

        public void FloaterOn()
        {
            _sound = new SourceVoice(_audioSounds, _floaterOn.Format);
            _sound.SubmitSourceBuffer(_bufferSounds[0], _floaterOn.DecodedPacketsInfo);
            _sound.Start();
        }

        public void FloaterOff()
        {
            _sound = new SourceVoice(_audioSounds, _floaterOff.Format);
            _sound.SubmitSourceBuffer(_bufferSounds[1], _floaterOff.DecodedPacketsInfo);
            _sound.Start();
        }

        public bool GetMusicRepeat()
        {
            if (_music.State.BuffersQueued == 0)
                return true;

            return false;
        }

        public void Stop()
        {
            try
            {
                _music.Stop();
                _sound.Stop();
            }
            catch { }
        }

        public void Dispose()
        {
            Utilities.Dispose(ref _audioMusic);
            Utilities.Dispose(ref _audioSounds);
            Utilities.Dispose(ref _sound);
            Utilities.Dispose(ref _music);
        }
    }
}
