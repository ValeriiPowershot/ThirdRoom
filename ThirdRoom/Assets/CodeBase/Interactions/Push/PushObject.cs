using System;
using CodeBase.Controls;
using CodeBase.Logic;
using UnityEngine;
using DG.Tweening;
using Zenject;

namespace CodeBase.Interactions.Push
{
    public class PushObject : InteractObject
    {
        [SerializeField] private float _XSensitivity;
        [SerializeField] private float _YSensitivity;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private GameObject _putDownPoint;

        private float _startXSensitivity;
        private float _startYSensitivity;
        private float _startMovementSpeed;

        private bool _canInteract;
        private bool _canPutDown;
        private PlayerPrefab _playerPrefab;
        private IInputService _inputService;
        private bool _isHolding;

        [Inject]
        public void Construct(PlayerPrefab playerPrefab, IInputService inputService)
        {
            _playerPrefab = playerPrefab;
            _inputService = inputService;
        }

        private void Start()
        {
            _startXSensitivity = _playerPrefab.FirstPersonCharacterLookInput.mouseSensitivity.x;
            _startYSensitivity = _playerPrefab.FirstPersonCharacterLookInput.mouseSensitivity.y;
            _startMovementSpeed = _playerPrefab.FirstPersonCharacter.maxWalkSpeed;
        }

        protected override void OnInteract()
        {
            print("ZALUPA");
            
            _inputService.EnableActionMap(ActionMapType.Push);
            _inputService.DisableActionMap(ActionMapType.Player);
        }

        private void Update()
        {
            
            if (_inputService.IsPushInteractPressed)
            {
                if (!_isHolding)
                {
                    _isHolding = true;

                    OnHoldInteract();
                }
            }
            else
            {
                if (_isHolding)
                {
                    _isHolding = false;

                    OnReleaseInteract();
                }
            }
        }

        protected override void OnHoldInteract()
        {
            _isHolding = true;
            _playerPrefab.HeadBobbing.ToggleHeadBob(false);
            transform.SetParent(_playerPrefab.PushPoint);

            SetupValues(_XSensitivity, _YSensitivity, _movementSpeed);
        }

        protected override void OnReleaseInteract(Action callback = null)
        {
                _isHolding = false;
                
                _playerPrefab.HeadBobbing.ToggleHeadBob(true);
                transform.SetParent(null);

                SetupValues(_startXSensitivity, _startYSensitivity, _startMovementSpeed);
                
                callback?.Invoke();
                
                _inputService.DisableActionMap(ActionMapType.Push);
                _inputService.EnableActionMap(ActionMapType.Player);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
                _canInteract = true;

            if (other.CompareTag(Tags.PlacementZone))
                _canPutDown = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                Debug.Log("Проебали");
                _canInteract = false;
            }

            if (other.CompareTag(Tags.PlacementZone))
                _canPutDown = false;
        }

        private void SetupValues(float xSensitivity, float ySensitivity, float movementSpeed)
        {
            _playerPrefab.FirstPersonCharacterLookInput.mouseSensitivity.x = xSensitivity;
            _playerPrefab.FirstPersonCharacterLookInput.mouseSensitivity.y = ySensitivity;
            _playerPrefab.FirstPersonCharacter.maxWalkSpeed = movementSpeed;
        }
    }
}
