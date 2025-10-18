using UnityEngine;

namespace Assets.GearMind.Objects.Utils
{
    public class RigitBody2DState
    {
        public Vector2 Position { get; }
        public float Rotation { get; }
        public Vector2 LinearVelocity { get; }
        public float AngularVelocity { get; }

        public RigitBody2DState(
            Vector2 position,
            float rotation,
            Vector2 linearVelocity,
            float angularVelocity
        )
        {
            Position = position;
            Rotation = rotation;
            LinearVelocity = linearVelocity;
            AngularVelocity = angularVelocity;
        }
    }

    public static class RigitBody2DStateExtensions
    {
        public static RigitBody2DState GetState(this Rigidbody2D component) =>
            new(
                component.position,
                component.rotation,
                component.linearVelocity,
                component.angularVelocity
            );

        public static void SetState(this Rigidbody2D component, RigitBody2DState state)
        {
            component.position = state.Position;
            component.rotation = state.Rotation;
            if (component.bodyType != RigidbodyType2D.Static)
            {
                component.linearVelocity = state.LinearVelocity;
                component.angularVelocity = state.AngularVelocity;
            }
        }
    }
}
