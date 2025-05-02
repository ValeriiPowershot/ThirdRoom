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
		[SerializeField] private LightColorType _requiredColor;
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
			Vector3 targetPosition = Camera.main.transform.position;
			Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
			Quaternion offsetRotation = Quaternion.Euler(270, 0, 180); // Изменено смещение
			Quaternion finalRotation = targetRotation * offsetRotation;

			transform.DORotateQuaternion(finalRotation, _speed);
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

		private void OnLightColorChanged(LightColorType lightColorType)
		{
			if (_requiredColor == lightColorType)
			{
				print("Required color selected");
				OnRequiredColorSelected?.Invoke();
			}
			else
			{
				print("Non-required color selected");
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