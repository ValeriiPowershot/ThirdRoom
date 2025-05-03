using CodeBase.Audio;
using CodeBase.Controls;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private FMODAudioPlayer _fmodAudioPlayer;
        [SerializeField] private CentralUI _centralUI;
        
        public override void InstallBindings()
        {
            InputSystemInstall();
            AudioPlayerInstall();
            UIInstall();
        }

        private void InputSystemInstall()
            => Container.Bind<IInputService>().To<InputService>().AsSingle().NonLazy();
        
        private void AudioPlayerInstall()
            => Container.Bind<FMODAudioPlayer>().FromComponentInNewPrefab(_fmodAudioPlayer)
                .WithGameObjectName("Audio Player").AsSingle().NonLazy();

        private void UIInstall()
            => Container.Bind<CentralUI>().FromComponentInNewPrefab(_centralUI)
                .WithGameObjectName("CentralCanvas").AsSingle().NonLazy();
    }
}