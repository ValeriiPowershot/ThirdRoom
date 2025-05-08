using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerPrefab _playerPrefab;
        [SerializeField] private ObjectRotation _objectRotation;
        
        public override void InstallBindings()
        {
            PlayerPrefabInstall();
            ObjectRotationInstall();
        }

        private void PlayerPrefabInstall()
        {
            Container.Bind<PlayerPrefab>().FromInstance(_playerPrefab).AsSingle().NonLazy();
        }

        private void ObjectRotationInstall()
        {
            Container.Bind<ObjectRotation>().FromInstance(_objectRotation).AsSingle().NonLazy();
        }
    }
}
