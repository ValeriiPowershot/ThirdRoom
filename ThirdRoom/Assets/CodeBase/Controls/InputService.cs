using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Controls
{
    public class InputService : IInputService, IDisposable
    {
        private readonly PlayerInputActions _inputActions;
        
        public InputService()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Player.Enable();
        }
        
        public Vector2 MoveDirection => _inputActions.Player.Move.ReadValue<Vector2>();
        public bool IsJumpPressed => _inputActions.Player.Jump.WasPressedThisFrame();
        public bool IsFirePressed => _inputActions.Player.Fire.WasPressedThisFrame();
        public bool IsInteractPressed => _inputActions.Player.Interact.WasPressedThisFrame();
        public bool IsObtainerReadPressed => _inputActions.ObtainerUI.Reading.WasPressedThisFrame();
        public bool IsObtainerTakePressed => _inputActions.ObtainerUI.Take.WasPressedThisFrame();
        public bool IsObtainerEscapePressed => _inputActions.ObtainerUI.Escape.WasPressedThisFrame();

        public void EnableActionMap(string actionMapName)
        {
            InputActionMap actionMap = GetActionMap(actionMapName);
            if (actionMap != null)
            {
                actionMap.Enable();
            }
            else
            {
                Debug.LogWarning($"Action Map '{actionMapName}' not found.");
            }
        }

        public void DisableActionMap(string actionMapName)
        {
            InputActionMap actionMap = GetActionMap(actionMapName);
            if (actionMap != null)
            {
                actionMap.Disable();
            }
            else
            {
                Debug.LogWarning($"Action Map '{actionMapName}' not found.");
            }
        }

        public void DisableAllActionMaps()
        {
            _inputActions.Disable();
        }

        public PlayerInputActions GetPlayerInputActions()
        {
            return _inputActions;
        }

        public void Dispose()
        {
            _inputActions.Player.Disable();
            _inputActions.Dispose();
        }
        
        private InputActionMap GetActionMap(string actionMapName)
        {
            return _inputActions.asset.FindActionMap(actionMapName, throwIfNotFound: false);
        }
    }
}