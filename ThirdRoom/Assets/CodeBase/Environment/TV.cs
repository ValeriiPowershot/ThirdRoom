using System.Collections;
using CodeBase.Audio;
using CodeBase.Interactions;
using UnityEngine;
using UnityEngine.Video;
using Zenject;

namespace CodeBase.Environment
{
    [DisallowMultipleComponent]
    public class TV : InteractObject
    {
        [SerializeField] private Console _console;
        
        [Space]
        [Header("TV Settings")]
        [SerializeField] private FMODSoundPlayer _soundPlayer;
        [SerializeField] private VideoPlayer _videoPlayer;

        [Header("Videos")]
        [Space] 
        [SerializeField] private VideoClip _whiteNoiseClip;
        [SerializeField] private VideoClip _singleEyeClip;
        [SerializeField] private VideoClip _oceanClip;
        
        [Space]
        [SerializeField] private VideoClip _hintVideoClip;
        
        private Coroutine _tvAnimationRoutine;
        private FMODAudioPlayer _audioPlayer;

        [Inject]
        public void Construct(FMODAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }
        
        protected override void OnInteract()
        {
            _tvAnimationRoutine = StartCoroutine(StartTVAnimationRoutine());
        }

        public void AllowInteract()
            => IsInteractable = true;

        public void DisplayDiskHint()
        {
            StopCoroutine(_tvAnimationRoutine);
            _soundPlayer.Stop();
            _videoPlayer.clip = _hintVideoClip;
            // TODO play hint sound
        }
        
        private IEnumerator StartTVAnimationRoutine()
        {
            _videoPlayer.clip = _whiteNoiseClip;
            _videoPlayer.Play();
            _soundPlayer.Play();

            yield return new WaitForSeconds(2f);
            
            _videoPlayer.clip = _singleEyeClip;
            _videoPlayer.Play();
            _soundPlayer.Stop();
            
            yield return new WaitForSeconds(2f);
            
            _videoPlayer.clip = _whiteNoiseClip;
            _videoPlayer.Play();
            _soundPlayer.Play();
            
            yield return new WaitForSeconds(3f);
            
            _soundPlayer.Stop();
            _videoPlayer.clip = _oceanClip;
            _videoPlayer.Play();
            
            yield return new WaitForSeconds(2f);
            
            _videoPlayer.clip = _whiteNoiseClip;
            _videoPlayer.Play();
            _soundPlayer.Play();
            _console.AllowInteract();
        }
    }
}