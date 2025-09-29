using System;
using UnityEngine;

namespace Assets.Utils.Runtime
{
    public abstract class TimeCache<T>
    {
        public T Value => IsDirty ? UpdateValue() : _cacheValue;
        public abstract bool IsDirty { get; }

        protected T _cacheValue;
        protected float _timestamp;
        protected readonly Func<T> _valueUpdater;

        public TimeCache(Func<T> valueUpdater)
        {
            _valueUpdater = valueUpdater;
            MarkAsDirty();
        }

        public void MarkAsDirty() => _timestamp = -1f;

        private T UpdateValue()
        {
            _cacheValue = _valueUpdater();
            SetTimeStamp();
            return _cacheValue;
        }

        protected abstract void SetTimeStamp();
    }

    public class FrameCache<T> : TimeCache<T>
    {
        public FrameCache(Func<T> valueUpdater)
            : base(valueUpdater) { }

        public override bool IsDirty => Time.time != _timestamp;

        protected override void SetTimeStamp() => _timestamp = Time.time;
    }

    public class FixedCache<T> : TimeCache<T>
    {
        public FixedCache(Func<T> valueUpdater)
            : base(valueUpdater) { }

        public override bool IsDirty => Time.fixedTime != _timestamp;

        protected override void SetTimeStamp() => _timestamp = Time.fixedTime;
    }
}
