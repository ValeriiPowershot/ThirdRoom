using CodeBase.Inventory;
using CodeBase.Inventory.Controller;
using CodeBase.Inventory.Model;
using CodeBase.Inventory.View;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private InventoryController _inventoryController;

        public override void InstallBindings()
        {
            BindInventoryModel();
            BindInventoryView();
            BindInventoryController();
        }

        private void BindInventoryModel()
        {
            Container.Bind<InventoryModel>().AsSingle().NonLazy();
        }

        private void BindInventoryView()
        {
            Container.Bind<InventoryView>().FromInstance(_inventoryView).AsSingle().NonLazy();
        }

        private void BindInventoryController()
        {
            Container.Bind<InventoryController>().FromInstance(_inventoryController).AsSingle().NonLazy();
        }
    }
}
