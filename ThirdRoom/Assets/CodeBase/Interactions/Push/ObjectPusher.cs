using System.Collections;
using UnityEngine;

namespace CodeBase.Interactions.Push
{
    public class ObjectPusher : MonoBehaviour
    {
        public float pushForce = 10f;
        public float pushRange = 2f;
        public LayerMask pushableLayer;
        public float unfreezeTime = 0.5f;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, pushRange, pushableLayer))
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

            rb.AddForce(direction * pushForce, ForceMode.Impulse);

            yield return new WaitForSeconds(unfreezeTime);

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
}
