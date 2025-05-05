using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase
{
    [DisallowMultipleComponent]
    public class SubtitlesUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private Coroutine _displayMessageRoutine;

        public void DisplayMessage(string message, float duration)
        {
            _message.text = message;
            _displayMessageRoutine ??= StartCoroutine(DisplayMessageRoutine(message, duration));
        }

        private IEnumerator DisplayMessageRoutine(string message, float duration)
        {
            _message.text = message;
            
            yield return _canvasGroup.DOFade(1f, 0.3f).WaitForCompletion();
            yield return new WaitForSeconds(duration);
            yield return _canvasGroup.DOFade(0f, 0.25f).WaitForCompletion();
            
            _message.text = "";
            _displayMessageRoutine = null;
        }
    }
}
