using ECM2.Examples.FirstPerson;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase
{
    public class PlayerPrefab : MonoBehaviour
    {
        [SerializeField] private FirstPersonCharacterLookInput _firstPersonCharacterLookInput;
        [SerializeField] private FirstPersonCharacterInput _firstPersonCharacterInput;

        public void BlockInput()
        {
            _firstPersonCharacterInput.ToggleInput(false);
            _firstPersonCharacterLookInput.ToggleMouseInput(false);
        }

        public void UnblockInput()
        {
            _firstPersonCharacterInput.ToggleInput(true);
            _firstPersonCharacterLookInput.ToggleMouseInput(true);
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
