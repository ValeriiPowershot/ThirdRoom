using UnityEngine;

namespace ECM2.Examples.FirstPerson
{
    /// <summary>
    /// First person mouse look input.
    /// </summary>
    
    public class FirstPersonCharacterLookInput : MonoBehaviour
    {
        [Space(15.0f)]
        public bool invertLook = true;
        [Tooltip("Mouse look sensitivity")]
        public Vector2 mouseSensitivity = new(1.0f, 1.0f);
        
        [Space(15.0f)]
        [Tooltip("How far in degrees can you move the camera down.")]
        public float minPitch = -80.0f;
        [Tooltip("How far in degrees can you move the camera up.")]
        public float maxPitch = 80.0f;

        private bool _enabled = true;
        private FirstPersonCharacter _character;

        private void Awake()
        {
            _character = GetComponent<FirstPersonCharacter>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if(!_enabled) return;
            
            Vector2 lookInput = new Vector2
            {
                x = Input.GetAxisRaw("Mouse X"),
                y = Input.GetAxisRaw("Mouse Y")
            };

            lookInput *= mouseSensitivity;

            _character.AddControlYawInput(lookInput.x);
            _character.AddControlPitchInput(invertLook ? -lookInput.y : lookInput.y);
        }
        
        public void ToggleMouseInput(bool active) => _enabled = active;
        public void ToggleMouseCursor(bool active) =>
            Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
