using NAudio.Wave;

namespace UntitledEngine.engine.audio
{
    public class AudioManager
    {

        // Play a one-shot sound
        public void Play(string filePath)
        {
            var waveOut = new WaveOutEvent();
            var audioReader = new AudioFileReader(filePath);

            waveOut.Init(audioReader);
            waveOut.Play();

            waveOut.PlaybackStopped += (s, e) =>
            {
                audioReader.Dispose();
                waveOut.Dispose();
            };
        }
    }
}
