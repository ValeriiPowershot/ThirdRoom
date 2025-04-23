using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace CodeBase.Audio
{
    [DisallowMultipleComponent]
    public class FMODSoundPlayer : MonoBehaviour
    {
        [Header("FMOD Event Settings")]
        [SerializeField] private EventReference _soundEvent;
        [SerializeField] private bool _playOnStart = true;
        [SerializeField] private bool _is3DEvent;

        [Header("Parameter Settings")]
        [SerializeField] private FMODParameter[] _parameters;

        private EventInstance _eventInstance;
        private bool _isInitialized;

        private void Awake()
        {
            InitializeEvent();
        }

        private void Start()
        {
            if (_playOnStart && _isInitialized) 
                Play();
        }

        private void OnDestroy()
        {
            Stop();
            ReleaseEvent();
        }

        private void InitializeEvent()
        {
            if (_soundEvent.IsNull)
            {
                Debug.LogWarning($"FMOD event path is not set on {gameObject.name}");
                return;
            }

            try
            {
                _eventInstance = RuntimeManager.CreateInstance(_soundEvent);

                if (_is3DEvent)
                {
                    RuntimeManager.AttachInstanceToGameObject(_eventInstance, transform, GetComponent<Rigidbody>());
                }

                // Initialize parameters
                foreach (FMODParameter param in _parameters)
                {
                    _eventInstance.setParameterByName(param.Name, param.DefaultValue);
                }

                _isInitialized = true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to initialize FMOD event {_soundEvent} on {gameObject.name}: {e.Message}");
            }
        }

        public void Play()
        {
            if (!_isInitialized) return;

            _eventInstance.start();
        }

        public void Stop()
        {
            if (!_isInitialized) return;

            _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public void SetParameter(string parameterName, float value)
        {
            if (!_isInitialized) return;

            try
            {
                _eventInstance.setParameterByName(parameterName, value);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to set parameter {parameterName} on {gameObject.name}: {e.Message}");
            }
        }

        public void PlayOneShot()
        {
            if (_soundEvent.IsNull) return;

            if (_is3DEvent)
            {
                RuntimeManager.PlayOneShot(_soundEvent, transform.position);
            }
            else
            {
                RuntimeManager.PlayOneShot(_soundEvent);
            }
        }

        private void ReleaseEvent()
        {
            if (!_isInitialized) return;
            _eventInstance.release();
            _isInitialized = false;
        }

        // Optional: Update 3D attributes every frame for moving objects
        private void Update()
        {
            if (_is3DEvent && _isInitialized) 
                _eventInstance.set3DAttributes(gameObject.To3DAttributes());
        }
    }
}