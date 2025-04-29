using CodeBase.Audio;
using CodeBase.Interactions;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private FMODAudioPlayer _fmodAudioPlayer;
        
        public override void InstallBindings()
        {
            AudioPlayerInstall();
        }

        private void AudioPlayerInstall()
            => Container.Bind<FMODAudioPlayer>().FromComponentInNewPrefab(_fmodAudioPlayer)
                .WithGameObjectName("Audio Player").AsSingle().NonLazy();
    }
}