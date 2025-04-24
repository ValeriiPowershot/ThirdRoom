using DG.Tweening;
using UnityEngine;

namespace CodeBase.Interactions.InteractObjects
{
    public class Cover : InteractObject
    {
        [SerializeField] private Transform _coverPivot;
        [SerializeField] private float _speed;
    
        protected override void OnInteract()
        {
            _coverPivot.transform.DORotate(new Vector3(0f, 0f, 300f), _speed);
        }
    }
}
