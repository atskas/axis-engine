using OpenTK.Mathematics;

namespace UntitledEngine.engine.entities
{
    public class Camera : BaseEntity
    {
        public float Zoom { get; set; } = 1f;

        private Vector2 originalPosition;
        private Vector2 shakeOffset = Vector2.Zero;
        private float shakeDuration = 0f;
        private float shakeIntensity = 0f;
        private readonly Random random = new Random();

        public Camera()
        {
            originalPosition = Transform.Position;
        }

        // Override Think() method so you don't have to specifically
        // call it in scene, you can call this instead
        public override void Think(float deltaTime)
        {
            base.Think(deltaTime);
            UpdateShake(deltaTime);
        }

        public void Shake(float duration, float intensity)
        {
            shakeDuration = duration;
            shakeIntensity = intensity;
            originalPosition = Transform.Position; // Save position when shake starts
        }

        private void UpdateShake(float deltaTime)
        {
            if (shakeDuration > 0f)
            {
                shakeDuration -= deltaTime;

                float offsetX = (float)(random.NextDouble() * 2 - 1) * shakeIntensity;
                float offsetY = (float)(random.NextDouble() * 2 - 1) * shakeIntensity;
                shakeOffset = new Vector2(offsetX, offsetY);

                Transform.Position = originalPosition + shakeOffset;

                if (shakeDuration <= 0f)
                {
                    // Shake ended, reset position
                    shakeDuration = 0f;
                    shakeOffset = Vector2.Zero;
                    Transform.Position = originalPosition;
                }
            }
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.CreateTranslation(-Transform.Position.X, -Transform.Position.Y, 0);
        }

        public Matrix4 GetProjectionMatrix(float aspectRatio)
        {
            float width = 2f / Zoom;
            float height = width / aspectRatio;
            return Matrix4.CreateOrthographic(width, height, -1, 1);
        }
    }
}
