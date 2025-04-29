using CodeBase.Audio;
using CodeBase.Controls;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private FMODAudioPlayer _fmodAudioPlayer;
        
        public override void InstallBindings()
        {
            InputSystemInstall();
            AudioPlayerInstall();
        }

        private void InputSystemInstall()
            => Container.Bind<IInputService>().To<InputService>().AsSingle().NonLazy();
        
        private void AudioPlayerInstall()
            => Container.Bind<FMODAudioPlayer>().FromComponentInNewPrefab(_fmodAudioPlayer)
                .WithGameObjectName("Audio Player").AsSingle().NonLazy();
    }
}