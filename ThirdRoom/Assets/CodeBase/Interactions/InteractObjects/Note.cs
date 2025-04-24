using ECM2.Examples.FirstPerson;
using UnityEngine;

namespace CodeBase.Interactions.InteractObjects
{
    public class Note : InteractObject
    {
        [SerializeField] private ObjectRotation _objectRotation;
        [SerializeField] private FirstPersonCharacterInput _firstPersonCharacterInput;
        [SerializeField] private FirstPersonCharacterLookInput _firstPersonCharacterLookInput;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _objectRotation.Deactivate();
                _objectRotation.transform.SetParent(null);
                _firstPersonCharacterLookInput.ToggleMouseInput(true);
                _firstPersonCharacterInput.ToggleInput(true);
                _objectRotation.Rigidbody.isKinematic = false;
            }
        }

        protected override void OnInteract()
        {
            _objectRotation.Rigidbody.isKinematic = true;
            _firstPersonCharacterLookInput.ToggleMouseInput(false);
            _firstPersonCharacterInput.ToggleInput(false);
            _objectRotation.Activate(gameObject.transform, false);
        }
    }
}
