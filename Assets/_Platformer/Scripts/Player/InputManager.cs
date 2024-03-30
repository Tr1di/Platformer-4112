using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Controls _controls;

    public Vector2 Move => _controls.Player.Move.ReadValue<Vector2>();

    public event Action onJump;

    private void Awake()
    {
        _controls = new Controls();
        _controls.Player.Jump.performed += _ => onJump?.Invoke();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void OnDestroy()
    {
        _controls.Dispose();
    }
}
