using CodeBase.Controls;
using ECM2;
using UnityEngine;
using Zenject;

namespace Thirdparty.ECM2.Examples.First_Person.Scripts
{
    /// <summary>
    /// First person character input using the new Input System.
    /// </summary>
    public class FirstPersonCharacterInput : MonoBehaviour
    {
        public Character Character => _character;

        [SerializeField] private bool _enabled = true;

        private Character _character;
        private IInputService _inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void Update()
        {
            if (!_enabled) return;
            
            Vector2 inputMove = _inputService.CurrentMoveDirection;
            
            Vector3 movementDirection =  Vector3.zero;
            
            movementDirection += _character.GetRightVector() * inputMove.x;
            movementDirection += _character.GetForwardVector() * inputMove.y;

            _character.SetMovementDirection(movementDirection);
            
            
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
                _character.Crouch();
            else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
                _character.UnCrouch();
            
            
            if (Input.GetButtonDown("Jump"))
                _character.Jump();
            else if (Input.GetButtonUp("Jump"))
                _character.StopJumping();
        }

        public void ToggleInput(bool active) => _enabled = active;
    }
}
