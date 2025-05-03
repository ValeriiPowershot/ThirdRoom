using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Environment
{
    public class DiskBox : Obtainer
    {
        [SerializeField] private Transform _upperSideTransform;
        [SerializeField] private Transform _diskTransform;
        
        protected override void OnInteract()
        {
            StartCoroutine(OpenAndShowcaseDiskRoutine(() => base.OnInteract()));
        }

        private IEnumerator OpenAndShowcaseDiskRoutine(Action onComplete)
        {
            float endYDiskPosition = _diskTransform.localPosition.y + 0.1f;
            
            yield return _upperSideTransform.DORotate(new Vector3(0f, 0f, 180f), 1f).WaitForCompletion();
            yield return _diskTransform.DOLocalMoveY(endYDiskPosition, 1f);
            
            onComplete?.Invoke();
        }
    }
}