using System;
using CodeBase.Data;
using CodeBase.Interactions;
using CodeBase.Inventory;
using CodeBase.Inventory.Controller;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace CodeBase
{
    [DisallowMultipleComponent]
    public class Obtainer : InteractObject
    {
        public static event Action OnDeductiveObjectClaimed;

        [Header("Components")]
        [SerializeField] private ObtainerUI _obtainerUI;
        [SerializeField] private ObjectRotation _objectRotation;
        [SerializeField] private bool _isScalingObject;
        [SerializeField] private bool _isDeductiveObject;
        [SerializeField] private bool _isRotatingObject;
        
        [Header("Object Types")]
        [SerializeField] private bool _isLetter;
        [SerializeField] private bool _isStick;
        
        [Space(10)]
        [SerializeField] private float _returnSpeed;
        
        public Vector3 StartPosition { get; set; }
        public Vector3 StartRotation { get; set; }

        private Transform _trailerDisposePosterPosition;

        private PlayerPrefab _playerPrefab;
        private InventoryController _inventoryController;
        
        [Inject]
        public void Construct(PlayerPrefab playerPrefab, InventoryController inventoryController)
        {
            _playerPrefab = playerPrefab;
            _inventoryController = inventoryController;
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
            StartPosition = gameObject.transform.position;
            StartRotation = gameObject.transform.rotation.eulerAngles;
            
            _obtainerUI.OnDestroyRequested += OnDestroyRequested;
            _obtainerUI.OnMoveToStashRequested += OnMoveToStashRequested;
        }

        private void OnDestroy()
        {
            _obtainerUI.OnDestroyRequested -= OnDestroyRequested;
            _obtainerUI.OnMoveToStashRequested -= OnMoveToStashRequested;
        }
        
        protected override void OnInteract()
            => Obtain();

        private void Obtain()
        {
            
            if (_isStick)
            {
                _objectRotation.Activate(transform, _isScalingObject, MoveToCameraPlan);
                _playerPrefab.BlockInput();
                RotateToCamera();
                
                _inventoryController.AddItem(Resources.Load<Item>("InventoryObjects/Stick"));
                
                _obtainerUI.Display(transform, LoadItem("InventoryObjects/Stick"));
            }
            if (_isLetter)
            {
                _objectRotation.Activate(transform, _isScalingObject, MoveToCameraPlan);
                _playerPrefab.BlockInput();
                RotateToCamera();
                
                _inventoryController.AddItem(Resources.Load<Item>("InventoryObjects/Letter"));
                
                _obtainerUI.Display(transform, LoadItem("InventoryObjects/Stick"));
            }
            else
            {
                _objectRotation.Activate(transform, _isScalingObject);
                _playerPrefab.BlockInput();
                RotateToCamera();
            }
        }

        private void MoveToCameraPlan()
        {
            if(Camera.main == null) return;
            transform.DOLocalMove(new Vector3(0,0,-0.62f), 1f);
        }
        
        private void RotateToCamera()
        {
            if (Camera.main == null) return;

            Vector3 directionToCamera = Camera.main.transform.position - transform.position;

            directionToCamera.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

            transform.DORotateQuaternion(targetRotation, 1f).SetEase(Ease.InOutSine);
        }

        private void ZoomCamera()
        {
            transform.DOScale(transform.localScale * 1.5f, 0.4f).SetEase(Ease.InOutSine);
        }

        private Item LoadItem(string path)
        {
            Item item = Resources.Load<Item>("InventoryObjects/Letter");
            return item;
        }
        
        private void OnMoveToStashRequested(Transform target)
        {
            print("MOVE TO SHTASH!!!");
            _objectRotation.Deactivate();
            target.SetParent(null);
            target.DORotate(StartRotation, 1f);
            target.DOMove(_trailerDisposePosterPosition.position, 0.5f).OnComplete(() => Destroy(target.gameObject));
            if (_isDeductiveObject)
            {
                OnDeductiveObjectClaimed?.Invoke();
            }
        }
        
        private void OnDestroyRequested()
        {
            _objectRotation.Deactivate();
        }
    }
}