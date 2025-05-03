using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace CodeBase
{
    [DisallowMultipleComponent]
    public class ObjectRotation : MonoBehaviour
    {
        private const string VerticalAxis = "Mouse Y";
        private const string HorizontalAxis = "Mouse X";

        [SerializeField] private float _selectedObjectScaleDuration = 1.0f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private bool _isActivate;
        [SerializeField] private bool _isScalingObject;

        public Transform SelectedObject { get; set; }

        public bool CanEscapeInput;
        
        private Transform _selectedDisposePosition;

        private Vector3 _lastMousePosition;
        private Vector2 _rotationInput;

        private PlayerPrefab _playerPrefab;
        
        private Vector3 _selectedObjectScale = new(0.24f, 0.3897f, 0.0032f);

        [Inject]
        public void Construct(PlayerPrefab playerPrefab)
        {
            _playerPrefab = playerPrefab;
        }
        
        private void Update()
        {
            HandleEscapeInput();
            HandleMouseInput();
        }

        public void Activate(Transform selectedObject, bool isScaling, Transform parentObject = null, Action onMoved = null)
        {
            _isActivate = true;
            SelectedObject = selectedObject;
            
            if (isScaling) 
                SelectedObject.DOScale(_selectedObjectScale, _selectedObjectScaleDuration);

            if (parentObject == null) 
                SelectedObject.SetParent(_playerPrefab.InventoryPoint);
            else
                selectedObject.SetParent(parentObject);

            SelectedObject.DOLocalMove(Vector3.zero, 1f).OnComplete(() =>
            {
                _isActivate = true;
                onMoved?.Invoke();
            });

        }
        
        public void Deactivate()
        {
            SelectedObject = null;
            _isActivate = false;
        }
        
        private void HandleEscapeInput()
        {
            if(!CanEscapeInput)
                return;
            
            if (!Input.GetKeyDown(KeyCode.Escape))
                return;

            if (SelectedObject == null)
                return;

            Obtainer obtainer = SelectedObject.GetComponent<Obtainer>();

            ResetSelectedObjectTransform(obtainer);

            _playerPrefab.UnblockInput();
            Deactivate();
        }

        private void HandleMouseInput()
        {
            if (!Input.GetMouseButton(0))
                return;
    
            if (!_isActivate)
                return;

            _rotationInput.x = Input.GetAxis(HorizontalAxis);
            _rotationInput.y = Input.GetAxis(VerticalAxis);

            RotateObject();
        }

        private void ResetSelectedObjectTransform(Obtainer obtainer)
        {
            SelectedObject.SetParent(null, true);
    
            DOTween.Kill(SelectedObject.transform);
    
            SelectedObject.DOMove(obtainer.StartPosition, 1);
            SelectedObject.DORotate(obtainer.StartRotation, 1);
        }
        
        private void RotateObject()
        {
            float rotationX = _rotationInput.y * _rotationSpeed * Time.deltaTime;
            float rotationY = -_rotationInput.x * _rotationSpeed * Time.deltaTime;
            SelectedObject.Rotate(Camera.main.transform.up, rotationY, Space.World);
            SelectedObject.Rotate(Camera.main.transform.right, rotationX, Space.World);
        }
    }
}
