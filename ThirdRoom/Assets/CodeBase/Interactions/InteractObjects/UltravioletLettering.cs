using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Interactions.InteractObjects
{
	public class UltravioletLettering : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _lettering;
		[SerializeField] private LedRemote _ledRemote;

		private void Start()
		{
			_ledRemote.OnRequiredColorSelected += OnRequiredColorSelected;
			_ledRemote.OnNonRequiredColorSelected += OnNonRequiredColorSelected;
		}

		private void OnNonRequiredColorSelected() =>
			_lettering.DOFade(0, 0.1f);

		private void OnRequiredColorSelected() =>
			_lettering.DOFade(1, 0.1f);
	}
}