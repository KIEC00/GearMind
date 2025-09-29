using System;
using UnityEngine;

namespace Assets.Utils.Runtime
{
    public struct Segment2
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        public readonly Vector2 Delta => End - Start;
        public readonly Vector2 Normalized => Delta.normalized;
        public readonly float SqrMagnitude => Delta.sqrMagnitude;
        public readonly float Magnitude => Delta.magnitude;
        public readonly Vector2 Middle => MiddlePoint(Start, End);
        public readonly int Count => 2;

        public Vector2 this[int index]
        {
            readonly get => Get(index);
            set => Set(index, value);
        }

        public Segment2(Vector2 Delta)
            : this(Vector2.zero, Delta) { }

        public Segment2(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public readonly Vector2 Lerp(float t) => Vector2.Lerp(Start, End, t);

        public readonly Vector2 LerpUnclamped(float t) => Vector2.LerpUnclamped(Start, End, t);

        public readonly float Angle(Vector2 from) => Vector2.Angle(from, Delta);

        public readonly float SignedAngle(Vector2 from) => Vector2.SignedAngle(from, Delta);

        public static Vector2 MiddlePoint(Vector2 start, Vector2 end) => start / 2 + end / 2;

        private readonly Vector2 Get(int index) =>
            index == 0 ? Start
            : index == 1 ? End
            : throw new IndexOutOfRangeException();

        private void Set(int index, Vector2 value)
        {
            if (index == 0)
                Start = value;
            else if (index == 1)
                End = value;
            else
                throw new IndexOutOfRangeException();
        }

        public static Segment2 operator +(Segment2 a, Segment2 b) =>
            new(a.Start + b.Start, a.End + b.End);

        public static Segment2 operator -(Segment2 a, Segment2 b) =>
            new(a.Start - b.Start, a.End - b.End);

        public readonly void Deconstruct(out Vector2 start, out Vector2 end)
        {
            start = Start;
            end = End;
        }

        public override readonly string ToString() => $"[{Start}, {End}]";
    }
}
