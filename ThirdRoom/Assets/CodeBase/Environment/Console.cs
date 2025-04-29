using CodeBase.Interactions;
using UnityEngine;

namespace CodeBase.Environment
{
    public class Console : InteractObject
    {
        [SerializeField] private TV _tv;

        [Header("Insert settings")] 
        [SerializeField] private Transform _initialPlacePoint;
        [SerializeField] private Transform _targetPlacePoint;
        
        private Disk _insertedDisk;
        
        protected override void OnInteract()
        {
            if (TryRemoveDisk()) return;
            
        }
        
        public void AllowInteract()
            => IsInteractable = true;

        private bool TryRemoveDisk()
        {
            if (_insertedDisk != null)
            {
                // TODO do some stuff
                return true;
            }

            return false;
        }
        
        private void TryInsertAndValidateDisk()
        {
            
        }
    }
}