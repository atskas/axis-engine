using System;
using System.Threading.Tasks;
using NAudio.Wave;

namespace UntitledEngine.engine.audio
{
    public class AudioManager
    {
        // Play a one-shot sound asynchronously
        public void Play(string filePath)
        {
            Task.Run(() =>
            {
                AudioFileReader audioReader = null;
                WaveOutEvent waveOut = null;

                try
                {
                    audioReader = new AudioFileReader(filePath);
                    waveOut = new WaveOutEvent();
                    waveOut.Init(audioReader);

                    // Register the event handler before playing
                    waveOut.PlaybackStopped += (s, e) =>
                    {
                        waveOut.Dispose();
                        audioReader.Dispose();
                    };

                    waveOut.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AudioManager] Error playing audio: {ex.Message}");
                    waveOut?.Dispose();
                    audioReader?.Dispose();
                }
            });
        }
    }
}
