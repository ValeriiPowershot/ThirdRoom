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
        [SerializeField] private TVVideoClip _whiteNoiseClip;
        [SerializeField] private TVVideoClip _singleEyeClip;
        [SerializeField] private TVVideoClip _oceanClip;
        
        [Space]
        [SerializeField] private VideoClip _hintVideoClip;
        
        private Coroutine _tvAnimationRoutine;
        private SubtitlesUI _subtitlesUI;

        [Inject]
        public void Construct(CentralUI centralUI)
            => _subtitlesUI = centralUI.SubtitlesUI;
        
        protected override void OnInteract() 
            => _tvAnimationRoutine = StartCoroutine(StartTVAnimationRoutine());

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
            while (true)
            {
                _videoPlayer.clip = _whiteNoiseClip.Clip;
                _videoPlayer.Play();
                _soundPlayer.Play();

                yield return new WaitForSeconds(_whiteNoiseClip.Duration);
            
                _videoPlayer.clip = _singleEyeClip.Clip;
                _videoPlayer.Play();
                _soundPlayer.Stop();
                _subtitlesUI.DisplayMessage(_singleEyeClip.Description, _whiteNoiseClip.Duration - 0.1f);
            
                yield return new WaitForSeconds(_singleEyeClip.Duration);
            
                _videoPlayer.clip = _whiteNoiseClip.Clip;
                _videoPlayer.Play();
            
                yield return new WaitForSeconds(_whiteNoiseClip.Duration);
            
                _soundPlayer.Stop();
                _videoPlayer.clip = _oceanClip.Clip;
                _videoPlayer.Play();
                _subtitlesUI.DisplayMessage(_oceanClip.Description, _oceanClip.Duration - 0.1f);
                
                yield return new WaitForSeconds(_oceanClip.Duration);
            }
        }
    }
}