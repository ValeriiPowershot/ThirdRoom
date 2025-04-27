using System;
using System.Collections;
using CodeBase;
using CodeBase.Infrastructure.Installers;
using CodeBase.Interactions;
using Codebase.Logic.Interactions;
using DG.Tweening;
using ECM2.Examples.FirstPerson;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Codebase.Logic
{
    [DisallowMultipleComponent]
    public class Obtainer : InteractObject
    {
        public static event Action OnDeductiveObjectClaimed;

        [SerializeField] private ObtainerUI _obtainerUI;
        [SerializeField] private ObjectRotation _objectRotation;
        [SerializeField] private bool _isScalingObject;
        [SerializeField] private bool _isDeductiveObject;
        [SerializeField] private bool _isRotatingObject;
        [SerializeField] private bool _isLetter;

        private bool _isCigaretteCase;

        private Vector3 _originalScale;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        //Delete after trailer and change to private Vector3 _originalPostion;
        private Transform _trailerDisposePosterPosition;

        private FirstPersonCharacterInput _firstPersonCharacterInput;
        private FirstPersonCharacter _firstPersonCharacter;
        
        [Inject]
        public void Construct(PlayerPrefab playerPrefab)
        {
            _firstPersonCharacterInput = playerPrefab.FirstPersonCharacterInput;
            _firstPersonCharacter = playerPrefab.FirstPersonCharacter;
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
            //yield return new WaitUntil(() => PlayerPrefab.Instance != null);
            //_trailerDisposePosterPosition = PlayerPrefab.Instance.TrailerDisposeTransform;
            _obtainerUI.OnDestroyRequested += OnDestroyRequested;
            _obtainerUI.OnMoveToStashRequested += OnMoveToStashRequested;
            //_isCigaretteCase = TryGetComponent(out CigaretteCase _);
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
            _originalPosition = transform.position;
            _originalScale = transform.localScale;
            _originalRotation = transform.rotation;
            if (_isCigaretteCase)
            {
                _objectRotation.Activate(transform, _isScalingObject, RotateToCamera);
            }
            else if (_isLetter)
            {
                _objectRotation.Activate(transform, _isScalingObject, MoveToCameraPlan);
            }
            else
            {
                _objectRotation.Activate(transform, _isScalingObject);
            }
            

            _obtainerUI.Display(transform);
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
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

            transform.DORotateQuaternion(targetRotation, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                /*if (TryGetComponent(out CigaretteCase cigaretteCase))
                {
                    cigaretteCase.Open(ZoomCamera);
                }*/
            });
        }

        private void ZoomCamera()
        {
            transform.DOScale(transform.localScale * 1.5f, 0.4f).SetEase(Ease.InOutSine);
        }

        private void OnMoveToStashRequested(Transform target)
        {
            print("MOVE TO SHTASH!!!");
            _objectRotation.Deactivate();
            target.SetParent(null);
            target.DORotate(_originalRotation.eulerAngles, 1f);
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