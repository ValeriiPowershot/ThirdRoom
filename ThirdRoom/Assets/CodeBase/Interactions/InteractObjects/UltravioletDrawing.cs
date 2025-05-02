using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Interactions.InteractObjects
{
	public class UltravioletDrawing : MonoBehaviour
	{
		[SerializeField] private List<Renderer> _drawings;
		[SerializeField] private LedRemote _ledRemote;

		private void Start()
		{
			_ledRemote.OnRequiredColorSelected += OnRequiredColorSelected;
			_ledRemote.OnNonRequiredColorSelected += OnNonRequiredColorSelected;
			OnNonRequiredColorSelected();
		}

		private void OnNonRequiredColorSelected() =>
			_drawings.ForEach(sprite => sprite.gameObject.SetActive(false));

		private void OnRequiredColorSelected() =>
			_drawings.ForEach(sprite => sprite.gameObject.SetActive(true));
	}
}