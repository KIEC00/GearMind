using System;

namespace Assets.GearMind.Level
{
    public interface ILevelGoalTrigger
    {
        public event Action Trigger;
    }
}
