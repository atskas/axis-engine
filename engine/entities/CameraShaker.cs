using OpenTK.Mathematics;

namespace UntitledEngine.engine.entities
{
    public class CameraShaker
    {
        // This is a helper class.
        // It doesn't extend from camera since it just returns the needed values for camera shake.

        private Vector2 originalPosition;
        private float shakeIntensity;
        private float shakeDecay;
        private bool shaking = false;
        private Random random = new Random();

        public void StartShake(Vector2 camPos, float intensity, float decay)
        {
            originalPosition = camPos;
            shakeIntensity = intensity;
            shakeDecay = decay;
            shaking = true;
        }

        public Vector2 Update(float deltaTime)
        {
            if (!shaking) return originalPosition;

            if (shakeIntensity > 0f)
            {
                float offsetX = (float)(random.NextDouble() * 2 - 1) * shakeIntensity;
                float offsetY = (float)(random.NextDouble() * 2 - 1) * shakeIntensity;
                shakeIntensity -= shakeDecay * deltaTime;

                return originalPosition + new Vector2(offsetX, offsetY);
            }
            else
            {
                shaking = false;
                return originalPosition;
            }
        }
    }
}