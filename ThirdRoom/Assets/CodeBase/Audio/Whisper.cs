using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Audio
{
    [DisallowMultipleComponent]
    public class Whisper : MonoBehaviour
    {
        [SerializeField] private EventReference _whisperEvent;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                Play();
        }

        public void Play()
        {
            Vector3 randomPosition = Random.insideUnitSphere;
            FMODAudioManager.Instance.PlayOneShot(_whisperEvent, randomPosition);
        }
    }
}
                    