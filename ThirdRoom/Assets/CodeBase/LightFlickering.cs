using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase
{
    [DisallowMultipleComponent]
    public class LightFlickering : MonoBehaviour
    {
        public bool FlickeringEnabled
        {
            get => _flickeringEnabled;
            set
            {
                if (_flickeringEnabled == value) return;
                _flickeringEnabled = value;

                if (_flickeringEnabled)
                {
                    StartFlickering();
                }
                else
                {
                    StopFlickering();
                }
            }
        }

        [Header("External references")]
        [SerializeField] private Light _lightSource;

        [Header("Settings")]
        [SerializeField] private float _minIntensity;
        [SerializeField] private float _maxIntensity = 1f;
        [SerializeField] private float _minFlickeringDurationInSeconds = 0.1f;
        [SerializeField] private float _maxFlickeringDurationInSeconds = 0.1f;
        [SerializeField] private bool _startFlickeringAtStart;
        
        private Coroutine _flickeringRoutine;

        private bool _flickeringEnabled;

        private void Start()
        {
            if (_startFlickeringAtStart)
                StartFlickering();
        }

        [ContextMenu("Start Flickering")]
        private void StartFlickering()
        {
            _flickeringRoutine ??= StartCoroutine(FlickeringRoutine());
        }

        private void StopFlickering()
        {
            if (_flickeringRoutine == null) return;
            StopCoroutine(_flickeringRoutine);
            _flickeringRoutine = null;
        }

        private IEnumerator FlickeringRoutine()
        {
            while (_flickeringEnabled)
            {
                yield return new WaitForSeconds(Random.Range(_minFlickeringDurationInSeconds,
                    _maxFlickeringDurationInSeconds));
                SetLightSourceIntensity();
            }
        }

        private void SetLightSourceIntensity()
            => _lightSource.intensity = Random.Range(_minIntensity, _maxIntensity);
    }
}
