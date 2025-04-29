using CodeBase.Interactions;
using ECM2.Examples.FirstPerson;
using UnityEngine;

namespace CodeBase
{
    public class PlayerPrefab : MonoBehaviour
    {
        [field: SerializeField] public Interactor Interactor { get; private set; }
        [field: SerializeField] public FirstPersonCharacter FirstPersonCharacter { get; private set; }
        [field: SerializeField] public FirstPersonCharacterLookInput _firstPersonCharacterLookInput;
        [field: SerializeField] public FirstPersonCharacterInput _firstPersonCharacterInput;

        public void BlockInput()
        {
            _firstPersonCharacterInput.ToggleInput(false);
            _firstPersonCharacterLookInput.ToggleMouseInput(false);
            Debug.Log("BlockInput");
        }

        public void UnblockInput()
        {
            _firstPersonCharacterInput.ToggleInput(true);
            _firstPersonCharacterLookInput.ToggleMouseInput(true);
            Debug.Log("UnblockInput");
        }

        public void LockCursor()
        {
            _firstPersonCharacterLookInput.ToggleMouseCursor(true);
        }

        public void UnlockCursor()
        {
            _firstPersonCharacterLookInput.ToggleMouseCursor(false);
        }
    }
}
