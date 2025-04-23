using System;
using UnityEngine;

namespace Codebase.Logic.Interactions
{
    [DisallowMultipleComponent]
    public abstract class InteractObject : MonoBehaviour, IInteractable
    {
        public event Action OnInteracted;
        public event Action OnLostFocus;
        public bool IsInteractable { get; protected set; } = true;
        public string Description => _description;
        
        [SerializeField] private string _description;
        [SerializeField] private bool _disableAfterInteraction;
        
        public void Interact()
        {
            if (!IsInteractable) return;
            
            if (_disableAfterInteraction)
                IsInteractable = false;
            
            OnInteracted?.Invoke();
            OnInteract();
        }

        public void LoseFocus()
        {
            OnLostFocus?.Invoke();
            OnLoseFocus();
        }

        protected abstract void OnInteract();
        protected virtual void OnLoseFocus() { }
    }
}