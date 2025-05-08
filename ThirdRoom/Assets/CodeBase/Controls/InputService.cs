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

        #region IInputManager

        public void EnableActionMap(ActionMapType actionMap)
        {
            InputActionMap map = GetActionMap(actionMap);
            if (map != null)
            {
                map.Enable();
            }
            else
            {
                Debug.LogWarning($"Action Map '{actionMap}' not found.");
            }
        }

        public void DisableActionMap(ActionMapType actionMap)
        {
            InputActionMap map = GetActionMap(actionMap);
            if (map != null)
            {
                map.Disable();
            }
            else
            {
                Debug.LogWarning($"Action Map '{actionMap}' not found.");
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

        #endregion

        #region IPlayerInput

        public Vector2 MoveDirection => _inputActions.Player.Move.ReadValue<Vector2>();
        public bool IsJumpPressed => _inputActions.Player.Jump.WasPressedThisFrame();
        public bool IsFirePressed => _inputActions.Player.Fire.WasPressedThisFrame();
        public bool IsInteractPressed => _inputActions.Player.Interact.WasPressedThisFrame();
        public bool IsOpenInventoryPressed => _inputActions.Player.OpenInventory.WasPressedThisFrame();

        #endregion
        
        #region IInventoryInput

        public bool IsNextButtonPressed => _inputActions.Inventory.NextItem.WasPressedThisFrame();
        public bool IsPreviousButtonPressed => _inputActions.Inventory.PreviousItem.WasPressedThisFrame();
        public bool IsCloseInventoryPressed => _inputActions.Inventory.CloseInventory.WasPressedThisFrame();
        public bool IsSelectInventoryPressed => _inputActions.Inventory.Select.WasPressedThisFrame();
        #endregion
        
        #region IObtainerInput

        public bool IsObtainerReadPressed => _inputActions.ObtainerUI.Reading.WasPressedThisFrame();
        public bool IsObtainerTakePressed => _inputActions.ObtainerUI.Take.WasPressedThisFrame();
        public bool IsObtainerEscapePressed => _inputActions.ObtainerUI.Escape.WasPressedThisFrame();

        #endregion

        #region IPushInput

        public PlayerInputActions.PushActions PushActions => _inputActions.Push;
        public Vector2 PushDirection => _inputActions.Push.Move.ReadValue<Vector2>();
        public bool IsPushInteractPressed => _inputActions.Push.Interact.WasPressedThisFrame();
        public bool IsPushInteractReleased => _inputActions.Push.Interact.WasReleasedThisFrame();

        #endregion

        #region IInputService

        public Vector2 CurrentMoveDirection
        {
            get
            {
                if (_inputActions.Player.enabled)
                    return _inputActions.Player.Move.ReadValue<Vector2>();
                if (_inputActions.Push.enabled)
                    return _inputActions.Push.Move.ReadValue<Vector2>();
                return Vector2.zero;
            }
        }

        #endregion

        public void Dispose()
        {
            _inputActions.Disable();
            _inputActions.Dispose();
        }
        private InputActionMap GetActionMap(ActionMapType actionMap)
        {
            return _inputActions.asset.FindActionMap(actionMap.ToString(), throwIfNotFound: false);
        }
    }
}