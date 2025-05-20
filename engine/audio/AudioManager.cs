using NAudio.Wave;

namespace UntitledEngine.engine.audio
{
    public class AudioManager
    {
        private IWavePlayer waveOut;
        private WaveStream audioReader;

        public AudioManager()
        {
            waveOut = new WaveOutEvent();
        }

        // Play a sound
        public void Play(string filePath)
        {
            audioReader?.Dispose();

            audioReader = new AudioFileReader(filePath);

            waveOut.Init(audioReader);
            waveOut.Play();

            waveOut.PlaybackStopped += (s, e) =>
            {
                audioReader.Dispose();
            };
        }

        // Stop all(?) sounds
        public void Stop()
        {
            waveOut?.Stop();
        }

        // Clean up resources
        public void Cleanup()
        {
            waveOut?.Stop();
            waveOut?.Dispose();
            audioReader?.Dispose();
        }
    }
}
