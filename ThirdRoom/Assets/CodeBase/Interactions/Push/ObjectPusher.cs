using System.Collections;
using CodeBase.Controls;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace CodeBase.Interactions.Push
{
    public class ObjectPusher : MonoBehaviour
    {
        [SerializeField] private float _pushForce = 10f;
        [SerializeField] private float _pushRange = 2f;
        [SerializeField] private LayerMask _pushableLayer;
        [SerializeField] private float _infreezeTime = 0.5f;
        
        private InputService _inputService;

        [Inject]
        public void Construct(InputService inputService)
        {
            _inputService = inputService;
        }
        
        private void Update()
        {
            if (_inputService.IsPushInteractPressed)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, _pushRange, _pushableLayer))
                {
                    Rigidbody rb = hit.collider.attachedRigidbody;
                    if (rb != null)
                    {
                        StartCoroutine(PushObject(rb, hit.transform.position - transform.position));
                    }
                }
            }
        }

        private IEnumerator PushObject(Rigidbody rb, Vector3 direction)
        {
            rb.isKinematic = false;

            direction.y = 0f;
            direction.Normalize();

            rb.AddForce(direction * _pushForce, ForceMode.Impulse);

            yield return new WaitForSeconds(_infreezeTime);

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
}
