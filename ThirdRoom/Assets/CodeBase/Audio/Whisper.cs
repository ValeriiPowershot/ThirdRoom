using FMODUnity;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Audio
{
    [DisallowMultipleComponent]
    public class Whisper : MonoBehaviour
    {
        [SerializeField] private EventReference _whisperEvent;
        
        private FMODAudioPlayer _audioPlayer;

        [Inject]
        public void Construct(FMODAudioPlayer audioPlayer)
            => _audioPlayer = audioPlayer;    
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                Play();
        }

        public void Play()
        {
            Vector3 randomPosition = Random.insideUnitSphere;
            _audioPlayer.PlayOneShot(_whisperEvent, randomPosition);
        }
    }
}
                    