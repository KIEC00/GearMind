using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GearMind.Grid.Components;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    [SerializeField] private GridComponent _gridComponent;
    [SerializeField] private GridController _gridController;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _stopButton;

    [SerializeField]
    private bool _preventEditDuringSimulation = true;

    public bool IsSimulating => _isSimulating;

    private readonly Dictionary<GameObject, GameObjectState> _sceneState = new();
    private readonly HashSet<GameObject> _objects = new();
    private readonly List<GameObject> _objectsToDestroy = new(); 

    private bool _isSimulating = false;

    void Awake()
    {
        if (_startButton) _startButton.onClick.AddListener(StartSimulation);
        if (_stopButton) _stopButton.onClick.AddListener(StopSimulation);
    }

    [Serializable]
    private class GameObjectState
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public bool IsPlacedOnGrid; 

        public RigidbodyState RigidbodyState;
        public ColliderState[] ColliderStates;
    }

    [Serializable]
    private class RigidbodyState
    {
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        public bool UseGravity;
        public bool IsKinematic;
    }

    [Serializable]
    private class ColliderState
    {
        public Collider Collider;
        public bool Enabled;
        public bool IsTrigger;
    }

    public void SaveSceneState()
    {
        _sceneState.Clear();
        _objects.Clear();
        _objectsToDestroy.Clear();


        var gridItems = _gridComponent?.GetAllItems()?.Select(item => item.Component as MonoBehaviour) 
            ?? Array.Empty<MonoBehaviour>();
        var placedObjects = new HashSet<GameObject>(gridItems.Where(x => x != null).Select(x => x.gameObject));

        var allRigidbodies = FindObjectsOfType<Rigidbody>(true);
        foreach (var rb in allRigidbodies)
        {
            var rbObject = rb.gameObject;
            if (_sceneState.ContainsKey(rbObject)) continue;

            bool isPlacedOnGrid = placedObjects.Contains(rbObject);

            var state = new GameObjectState
            {
                Position = rbObject.transform.position,
                Rotation = rbObject.transform.rotation,
                IsPlacedOnGrid = isPlacedOnGrid,
                RigidbodyState = new RigidbodyState
                {
                    Velocity = rb.linearVelocity,
                    AngularVelocity = rb.angularVelocity,
                    UseGravity = rb.useGravity,
                    IsKinematic = rb.isKinematic
                },
                ColliderStates = rbObject.GetComponents<Collider>()
                                   .Select(c => new ColliderState { Collider = c, Enabled = c.enabled, IsTrigger = c.isTrigger })
                                   .ToArray()
            };

            _sceneState.Add(rbObject, state);
            _objects.Add(rbObject);


            if (!isPlacedOnGrid)
            {
                _objectsToDestroy.Add(rbObject);
            }
        }

        var allGridItems = FindObjectsOfType<AbstractGridItemComponent>(true);
        foreach (var gridItem in allGridItems)
        {
            var gridObject = gridItem.gameObject;
            if (_sceneState.ContainsKey(gridObject)) continue;

            bool isPlacedOnGrid = placedObjects.Contains(gridObject);

            var state = new GameObjectState
            {
                Position = gridObject.transform.position,
                Rotation = gridObject.transform.rotation,
                IsPlacedOnGrid = isPlacedOnGrid,
                RigidbodyState = null,
                ColliderStates = gridObject.GetComponents<Collider>()
                                   .Select(c => new ColliderState { Collider = c, Enabled = c.enabled, IsTrigger = c.isTrigger })
                                   .ToArray()
            };

            _sceneState.Add(gridObject, state);
            _objects.Add(gridObject);


            if (!isPlacedOnGrid)
            {
                _objectsToDestroy.Add(gridObject);
            }
        }
    }

    public void StartSimulation()
    {
        if (_isSimulating) return;

        SaveSceneState();


        foreach (var objToDestroy in _objectsToDestroy)
        {
            if (objToDestroy != null)
            {
                Destroy(objToDestroy);
            }
        }
        _objectsToDestroy.Clear();

        if (_gridController != null && _preventEditDuringSimulation)
            _gridController.enabled = false; // Хрень, переделать

        foreach (var kv in _sceneState)
        {
            var go = kv.Key;
            var state = kv.Value;


            if (go == null) continue;

            if (state.ColliderStates != null)
            {
                foreach (var cState in state.ColliderStates)
                    if (cState?.Collider != null) cState.Collider.enabled = true;
            }

            var rb = go.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.detectCollisions = true;
            }
        }

        _isSimulating = true;
    }

    public void StopSimulation()
    {
        if (!_isSimulating) return;

        foreach (var objects in _sceneState)
        {
            var gameObject = objects.Key;
            var state = objects.Value;
            if (gameObject == null) continue;

            gameObject.transform.position = state.Position;
            gameObject.transform.rotation = state.Rotation;

            if (state.ColliderStates != null)
            {
                foreach (var cState in state.ColliderStates)
                    if (cState?.Collider != null)
                    {
                        cState.Collider.enabled = cState.Enabled;
                        cState.Collider.isTrigger = cState.IsTrigger;
                    }
            }

            var rb = gameObject.GetComponent<Rigidbody>();
            if (rb != null && state.RigidbodyState != null)
            {
                rb.linearVelocity = state.RigidbodyState.Velocity;
                rb.angularVelocity = state.RigidbodyState.AngularVelocity;
                rb.useGravity = state.RigidbodyState.UseGravity;
                rb.isKinematic = state.RigidbodyState.IsKinematic;
            }
        }

        if (_gridController != null && _preventEditDuringSimulation)
             _gridController.enabled = true; // Хрень, переделать

        _isSimulating = false;
    }
}