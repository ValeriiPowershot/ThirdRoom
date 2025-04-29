using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace CodeBase.Audio
{
    [DisallowMultipleComponent]
    public class FMODAudioPlayer : MonoBehaviour
    {
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
        
        public EventInstance Play3DLoopingSound(EventReference eventReference, Transform followTarget)
        {
            if (eventReference.IsNull)
            {
                Debug.LogWarning("FMOD event path is empty.");
                return default;
            }

            if (followTarget == null)
            {
                Debug.LogWarning("Follow target Transform is null.");
                return default;
            }

            try
            {
                EventInstance instance = RuntimeManager.CreateInstance(eventReference);
                FMODPositionFollower follower = gameObject.AddComponent<FMODPositionFollower>();
                
                follower.Initialize(instance, followTarget);
                
                instance.start();
                return instance;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to play 3D looping FMOD event {eventReference}: {e.Message}");
                return default;
            }
        }
        
        [DisallowMultipleComponent]
        private class FMODPositionFollower : MonoBehaviour
        {
            private EventInstance _eventInstance;
            private Transform _target;

            public void Initialize(EventInstance eventInstance, Transform target)
            {
                _eventInstance = eventInstance;
                _target = target;
            }

            private void Update()
            {
                if (!_eventInstance.isValid() || _target == null)
                {
                    Destroy(this);
                    return;
                }

                _eventInstance.set3DAttributes(_target.position.To3DAttributes());
            }

            private void OnDestroy()
            {
                if (!_eventInstance.isValid()) return;
                _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                _eventInstance.release();
            }
        }
    }
}