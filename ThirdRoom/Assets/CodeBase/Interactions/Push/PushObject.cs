using CodeBase.Controls;
using CodeBase.Logic;
using DG.Tweening;
using UnityEngine;
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
        private bool _inPlacementZone;
        private PlayerPrefab _playerPrefab;
        private IInputService _inputService;

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

        private void Update()
        {
            if (_inputService.IsPushInteractPressed && _canPutDown)
            {
                _playerPrefab.HeadBobbing.ToggleHeadBob(true);
                
                transform.Unparent();
                transform.DOMove(_putDownPoint.transform.position,  2);
                transform.DORotate(_putDownPoint.transform.eulerAngles, 2);
                
                SetupValues(_startXSensitivity, _startYSensitivity, _startMovementSpeed);
                
                _inputService.DisableActionMap(ActionMaps.Push);
                _inputService.EnableActionMap(ActionMaps.Player);
            }
            if (_inputService.IsPushInteractPressed && !_canPutDown)
            {
                _playerPrefab.HeadBobbing.ToggleHeadBob(true);
                
                transform.Unparent();
                SetupValues(_startXSensitivity, _startYSensitivity, _startMovementSpeed);
                
                _inputService.DisableActionMap(ActionMaps.Push);
                _inputService.EnableActionMap(ActionMaps.Player);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                Debug.Log("asd");
                _canInteract = true;
            }

            if (other.CompareTag(Tags.PlacementZone)) 
                _canPutDown = true;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Player)) 
                _canInteract = false;
        }

        protected override void OnInteract()
        {
            Debug.Log("dfd");
            
            if (_canInteract && !_canPutDown)
            {
                _inputService.DisableActionMap(ActionMaps.Player);
                _inputService.EnableActionMap(ActionMaps.Push);
                
                _playerPrefab.HeadBobbing.ToggleHeadBob(false);
                
                transform.SetParent(_playerPrefab.PushPoint);
                transform.localPosition = new Vector3(0,transform.localPosition.y,0);
                transform.localEulerAngles = Vector3.zero;
                
                SetupValues(_XSensitivity, _YSensitivity, _movementSpeed);
            }
        }

        private void SetupValues(float xSensitivity, float ySensitivity, float movementSpeed)
        {
            _playerPrefab.FirstPersonCharacterLookInput.mouseSensitivity.x = xSensitivity;
            _playerPrefab.FirstPersonCharacterLookInput.mouseSensitivity.y = ySensitivity;
            _playerPrefab.FirstPersonCharacter.maxWalkSpeed = movementSpeed;
        }
        
    }
}
