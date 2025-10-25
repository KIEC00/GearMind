using System.Collections.Generic;

namespace Assets.GearMind.State
{
    public class StateService : IStateService
    {
        private readonly Dictionary<IHaveState, EntityState> _entitiesStates = new();

        public void Register(IHaveState entity, bool saveState = true) =>
            _entitiesStates.Add(entity, saveState ? new(entity.GetState()) : new());

        public void Unregister(IHaveState entity, bool loadState = false)
        {
            if (!_entitiesStates.TryGetValue(entity, out var state))
                return;
            if (loadState && state)
                entity.SetState(state.Data);
            _entitiesStates.Remove(entity);
        }

        public void SaveStates()
        {
            foreach (var entity in _entitiesStates.Keys)
                _entitiesStates[entity] = new(entity.GetState());
        }

        public void LoadStates()
        {
            foreach (var (entity, state) in _entitiesStates)
                if (state)
                    entity.SetState(state.Data);
        }

        private readonly struct EntityState
        {
            public readonly bool IsInitialized;
            public readonly object Data;

            public static implicit operator bool(EntityState entity) => entity.IsInitialized;

            public EntityState(object data)
            {
                Data = data;
                IsInitialized = true;
            }
        }
    }
}
