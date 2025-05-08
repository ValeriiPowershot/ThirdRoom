using System;
using CodeBase.Controls;
using CodeBase.Data;
using CodeBase.Inventory.Model;
using CodeBase.Inventory.View;
using UnityEngine;
using Zenject;

namespace CodeBase.Inventory.Controller
{
	[DisallowMultipleComponent]
	public class InventoryController : MonoBehaviour
	{
		private InventoryView _view;
		private InventoryModel _model;
		private PlayerPrefab _playerPrefab;
		private IInputService _inputService;
		public Action<Item> OnItemSelected;

		[Inject]
		public void Construct(PlayerPrefab playerPrefab, IInputService inputService, CentralUI centralUI)
		{
			_playerPrefab = playerPrefab;
			_model = new InventoryModel();
			_inputService = inputService;
			_view = centralUI.InventoryView;
		}

		private void Start()
		{
			_view.NextButton.onClick.AddListener(OnNextItem);
			_view.PrevButton.onClick.AddListener(OnPrevItem);
			_view.HideInventory();
		}

		private void OnDestroy()
		{
			_view.NextButton.onClick.RemoveListener(OnNextItem);
			_view.PrevButton.onClick.RemoveListener(OnPrevItem);
		}

		private void Update()
		{
			if (_inputService.IsOpenInventoryPressed) OpenInventory();
			if (_inputService.IsCloseInventoryPressed) CloseInventory();
			if (_inputService.IsNextButtonPressed) OnNextItem();
			if (_inputService.IsPreviousButtonPressed) OnPrevItem();
			if (_inputService.IsSelectInventoryPressed) OnSelectItem();
		}

		public void AddItem(Item item)
		{
			Debug.Log(_model.AddItem(item) ? "Item added to inventory." : "Item already exists in inventory.");
		}

		public void OpenInventory()
		{
			Item currentItem = _model.GetCurrentItem();
			_inputService.EnableActionMap(ActionMapType.Inventory);
			_view.ShowInventory();
			_playerPrefab.LockCursor();
			_playerPrefab.BlockInput();
			SetLayerRecursively(currentItem.gameObject, LayerMask.NameToLayer(TagsAndLayers.InventoryObjectLayer));
			_view.DisplayItem(currentItem);
		}

		public void CloseInventory()
		{
			_view.HideInventory();
			_playerPrefab.UnlockCursor();
			_playerPrefab.UnblockInput();
			_inputService.DisableActionMap(ActionMapType.Inventory);
		}

		private void SetLayerRecursively(GameObject obj, int newLayer)
		{
			if (obj == null) return;

			obj.layer = newLayer;

			foreach (Transform child in obj.transform)
			{
				SetLayerRecursively(child.gameObject, newLayer);
			}
		}

		private void OnNextItem()
		{
			_model.NextItem();
			_view.DisplayItem(_model.GetCurrentItem());
		}

		private void OnPrevItem()
		{
			_model.PreviousItem();
			_view.DisplayItem(_model.GetCurrentItem());
		}

		private void OnSelectItem()
		{
			Item currentItem = _model.GetCurrentItem();
			OnItemSelected?.Invoke(currentItem);
		}
	}
}