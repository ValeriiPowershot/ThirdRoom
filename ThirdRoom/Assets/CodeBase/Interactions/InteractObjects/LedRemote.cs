using System;
using System.Collections.Generic;
using DG.Tweening;
using ECM2.Examples.FirstPerson;
using UnityEngine;
using static UnityEngine.Screen;

namespace CodeBase.Interactions.InteractObjects
{
	public class LedRemote : InteractObject
	{
		[SerializeField] private List<LightButton> _buttons = new List<LightButton>();
		[SerializeField] private FirstPersonCharacterInput _firstPersonCharacterInput;
		[SerializeField] private FirstPersonCharacterLookInput _firstPersonCharacterLookInput;
		[SerializeField] private float _speed;
		[SerializeField] private Color _requiredColor;
		public Action OnRequiredColorSelected;
		public Action OnNonRequiredColorSelected;
		
		private void Start()
		{
			Deactivate();
		}

		protected override void OnInteract()
		{
			DragToCenter();
			_firstPersonCharacterInput.ToggleInput(false);
			_firstPersonCharacterLookInput.ToggleMouseInput(false);
		}

		private void DragToCenter()
		{
			Vector3 screenCenter = new Vector3(width / 2, height / 2, Camera.main.WorldToScreenPoint(transform.position).z);
			Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);

			transform.DOMove(worldCenter, _speed)
				.SetEase(Ease.InOutQuad)
				.OnComplete(() =>
				{
					RotateToCenter();
					Activate();
				});
		}

		[ContextMenu("Return")]
		private void RotateToCenter()
		{
			transform.DOLookAt(Camera.main.transform.position, _speed);
		}

		private void Activate()
		{
			foreach (LightButton lightButton in _buttons)
			{
				lightButton.Button.interactable = true;
				lightButton.OnLightColorChanged += OnLightColorChanged;
			}
			_firstPersonCharacterLookInput.ToggleMouseCursor(true);
		}

		private void OnLightColorChanged(Color color)
		{
			if (_requiredColor == color)
			{
				OnRequiredColorSelected?.Invoke();
			}
			else
			{
				OnNonRequiredColorSelected?.Invoke();
			}
		}

		private void Deactivate()
		{
			foreach (LightButton lightButton in _buttons)
			{
				lightButton.Button.interactable = false;
			}
		}
	}
}