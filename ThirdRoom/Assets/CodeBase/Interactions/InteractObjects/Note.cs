using System;
using DG.Tweening;
using ECM2.Examples.FirstPerson;
using UnityEngine;

namespace CodeBase.Interactions.InteractObjects
{
    public class Note : InteractObject
    {
        [SerializeField] private ObjectRotation _objectRotation;
        [SerializeField] private FirstPersonCharacterInput _firstPersonCharacterInput;
        [SerializeField] private FirstPersonCharacterLookInput _firstPersonCharacterLookInput;
        [SerializeField] private float _returnSpeed;
        
        private Vector3 _startPosition;
        private Vector3 _startRotation;

        
        private void Start()
        {
            _startPosition = gameObject.transform.position;
            _startRotation = gameObject.transform.rotation.eulerAngles;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _objectRotation.Deactivate();
                _objectRotation.transform.SetParent(null);
                _firstPersonCharacterLookInput.ToggleMouseInput(true);
                _firstPersonCharacterInput.ToggleInput(true);
                
                transform.DOMove(_startPosition, _returnSpeed);
                transform.DORotate(_startRotation, _returnSpeed);
            }
        }

        protected override void OnInteract()
        {
            _firstPersonCharacterLookInput.ToggleMouseInput(false);
            _firstPersonCharacterInput.ToggleInput(false);
            _objectRotation.Activate(gameObject.transform, false);
        }
    }
}
