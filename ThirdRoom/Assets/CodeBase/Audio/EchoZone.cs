using ECM2;
using UnityEngine;

namespace CodeBase.Audio
{
    [DisallowMultipleComponent]
    public class EchoZone : MonoBehaviour
    {
        [SerializeField] private FMODSoundPlayer _soundPlayer;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Character _)) return;
            _soundPlayer.Play();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Character _)) return;
            _soundPlayer.Stop();
        }
    }
}