using OpenTK.Mathematics;

namespace UntitledEngine.engine.entities
{
    public class CameraShaker
    {
        private Vector2 originalPos;
        private readonly Random random = new Random();

        public CameraShaker(ref Camera camera)
        {
            originalPos = camera.Transform.Position;
        }

        public void StartShaking(ref Vector2 camPos, float intensity)
        {
            float offsetX = (float)(random.NextDouble() * 2 - 1) * intensity;
            float offsetY = (float)(random.NextDouble() * 2 - 1) * intensity;

            camPos += new Vector2(offsetX, offsetY);
        }

        public void StopShaking(ref Vector2 camPos)
        {
            camPos = originalPos;
        }
    }
}
