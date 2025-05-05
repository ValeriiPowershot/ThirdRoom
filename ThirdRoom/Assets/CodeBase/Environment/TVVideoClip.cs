using UnityEngine;
using UnityEngine.Video;

namespace CodeBase.Environment
{
    [System.Serializable]
    public class TVVideoClip
    {
        [field: SerializeField] public VideoClip Clip { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }
}