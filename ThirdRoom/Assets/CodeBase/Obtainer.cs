using System;
using CodeBase.Controls;
using CodeBase.Data;
using CodeBase.Interactions;
using CodeBase.Inventory.Controller;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace CodeBase
{
    [DisallowMultipleComponent]
    public class Obtainer : InteractObject
    {
        public static event Action OnDeductiveObjectClaimed;

        [Header("Components")]
        [SerializeField] private bool _isScalingObject;
        [SerializeField] private bool _isDeductiveObject;
        [SerializeField] private bool _isRotatingObject;

        [Header("Object Types")]
        [SerializeField] private bool _isLetter;
        [SerializeField] private bool _isStick;

        [Space(10)]
        [SerializeField] private float _returnSpeed = 1f;
        
        [Space(10)]
        [Header("Full Description")]
        [SerializeField] private bool _hasFullDescription;
        [SerializeField] private string _title;
        [TextArea(3, 10)]
        [SerializeField] private string _fullDescription;

        public Vector3 StartPosition { get; private set; }
        public Vector3 StartRotation { get; private set; }

        private Transform _trailerDisposePosterPosition;
        private Transform _selectedTransform;
        
        private ObtainerUI _obtainerUI;
        private PlayerPrefab _playerPrefab;
        private InventoryController _inventoryController;
        private ObjectRotation _objectRotation;
        private IInputService _inputService;

        private bool _isInspecting;
        private Item _currentItem;
        private string _currentItemPath;

        [Inject]
        public void Construct(PlayerPrefab playerPrefab, InventoryController inventoryController,
            ObjectRotation objectRotation, IInputService inputService, CentralUI centralUI)
        {
            _playerPrefab = playerPrefab;
            _inventoryController = inventoryController;
            _objectRotation = objectRotation;
            _inputService = inputService;
            _obtainerUI = centralUI.ObtainerUI;
        }

        private void OnValidate()
        {
            _objectRotation = FindFirstObjectByType<ObjectRotation>();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        private void Start()
        {
            StartPosition = transform.position;
            StartRotation = transform.rotation.eulerAngles;
            _selectedTransform = transform;
            
            _obtainerUI.OnDestroyRequested += OnDestroyRequested;
            _obtainerUI.OnMoveToStashRequested += OnMoveToStashRequested;
        }

        private void OnDestroy()
        {
            _obtainerUI.OnDestroyRequested -= OnDestroyRequested;
            _obtainerUI.OnMoveToStashRequested -= OnMoveToStashRequested;
        }

        protected override void OnInteract()
        {
            Obtain();
        }

        protected void SetSelectedTransform(Transform selectedTransform)
            => _selectedTransform = selectedTransform;
        
        private void Update()
        {
            if (!_isInspecting) return;

            if (_inputService.IsObtainerTakePressed && !_obtainerUI.IsFullDescriptionVisible)
            {
                ConfirmObtain();
            }

            if (_inputService.IsObtainerReadPressed && _hasFullDescription && !_obtainerUI.IsFullDescriptionVisible)
            {
                _obtainerUI.ToggleFullDescriptionCanvas(true, _title, _fullDescription);
                _objectRotation.CanEscapeInput = false;
                _obtainerUI.ToggleMainCanvas(false);
            }

            if (_inputService.IsObtainerEscapePressed)
            {
                if (_obtainerUI.IsFullDescriptionVisible)
                {
                    _obtainerUI.ToggleFullDescriptionCanvas(false);
                    _obtainerUI.ToggleMainCanvas(true);
                    _objectRotation.CanEscapeInput = true;
                }
                else
                {
                    CancelInspecting();
                }
            }
        }

        private void Obtain()
        {
            _inputService.DisableActionMap(ActionMaps.Player);
            _inputService.EnableActionMap(ActionMaps.ObtainerUI);
            _objectRotation.Activate(_selectedTransform, _isScalingObject, null, MoveToCameraPlain);
            _playerPrefab.BlockInput();
            _objectRotation.CanEscapeInput = true;
            RotateToCamera();

            _currentItemPath = _isStick ? "InventoryObjects/Stick" :
                               _isLetter ? "InventoryObjects/Letter" : string.Empty;

            if (!string.IsNullOrEmpty(_currentItemPath))
            {
                _currentItem = LoadItem(_currentItemPath);
                _obtainerUI.Display(_selectedTransform, _currentItem);
            }

            _isInspecting = true;
        }

        private void ConfirmObtain()
        {
            if (_currentItem == null) return;

            _inputService.DisableActionMap(ActionMaps.ObtainerUI);
            _inputService.EnableActionMap(ActionMaps.Player);
            _inventoryController.AddItem(_currentItem);
            Debug.Log($"{_currentItem.name} added to inventory.");
            _obtainerUI.ToggleMainCanvas(false);
            _isInspecting = false;
            _playerPrefab.UnblockInput();
            _objectRotation.SelectedObject = null;
            Destroy(gameObject);
            Destroy(_selectedTransform.gameObject);
        }

        private void CancelInspecting()
        {
            _inputService.DisableActionMap(ActionMaps.ObtainerUI);
            _inputService.EnableActionMap(ActionMaps.Player);
            _obtainerUI.ToggleMainCanvas(false);
            _objectRotation.Deactivate();
            _playerPrefab.UnblockInput();
            _isInspecting = false;
        }

        private void MoveToCameraPlain()
        {
            if (Camera.main == null) return;
            transform.DOLocalMove(new Vector3(0, 0, -0.62f), 1f);
        }

        private void RotateToCamera()
        {
            if (Camera.main == null) return;

            Vector3 directionToCamera = Camera.main.transform.position - transform.position;
            directionToCamera.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            _selectedTransform.DORotateQuaternion(targetRotation, 1f).SetEase(Ease.InOutSine);
        }

        private Item LoadItem(string path)
        {
            return Resources.Load<Item>(path);
        }

        private void OnMoveToStashRequested(Transform target)
        {
            _objectRotation.Deactivate();
            target.SetParent(null);
            target.DORotate(StartRotation, 1f);
            target.DOMove(_trailerDisposePosterPosition.position, 0.5f)
                  .OnComplete(() => Destroy(target.gameObject));

            if (_isDeductiveObject)
                OnDeductiveObjectClaimed?.Invoke();
        }

        private void OnDestroyRequested()
        {
            _objectRotation.Deactivate();
        }
    }
}
