using CodeBase.Data;
using CodeBase.Interactions;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Environment
{
    public class Disk : Item
    {
        [field: SerializeField] public DiskType DiskType { get; private set; }
        [field: SerializeField] public GameObject Disk3DModel { get; private set; }
    }
}