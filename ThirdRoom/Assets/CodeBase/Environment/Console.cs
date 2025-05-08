using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Audio;
using CodeBase.Data;
using CodeBase.Interactions;
using CodeBase.Inventory.Controller;
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
        private InventoryController _inventory;
        private Dictionary<DiskType, Action> _diskActions;
        
        [Inject]
        private void Construct(FMODAudioPlayer audioPlayer, InventoryController inventory)
        {
            _audioPlayer = audioPlayer;
            _inventory = inventory;
        }

        private void Start()
        {
            _diskActions = new Dictionary<DiskType, Action>
            {
                { DiskType.Gloom , GloomTvEffect},
                { DiskType.Hidden, HiddenTvEffect},
                { DiskType.Steampunk, SteamTvEffect},
                { DiskType.Whisper, WhisperTvEffect},
                
            };
        }

        private void SteamTvEffect()
        {
        }

        private void WhisperTvEffect()
        {
        }

        private void HiddenTvEffect()
        {
        }

        private void GloomTvEffect()
        {
        }

        protected override void OnInteract()
        {
            _inventory.OnItemSelected += OnItemSelected; 
            _inventory.OpenInventory();
            // StartCoroutine(TryInsertAndValidateDiskRoutine(_insertedDisk));
        }

        private void OnItemSelected(Item obj)
        {
            if (obj is not Disk disk)
                return;

            _inventory.OnItemSelected -= OnItemSelected;
            StartCoroutine(TryInsertAndValidateDiskRoutine(disk));
        }

        public void AllowInteract()
            => IsInteractable = true;
        
        private IEnumerator TryInsertAndValidateDiskRoutine(Disk disk)
        {
            _insertedDisk = disk;
            _insertedDisk.transform.position = _initialPlacePoint.position;
            _insertedDisk.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            
            if (_diskActions.TryGetValue(disk.DiskType, out Action action))
                action.Invoke();
   
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