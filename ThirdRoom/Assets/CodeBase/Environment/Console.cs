using System.Collections;
using CodeBase.Audio;
using CodeBase.Interactions;
using DG.Tweening;
using FMODUnity;
using UnityEngine;
using Zenject;

namespace CodeBase.Environment
{
    public class Console : InteractObject
    {
        [SerializeField] private TV _tv;

        [Header("Insert settings")] 
        [SerializeField] private Transform _initialPlacePoint;
        [SerializeField] private Transform _targetPlacePoint;
        [SerializeField] private float _insertSpeed = 1f;
        
        [Space]
        [SerializeField] private Disk _insertedDisk;

        [Header("Sounds")] 
        [SerializeField] private EventReference _putDiskSound;
        [SerializeField] private EventReference _driveRunSound;
        
        private FMODAudioPlayer _audioPlayer;

        [Inject]
        private void Construct(FMODAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }
        
        protected override void OnInteract()
        {
            StartCoroutine(TryInsertAndValidateDiskRoutine(_insertedDisk));
        }
        
        public void AllowInteract()
            => IsInteractable = true;
        
        private IEnumerator TryInsertAndValidateDiskRoutine(Disk disk)
        {
            _insertedDisk = disk;
            _insertedDisk.transform.position = _initialPlacePoint.position;
            _insertedDisk.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

            yield return new WaitForSeconds(1f);
            
            // TODO player insert disk sound
            
            _insertedDisk.transform.DORotate(new Vector3(45f, 0f, 90f), _insertSpeed).SetEase(Ease.InOutQuad);
            _insertedDisk.transform.DOMove(_targetPlacePoint.position, _insertSpeed).SetEase(Ease.InOutQuad)
                .WaitForCompletion();
            
            // TODO play drive run sound
            
            yield return new WaitForSeconds(1f);
            
            _tv.DisplayDiskHint();
        }
    }
}