using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerPrefab _playerPrefab;
        
        public override void InstallBindings()
        {
            PlayerInputInstall();
        }

        private void PlayerInputInstall()
        {
            Container.Bind<PlayerPrefab>().FromInstance(_playerPrefab).AsSingle().NonLazy();
        }
    }

}
