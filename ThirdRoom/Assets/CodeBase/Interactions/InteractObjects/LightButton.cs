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
		[SerializeField] private LightColorType _lightColorType;
		public Action<LightColorType> OnLightColorChanged;

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
			
			_light.color = _lightColor;
			_light.enabled = true;
			OnLightColorChanged?.Invoke(_lightColorType);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			print(gameObject.name);
		}
	}
}