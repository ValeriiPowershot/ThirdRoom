using System.Collections;
using CodeBase.Audio;
using CodeBase.Interactions;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Zenject;

namespace CodeBase.Environment
{
    [DisallowMultipleComponent]
    public class Phone : InteractObject
    {
        [Header("Sounds")] 
        [SerializeField] private EventReference _phoneRingingEvent;
        [SerializeField] private EventReference _phonePickupEvent;
        [SerializeField] private EventReference _phoneVoiceEvent;
        [SerializeField] private EventReference _phoneDialingEvent;
        [SerializeField] private EventReference _phoneHangupEvent;

        [Header("References")] 
        [SerializeField] private TV _tv;
        
        private FMODAudioPlayer _audioPlayer;
        private EventInstance _ringingSoundInstance;

        [Inject]
        public void Construct(FMODAudioPlayer audioPlayer)
            => _audioPlayer = audioPlayer;
        
        private void Start()
        {
            StartRinging();
        }

        protected override void OnInteract()
        {
            StartCoroutine(HandlePickupRoutine());
        }

        public void StartRinging()
        {
            _ringingSoundInstance = _audioPlayer.Play3DLoopingSound(_phoneRingingEvent, transform);
        }

        private IEnumerator HandlePickupRoutine()
        {
            _audioPlayer.StopEvent(_ringingSoundInstance);
            _audioPlayer.PlayOneShot(_phonePickupEvent);
            yield return new WaitForSeconds(1f);
            _audioPlayer.PlayOneShot(_phoneVoiceEvent);
            yield return new WaitForSeconds(8f);
            _audioPlayer.PlayOneShot(_phoneDialingEvent);
            yield return new WaitForSeconds(4f);
            _audioPlayer.PlayOneShot(_phoneHangupEvent);
            _tv.AllowInteract();
        }
    }
}
