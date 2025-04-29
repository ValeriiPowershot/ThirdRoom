using System.Collections;
using CodeBase.Interactions;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Environment
{
    public class DiskBox : InteractObject
    {
        [SerializeField] private Transform _upperSideTransform;
        [SerializeField] private Transform _diskTransform;
        
        protected override void OnInteract()
        {
            StartCoroutine(OpenAndShowcaseDiskRoutine());
        }

        private IEnumerator OpenAndShowcaseDiskRoutine()
        {
            float endYDiskPosition = _diskTransform.localPosition.y + 0.1f;
            
            yield return _upperSideTransform.DORotate(new Vector3(0f, 0f, 180f), 1f).WaitForCompletion();
            yield return _diskTransform.DOLocalMoveY(endYDiskPosition, 1f);
        }
    }
}