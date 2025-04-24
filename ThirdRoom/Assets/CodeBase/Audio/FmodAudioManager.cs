using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace CodeBase.Audio
{
    [DisallowMultipleComponent]
    public class FMODAudioManager : MonoBehaviour
    {
        public static FMODAudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            RuntimeManager.LoadBank("Master");
            RuntimeManager.LoadBank("Master.strings");
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void PlayOneShot(EventReference eventReference, Vector3 position)
        {
            if (eventReference.IsNull)
            {
                Debug.LogWarning("FMOD event path is empty.");
                return;
            }

            try
            {
                EventInstance instance = RuntimeManager.CreateInstance(eventReference);
                instance.set3DAttributes(position.To3DAttributes());
                instance.start();
                instance.release();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to play FMOD event {eventReference}: {e.Message}");
            }
        }

        public void PlayOneShot(EventReference eventReference)
        {
            if (eventReference.IsNull)
            {
                Debug.LogWarning("FMOD event path is empty.");
                return;
            }

            try
            {
                RuntimeManager.PlayOneShot(eventReference);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to play FMOD event {eventReference}: {e.Message}");
            }
        }

        public EventInstance CreateLoopingEvent(EventReference eventReference)
        {
            if (eventReference.IsNull)
            {
                Debug.LogWarning("FMOD event path is empty.");
                return default;
            }

            try
            {
                EventInstance instance = RuntimeManager.CreateInstance(eventReference);
                return instance;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to create looping FMOD event {eventReference}: {e.Message}");
                return default;
            }
        }

        public void SetParameter(EventInstance eventInstance, string parameterName, float value)
        {
            if (!eventInstance.isValid())
            {
                Debug.LogWarning("Invalid FMOD event instance.");
                return;
            }

            try
            {
                eventInstance.setParameterByName(parameterName, value);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to set parameter {parameterName}: {e.Message}");
            }
        }

        public void StopEvent(EventInstance eventInstance, bool immediate = false)
        {
            if (!eventInstance.isValid())
            {
                return;
            }

            try
            {
                eventInstance.stop(immediate ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                eventInstance.release();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to stop FMOD event: {e.Message}");
            }
        }
    }
}