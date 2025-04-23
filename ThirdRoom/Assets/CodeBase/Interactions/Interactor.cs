using System;
using Codebase.Logic.Interactions;
using UnityEngine;

namespace CodeBase.Interactions
{
    [DisallowMultipleComponent]
    public class Interactor : MonoBehaviour
    {
        public event Action<bool, string> OnInteractObjectStateChanged;
        
        [SerializeField] private float _interactionDistance = 3f;
        [SerializeField] private KeyCode _interactionKey = KeyCode.E;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private bool _interactionEnabled = true;
        
        private RaycastHit _raycastHit;
        private InteractObject _interactObject;
        private Camera _camera;
        
        private void Start() => _camera = Camera.main;

        private void Update()
        {
            if (!_interactionEnabled) return;
            
            if (Input.GetKeyDown(KeyCode.E))
                TryInteract();
                
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out _raycastHit, _interactionDistance, _interactionLayer))
            {
                if (_interactObject != null)
                {
                    _interactObject.LoseFocus();
                    _interactObject = null;
                }
                
                OnInteractObjectStateChanged?.Invoke(false, null);
                return;
            }

            if (_raycastHit.collider.TryGetComponent(out InteractObject interactObject))
            {
                _interactObject = interactObject;
                
                if (interactObject.IsInteractable)
                {
                    OnInteractObjectStateChanged?.Invoke(true, interactObject.Description);
                    return;
                }
            }
            
            OnInteractObjectStateChanged?.Invoke(true, null);
        }

        public void ToggleAllowInteract(bool allowed) 
            => _interactionEnabled = allowed;

        private void TryInteract()
        {
            if (_raycastHit.collider != null && _raycastHit.collider.TryGetComponent(out IInteractable interactable))
                interactable.Interact();
        }
    }
}