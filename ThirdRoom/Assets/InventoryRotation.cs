using System;
using UnityEngine;

public class InventoryRotation : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 10f);
    }
}
