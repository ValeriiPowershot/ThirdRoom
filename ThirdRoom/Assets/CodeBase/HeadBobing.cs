using System;
using System.Collections;
using DG.Tweening;
using ECM2.Examples.FirstPerson;
using UnityEngine;

namespace CodeBase
{
    [DisallowMultipleComponent]
    public class HeadBobbing : MonoBehaviour
    {
        public event Action OnStepDown;
        
        [Header("Walk Settings")] 
        [SerializeField] private float _walkBobVerticalFrequency = 14f;
        [SerializeField] private float _walkBobVerticalAmplitude = 0.05f;
        [SerializeField] private float _walkBobHorizontalFrequency = 10f;
        [SerializeField] private float _walkBobHorizontalAmplitude = 0.05f;

        [Header("Sprint Settings")] 
        [SerializeField] private float _sprintBobVerticalFrequency = 18f;
        [SerializeField] private float _sprintBobVerticalAmplitude = 0.1f;
        [SerializeField] private float _sprintBobHorizontalFrequency = 14f;
        [SerializeField] private float _sprintBobHorizontalAmplitude = 0.1f;

        [Header("Landing Settings")]
        [SerializeField] private float _landingShakeIntensity = 0.1f;
        [SerializeField] private float _landingShakeDuration = 0.3f;
        [SerializeField] private int _landingShakeVibrato = 10;
        [SerializeField] private float _landingShakeRandomness = 90f;
        
        [Header("General Settings")] 
        [SerializeField] private float _stopBobbingTimeInSeconds = .25f;
        [SerializeField] private float _bobbingSmoothTime = 12.5f;
        [SerializeField] private float _bobStartLerpSpeed = 5f;
        [SerializeField] private float _bobTransitionSpeed = 5f;
        
        [Space(10)]
        [SerializeField] private bool _headBobbingEnabled = true;

        [Header("References")]
        [SerializeField] private FirstPersonCharacter _character;
        [SerializeField] private Transform _cameraTransform;
        
        private float _defaultYPosition;
        private float _defaultXPosition;
        private float _timerX;
        private float _timerY;
        private bool _isMoving;
        private bool _isSprinting;
        private float _bobLerpFactor;

        private float _targetFrequencyY;
        private float _targetFrequencyX;
        private float _targetAmplitudeY;
        private float _targetAmplitudeX;

        private float _lastSinY;

        private float BobbingFrequencyY => Mathf.Lerp(_targetFrequencyY,
            _isSprinting ? _sprintBobVerticalFrequency : _walkBobVerticalFrequency,
            Time.deltaTime * _bobTransitionSpeed);

        private float BobbingFrequencyX => Mathf.Lerp(_targetFrequencyX,
            _isSprinting ? _sprintBobHorizontalFrequency : _walkBobHorizontalFrequency,
            Time.deltaTime * _bobTransitionSpeed);

        private float BobbingAmplitudeY => Mathf.Lerp(_targetAmplitudeY,
            _isSprinting ? _sprintBobVerticalAmplitude : _walkBobVerticalAmplitude,
            Time.deltaTime * _bobTransitionSpeed);

        private float BobbingAmplitudeX => Mathf.Lerp(_targetAmplitudeX,
            _isSprinting ? _sprintBobHorizontalAmplitude : _walkBobHorizontalAmplitude,
            Time.deltaTime * _bobTransitionSpeed);


        private Coroutine _bobbingRoutine;
        private Tween _resetCameraPositionTween;
        private Tween _landingShakeTween;
        
        private void Awake()
        {
            _defaultYPosition = _cameraTransform.localPosition.y;
            _defaultXPosition = _cameraTransform.localPosition.x;
        }

        private void Start()
        {
            _character.Landed += PerformLandBobbing;
            _character.CharacterMovementUpdated += OnMovementModeChanged;
        }

        private void OnDestroy()
        {
            _character.Landed -= PerformLandBobbing;
            _character.CharacterMovementUpdated -= OnMovementModeChanged;
        }

        #region Reset

        private void Reset()
        {
            // Walk Settings
            _walkBobVerticalFrequency = 14f;
            _walkBobVerticalAmplitude = 0.05f;
            _walkBobHorizontalFrequency = 10f;
            _walkBobHorizontalAmplitude = 0.05f;

            // Sprint Settings
            _sprintBobVerticalFrequency = 18f;
            _sprintBobVerticalAmplitude = 0.1f;
            _sprintBobHorizontalFrequency = 14f;
            _sprintBobHorizontalAmplitude = 0.1f;

            // Landing Settings
            _landingShakeIntensity = 0.1f;
            _landingShakeDuration = 0.3f;
            _landingShakeVibrato = 10;
            _landingShakeRandomness = 90f;

            // General Settings
            _stopBobbingTimeInSeconds = 0.25f;
            _bobbingSmoothTime = 12.5f;
            _bobStartLerpSpeed = 5f;
            _bobTransitionSpeed = 5f;
            _headBobbingEnabled = true;

            // References (try to auto-assign if possible)
            _character = GetComponent<FirstPersonCharacter>();
            _cameraTransform = GetComponentInChildren<Camera>()?.transform;
        }

