using System;
using UnityEngine;

namespace CodeBase.Data
{
	public class Item : MonoBehaviour
	{
		[field: SerializeField] public string ItemName { get; private set; }
		[field: SerializeField] public GameObject Item3DModel { get; private set; }
		[field: SerializeField] public string ItemDescription { get; private set; }
		public int Index;
	}
}