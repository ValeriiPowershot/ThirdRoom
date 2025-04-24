using ECM2;
using UnityEngine;

namespace CodeBase.Audio
{
    [DisallowMultipleComponent]
    public class Footsteps : MonoBehaviour
    {
        [SerializeField] private FMODSoundPlayer _soundPlayer;
        [SerializeField] private HeadBobbing _headBobbing;
        [SerializeField] private Character _character;
        
        private void Start()
        {
            _headBobbing.OnStepDown += PlayFootstepSound;
        }

        private void OnDestroy()
        {
            _headBobbing.OnStepDown -= PlayFootstepSound;
        }

        private void PlayFootstepSound()
        {
            if (!_character.IsOnGround()) return;
            _soundPlayer.Play();
        }
    }
}
