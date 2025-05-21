using OpenTK.Mathematics;

namespace UntitledEngine.engine.entities
{
    public class CameraShaker
    {
        private Vector2 originalPosition;
        private float timer = 0f;
        private float duration = 0f;
        private float intensity = 0f;
        private float decay = 0f;

        private readonly Random random = new Random();

        public void Shake(ref Vector2 camPos, float intensity, float duration, float deltaTime)
        {
            // Start shaking if not already
            if (timer <= 0f)
            {
                originalPosition = camPos;
                this.intensity = intensity;
                this.duration = duration;
                decay = intensity / duration;
                timer = duration;
            }

            // Update if still shaking
            if (timer > 0f)
            {
                float offsetX = (float)(random.NextDouble() * 2 - 1) * this.intensity;
                float offsetY = (float)(random.NextDouble() * 2 - 1) * this.intensity;

                this.intensity -= decay * deltaTime;
                timer -= deltaTime;

                camPos = originalPosition + new Vector2(offsetX, offsetY);
            }
            else
            {
                camPos = originalPosition;
            }
        }
    }
}
