using System;
using CodeBase.Controls;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace CodeBase.Interactions.Push
{
    public class PushObject : InteractObject
    {
        [SerializeField] private float _xSensitivity;
        [SerializeField] private float _ySensitivity;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private GameObject _putDownPoint;

        private float _startXSensitivity;
        private float _startYSensitivity;
        private float _startMovementSpeed;

        private PlayerPrefab _playerPrefab;
        private IInputService _inputService;
        private PlayerInputActions.PushActions _pushActions;
        
        [Inject]
        public void Construct(PlayerPrefab playerPrefab, IInputService inputService)
        {
            _playerPrefab = playerPrefab;
            _inputService = inputService;
            _pushActions = _inputService.PushActions;
        }

        private void Start()
        {
            _startXSensitivity = _playerPrefab.FirstPersonCharacterLookInput.mouseSensitivity.x;
            _startYSensitivity = _playerPrefab.FirstPersonCharacterLookInput.mouseSensitivity.y;
            _startMovementSpeed = _playerPrefab.FirstPersonCharacter.maxWalkSpeed;
        }

        private void OnDestroy()
        {
            _pushActions.Interact.performed -= PerformPushInteract;
            _pushActions.Interact.canceled -= PushInteractCancel;
        }

        private void Update()
        {
            if (!_inputService.IsPushInteractPressed) return;
            
            OnHoldInteract();
        }

        protected override void OnInteract()
        {
            _inputService.EnableActionMap(ActionMapType.Push);
            _inputService.DisableActionMap(ActionMapType.Player);
            
            _pushActions.Interact.canceled += PushInteractCancel;
            _pushActions.Interact.performed += PerformPushInteract;
        }

        protected override void OnHoldInteract()
        {
            SetupValues(_xSensitivity, _ySensitivity, _movementSpeed);
        }

        protected override void OnReleaseInteract(Action callback = null)
        {
            _playerPrefab.HeadBobbing.ToggleHeadBob(true);
            transform.SetParent(null);

            SetupValues(_startXSensitivity, _startYSensitivity, _startMovementSpeed);
            
            callback?.Invoke();
            
            _inputService.DisableActionMap(ActionMapType.Push);
            _inputService.EnableActionMap(ActionMapType.Player);
        }

        private void PerformPushInteract(InputAction.CallbackContext _)
        {
            _playerPrefab.HeadBobbing.ToggleHeadBob(false);
            transform.SetParent(_playerPrefab.PushPoint);
        }

        private void PushInteractCancel(InputAction.CallbackContext _)
        {
            _pushActions.Interact.performed -= PerformPushInteract;
            _pushActions.Interact.canceled -= PushInteractCancel;
            
            OnReleaseInteract();
        }

        private void SetupValues(float xSensitivity, float ySensitivity, float movementSpeed)
        {
            _playerPrefab.FirstPersonCharacterLookInput.mouseSensitivity.x = xSensitivity;
            _playerPrefab.FirstPersonCharacterLookInput.mouseSensitivity.y = ySensitivity;
            _playerPrefab.FirstPersonCharacter.maxWalkSpeed = movementSpeed;
        }
    }
}
