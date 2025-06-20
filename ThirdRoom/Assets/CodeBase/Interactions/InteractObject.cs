using System;
using Codebase.Logic.Interactions;
using UnityEngine;

namespace CodeBase.Interactions
{
    [DisallowMultipleComponent]
    public abstract class InteractObject : MonoBehaviour, IInteractable
    {
        public event Action OnInteracted;
        public event Action OnLostFocus;
        public event Action OnHoldInteractStart;
        public event Action OnHoldInteractEnd;
        
        [field: SerializeField] public bool IsInteractable { get; protected set; } = true;
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

        public void HoldInteract(bool isPressed)
        {
            if (isPressed)
            {
                OnHoldInteractStart?.Invoke();
                OnHoldInteract();
            }
            else
            {
                OnHoldInteractEnd?.Invoke();
                OnReleaseInteract();
            }
        }

        protected virtual void OnHoldInteract() { }

        protected virtual void OnReleaseInteract(Action callback = null) { }
    }
}