        #endregion
        
        public void ToggleHeadBob(bool bobbingEnabled)
        {
            _headBobbingEnabled = bobbingEnabled;

            if (bobbingEnabled) return;
            
            StopBobbing();
            ResetCameraPosition();
        }

        private void SetMoving(bool isMoving, bool isSprinting = false)
        {
            if (_isMoving == isMoving && _isSprinting == isSprinting)
                return;

            _isMoving = isMoving;
            _isSprinting = isSprinting;

            _targetFrequencyY = _isSprinting ? _sprintBobVerticalFrequency : _walkBobVerticalFrequency;
            _targetFrequencyX = _isSprinting ? _sprintBobHorizontalFrequency : _walkBobHorizontalFrequency;
            _targetAmplitudeY = _isSprinting ? _sprintBobVerticalAmplitude : _walkBobVerticalAmplitude;
            _targetAmplitudeX = _isSprinting ? _sprintBobHorizontalAmplitude : _walkBobHorizontalAmplitude;

            if (!isMoving)
            {
                ResetCameraPosition();
                StopBobbing();
            }
            else
            {
                TryBreakCameraPositionTween();
                StopBobbing();
                _bobLerpFactor = 0f;
                _bobbingRoutine = StartCoroutine(HeadBobRoutine());
            }
        }

        private void ResetCameraPosition()
        {
            _resetCameraPositionTween ??= _cameraTransform.DOLocalMoveY(_defaultYPosition, _stopBobbingTimeInSeconds)
                .SetEase(Ease.InOutSine).OnComplete(() => _resetCameraPositionTween = null);
        }

        private void TryBreakCameraPositionTween()
        {
            if (_resetCameraPositionTween == null) return;
            _resetCameraPositionTween?.Kill();
            _resetCameraPositionTween = null;
        }

        private void StopBobbing()
        {
            if (_bobbingRoutine == null) return;
            StopCoroutine(_bobbingRoutine);
            _bobbingRoutine = null;
        }

        private void HandleHeadBob()
        {
            if (!_headBobbingEnabled || !_isMoving)
                return;

            _bobLerpFactor = Mathf.MoveTowards(_bobLerpFactor, 1f, Time.deltaTime * _bobStartLerpSpeed);

            _timerX += Time.deltaTime * BobbingFrequencyX;
            _timerY += Time.deltaTime * BobbingFrequencyY;

            Vector3 originalPosition = _cameraTransform.localPosition;
            Vector3 targetPosition = originalPosition;
            float sinY = Mathf.Sin(_timerY);
            targetPosition.y = _defaultYPosition + sinY * BobbingAmplitudeY;
            targetPosition.x = _defaultXPosition + Mathf.Cos(_timerX / 2) * BobbingAmplitudeX;

            if (_lastSinY > -0.99f && sinY <= -0.99f) 
                OnStepDown?.Invoke();
            
            _lastSinY = sinY;

            _cameraTransform.localPosition = Vector3.MoveTowards(
                _cameraTransform.localPosition, targetPosition, Time.deltaTime * _bobbingSmoothTime);
        }

        private IEnumerator HeadBobRoutine()
        {
            while (_isMoving)
            {
                HandleHeadBob();
                yield return null;
            }
        }

        private void PerformLandBobbing(Vector3 landingVelocity)
        {
            if (!_headBobbingEnabled) return;

            if (_landingShakeTween != null)
            {
                _landingShakeTween.Kill();
                _landingShakeTween = null;
            }

            float velocityMagnitude = landingVelocity.magnitude;
            float normalizedIntensity = Mathf.Clamp01(velocityMagnitude / 10f);
            float adjustedIntensity = _landingShakeIntensity * normalizedIntensity;

            _landingShakeTween = _cameraTransform.DOShakePosition(
                duration: _landingShakeDuration,
                strength: new Vector3(0f, adjustedIntensity, 0f), 
                vibrato: _landingShakeVibrato, 
                randomness: _landingShakeRandomness, 
                snapping: false, 
                fadeOut: true
            ).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                _landingShakeTween = null;
                ResetCameraPosition();
            });
        }

        private void OnMovementModeChanged(float magnitude) 
            => SetMoving(_character.GetMovementDirection() != Vector3.zero);
    }
}
