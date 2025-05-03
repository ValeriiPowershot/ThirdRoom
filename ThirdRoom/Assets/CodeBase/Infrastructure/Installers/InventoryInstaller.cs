using CodeBase.Inventory.Controller;
using CodeBase.Inventory.Model;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private InventoryController _inventoryController;

        public override void InstallBindings()
        {
            BindInventoryModel();
            BindInventoryController();
        }

        private void BindInventoryModel()
        {
            Container.Bind<InventoryModel>().AsSingle().NonLazy();
        }

        private void BindInventoryController()
        {
            Container.Bind<InventoryController>().FromInstance(_inventoryController).AsSingle().NonLazy();
        }
    }
}
