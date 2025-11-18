using System;
using Assets.GearMind.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;

public class InputCutRope: IInputCutRope
{
    private IInputService _inputService;

    public event Action InputEvent;

    public InputCutRope (IInputService inputService)
    {
        _inputService = inputService;
        _inputService.Enable();
    }

    public bool GetIsDraging()
    {
        return _inputService.IsPointerDown;
    }
}

public interface IInputCutRope
{
    public event Action InputEvent;
}
