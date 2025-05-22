using System;
using System.IO;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;

namespace UntitledEngine.engine.audio
{
    public class AudioManager : IDisposable
    {
        private ALDevice device;
        private ALContext context;

        public AudioManager()
        {
            // Open default device
            device = ALC.OpenDevice(null);
            if (device == IntPtr.Zero)
                throw new Exception("Failed to open OpenAL device.");
            
            // Create context
            context = ALC.CreateContext(device, (int[])null);
            if (context == IntPtr.Zero)
                throw new Exception("Failed to open OpenAL context.");

            // Make context current
            if (!ALC.MakeContextCurrent(context))
                throw new Exception("Failed to make OpenAL context current.");
        }

        // Play a one-shot sound asynchronously
        public void Play(string filePath)
        {
            Task.Run(() =>
            {
                int buffer = 0;
                int source = 0;

                try
                {
                    // Load WAV data
                    byte[] soundData;
                    ALFormat format;
                    int sampleRate;

                    using (FileStream fs = File.OpenRead(filePath))
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        // Parse WAV header
                        reader.ReadBytes(12); // RIFF header
                        while (new string(reader.ReadChars(4)) != "fmt ") ;

                        int fmtChunkSize = reader.ReadInt32();
                        short audioFormat = reader.ReadInt16();
                        short numChannels = reader.ReadInt16();
                        sampleRate = reader.ReadInt32();
                        reader.ReadInt32(); // byte rate
                        reader.ReadInt16(); // block align
                        short bitsPerSample = reader.ReadInt16();
                        reader.ReadBytes(fmtChunkSize - 16); // skip extra bytes

                        format = GetSoundFormat(numChannels, bitsPerSample);

                        while (new string(reader.ReadChars(4)) != "data") ;
                        int dataSize = reader.ReadInt32();
                        soundData = reader.ReadBytes(dataSize);
                    }

                    buffer = AL.GenBuffer();
                    source = AL.GenSource();

                    unsafe
                    {
                        fixed (byte* p = soundData)
                        {
                            AL.BufferData(buffer, format, (IntPtr)p, soundData.Length, sampleRate);
                        }
                    }

                    AL.Source(source, ALSourcei.Buffer, buffer);
                    AL.SourcePlay(source);

                    // Wait until playback finishes, then clean up
                    ALSourceState state;
                    do
                    {
                        Task.Delay(100).Wait();

                        AL.GetSource(source, ALGetSourcei.SourceState, out int stateInt);
                        state = (ALSourceState)stateInt;

                    } while (state == ALSourceState.Playing);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AudioManager] Error playing audio: {ex.Message}");
                }
                finally
                {
                    if (source != 0)
                    {   
                        AL.SourceStop(source);
                        AL.DeleteSource(source);
                    }

                    if (buffer != 0)
                    {
                        AL.DeleteBuffer(buffer);
                    }
                }
            });
        }


        private ALFormat GetSoundFormat(int channels, int bits)
        {
            return (channels, bits) switch
            {
                (1, 8) => ALFormat.Mono8,
                (1, 16) => ALFormat.Mono16,
                (2, 8) => ALFormat.Stereo8,
                (2, 16) => ALFormat.Stereo16,
                _ => throw new NotSupportedException($"Unsupported WAV format: {channels} channels, {bits} bits")
            };
        }

        public void Dispose()
        {
            ALC.MakeContextCurrent(ALContext.Null);
            if (context != IntPtr.Zero)
                ALC.DestroyContext(context);
            if (device != IntPtr.Zero)
                ALC.CloseDevice(device);
        }
    }
}
