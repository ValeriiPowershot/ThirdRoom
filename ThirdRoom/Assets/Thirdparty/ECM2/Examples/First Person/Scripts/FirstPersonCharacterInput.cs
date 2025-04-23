using UnityEngine;

namespace ECM2.Examples.FirstPerson
{
    /// <summary>
    /// First person character input.
    /// </summary>
    
    public class FirstPersonCharacterInput : MonoBehaviour
    {
        private Character _character;

        [SerializeField] private bool _enabled = true;
        
        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void Update()
        {
            if (!_enabled) return;
            
            // Movement input, relative to character's view direction
            
            Vector2 inputMove = new()
            {
                x = Input.GetAxisRaw("Horizontal"),
                y = Input.GetAxisRaw("Vertical")
            };
            
            Vector3 movementDirection =  Vector3.zero;
            
            movementDirection += _character.GetRightVector() * inputMove.x;
            movementDirection += _character.GetForwardVector() * inputMove.y;

            _character.SetMovementDirection(movementDirection);
            
            // Crouch input
            
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
                _character.Crouch();
            else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
                _character.UnCrouch();
            
            // Jump input
            
            if (Input.GetButtonDown("Jump"))
                _character.Jump();
            else if (Input.GetButtonUp("Jump"))
                _character.StopJumping();
        }

        public void ToggleInput(bool active) => _enabled = active;
    }
}
