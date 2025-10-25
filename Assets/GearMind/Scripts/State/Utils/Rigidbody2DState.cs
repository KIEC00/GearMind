using UnityEngine;

namespace Assets.GearMind.State.Utils
{
    public class Rigidbody2DState
    {
        public Vector2 Position { get; }
        public float Rotation { get; }
        public Vector2 LinearVelocity { get; }
        public float AngularVelocity { get; }

        public Rigidbody2DState(
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

    public static class Rigitbody2DStateExtensions
    {
        public static Rigidbody2DState GetState(this Rigidbody2D component) =>
            new(
                component.position,
                component.rotation,
                component.linearVelocity,
                component.angularVelocity
            );

        public static void SetState(this Rigidbody2D component, Rigidbody2DState state)
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
