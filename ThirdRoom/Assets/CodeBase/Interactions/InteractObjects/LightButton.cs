using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.Interactions.InteractObjects
{
	public class LightButton : MonoBehaviour, IPointerEnterHandler
	{
		[field: SerializeField] public Button Button { get; private set; }
		[SerializeField] private Light _light;
		[SerializeField] private Color _lightColor;
		public Action<Color> OnLightColorChanged;

		private void OnValidate()
		{
			Button ??= GetComponent<Button>();
		}

		private void Start()
		{
			Button.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			print(gameObject.name);

			if (_light.color == _lightColor)
				return;

			_light.color = _lightColor;
			_light.enabled = true;
			OnLightColorChanged?.Invoke(_light.color);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			print(gameObject.name);
		}
	}
}