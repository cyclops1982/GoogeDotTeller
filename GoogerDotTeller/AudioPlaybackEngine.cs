using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace GoogerDotTeller
{
    class AudioPlaybackEngine : IDisposable
    {
        private readonly IWavePlayer outputDevice;
        private readonly MixingSampleProvider mixer;
        private MemoryStream ms;
        public AudioPlaybackEngine(int sampleRate = 44100, int channelCount = 2)
        {
            outputDevice = new WaveOutEvent();
            ms = EmbeddedResourceHelper.GetEmbeddedResource("Windows Navigation Start.wav");
            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            mixer.ReadFully = true;
            outputDevice.Init(mixer);
            outputDevice.Play();
        }


        public void PlaySound(int delay)
        {
            ms.Seek(0, SeekOrigin.Begin);
            WaveFileReader re = new WaveFileReader(ms);
            mixer.AddMixerInput(re);
            Thread.Sleep(delay);
        }


        public void Dispose()
        {
            outputDevice.Dispose();
        }

        public static readonly AudioPlaybackEngine Instance = new AudioPlaybackEngine(44100, 2);
    }

}
