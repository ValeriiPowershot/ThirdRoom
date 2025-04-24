using System.Collections;
using CodeBase.Audio;
using UnityEngine;
using UnityEngine.Video;

namespace CodeBase.Environment
{
    [DisallowMultipleComponent]
    public class TV : MonoBehaviour
    {
        [SerializeField] private FMODSoundPlayer _soundPlayer;
        [SerializeField] private VideoPlayer _videoPlayer;

        [Space] 
        [SerializeField] private VideoClip _whiteNoiseClip;
        [SerializeField] private VideoClip _singleEyeClip;
        [SerializeField] private VideoClip _oceanClip;
        
        private void Start() 
            => StartCoroutine(StartTVAnimation());

        private IEnumerator StartTVAnimation()
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
        }
    }
}