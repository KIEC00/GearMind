using System;
using UnityEngine;

namespace Assets.GearMind.Input
{
    public interface IInputService
    {
        bool Enabled { get; set; }
        void Enable();
        void Disable();

        bool IsPointerDown { get; }
        event Action<Vector2> OnPointerReleased;
        event Action<Vector2> OnPointerPressed;
        event Action<Vector2> OnPointerClick;

        bool IsPointerAltDown { get; }
        event Action<Vector2> OnPointerAltReleased;
        event Action<Vector2> OnPointerAltPressed;
        event Action<Vector2> OnPointerAltClick;

        Vector2 PointerPosition { get; }
        Vector2 PointerDelta { get; }
        event Action<PointerMove> OnPointerMove;

        bool IsDraging { get; }
        event Action<Vector2> OnDragStart;
        event Action<PointerMove> OnDrag;
        event Action<Vector2> OnDragEnd;

        event Action OnEscPressed;
    }

    public readonly struct PointerMove
    {
        public readonly Vector2 Previous => Current - Delta;
        public readonly Vector2 Current;
        public readonly Vector2 Delta;

        public PointerMove(Vector2 current, Vector2 delta)
        {
            Current = current;
            Delta = delta;
        }

        public override string ToString() => $"({Previous} => {Current}, Delta: {Delta})";
    }
}
