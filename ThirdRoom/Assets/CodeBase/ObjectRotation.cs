using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private Transform _pocketPoint;
        
        public Rigidbody Rigidbody;

        
        private Transform _selectedDisposePosition;
        private Transform _selectedObject;

        private Vector3 _lastMousePosition;
        private Vector2 _rotationInput;


        private Vector3 _selectedObjectScale = new(0.24f, 0.3897f, 0.0032f);

        private void Update()
        {
            if (!Input.GetMouseButton(0)) return;
            if (!_isActivate) return;

            _rotationInput.x = Input.GetAxis(HorizontalAxis);
            _rotationInput.y = Input.GetAxis(VerticalAxis);
            RotateObject();
        }

        public void Activate(Transform selectedObject, bool isScaling, Action onMoved = null)
        {
            _isActivate = true;
            _selectedObject = selectedObject;
            if (isScaling)
            {
                _selectedObject.DOScale(_selectedObjectScale, _selectedObjectScaleDuration);
            }

            _selectedObject.SetParent(_pocketPoint);
            _selectedObject.DOLocalMove(Vector3.zero, 1f).OnComplete(() =>
            {
                _isActivate = true;
                onMoved?.Invoke();
            });

        }
        
        public void Deactivate()
        {
            _selectedObject = null;
            _isActivate = false;
        }
        
        private void RotateObject()
        {
            float rotationX = _rotationInput.y * _rotationSpeed * Time.deltaTime;
            float rotationY = -_rotationInput.x * _rotationSpeed * Time.deltaTime;
            _selectedObject.Rotate(Camera.main.transform.up, rotationY, Space.World);
            _selectedObject.Rotate(Camera.main.transform.right, rotationX, Space.World);
        }
    }
}
