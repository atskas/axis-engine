using Silk.NET.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledEngine.Core.Entities;
using UntitledEngine.Core.Physics;

namespace UntitledEngine.Core.Components;

internal class PlayerController : Component
{
    public override void Update()
    {
        // Get current linear velocity
        var velocity = debugBody.Body.GetLinearVelocity();

        if (Engine.Instance.InputManager.KeyDown(Key.A))
        {
            velocity.X = -moveSpeed;
        }
        else if (Engine.Instance.InputManager.KeyDown(Key.D))
        {
            velocity.X = moveSpeed;
        }
        else
        {
            velocity.X = 0;
        }

        if (Engine.Instance.InputManager.KeyDown(Key.W))
        {
            if (IsGrounded(debugObject1))
                velocity.Y = jumpVelocity;
        }

        debugBody.Body.SetLinearVelocity(velocity);
    }

    // Grounded check
    public bool IsGrounded(Entity entity)
    {
        var body = entity.GetComponent<PhysicsBody>().Body;
        var position = body.GetPosition(); // This is already System.Numerics.Vector2

        bool isGrounded = false;
        float offset = 0.15f;
        float rayLength = debugObject1.Transform.Scale.Y / 1.18f;

        void CastRay(System.Numerics.Vector2 origin)
        {
            var start = origin;
            var end = origin + new System.Numerics.Vector2(0, -rayLength);

            PhysicsManager.Instance.Box2DWorld.RayCast((fixture, point, normal, fraction) =>
            {
                if (fixture.Body != body)
                    isGrounded = true;

            }, start, end);
        }

        CastRay(position);
        CastRay(position + new System.Numerics.Vector2(-offset, 0));
        CastRay(position + new System.Numerics.Vector2(+offset, 0));

        return isGrounded;
    }
}
