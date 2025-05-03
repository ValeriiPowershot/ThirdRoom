using CodeBase.Interactions;
using ECM2.Examples.FirstPerson;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase
{
    [DisallowMultipleComponent]
    public class PlayerPrefab : MonoBehaviour
    {
        [Header("Components")]
        [field: SerializeField] public Interactor Interactor { get; private set; }
        
        [SerializeField] private FirstPersonCharacterInput _firstPersonCharacterInput;
        [SerializeField] private FirstPersonCharacter _firstPersonCharacter;
        [SerializeField] private FirstPersonCharacterLookInput _firstPersonCharacterLookInput;
        
        [Header("Points")]
        [field: SerializeField] public Transform InventoryPoint { get; private set; }
        
        public void BlockInput()
        {
            _firstPersonCharacterInput.ToggleInput(false);
            _firstPersonCharacterLookInput.ToggleMouseInput(false);
            _firstPersonCharacterInput.Character.SetMovementDirection(Vector3.zero);
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
