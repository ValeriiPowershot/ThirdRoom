﻿using System.Linq;
using CodeBase.Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
	[CustomEditor(typeof(UniqueId))]
	public class UniqueIdEditor : UnityEditor.Editor
	{
		private void OnEnable()
		{
			UniqueId uniqueId = (UniqueId) target;
      
			if(IsPrefab(uniqueId))
				return;
      
			if (string.IsNullOrEmpty(uniqueId.Id))
				Generate(uniqueId);
			else
			{
				UniqueId[] uniqueIds = FindObjectsByType<UniqueId>(FindObjectsSortMode.None);
        
				if(uniqueIds.Any(other => other != uniqueId && other.Id == uniqueId.Id))
					Generate(uniqueId);
			}
		}

		private bool IsPrefab(UniqueId uniqueId) => 
			uniqueId.gameObject.scene.rootCount == 0;

		private void Generate(UniqueId uniqueId)
		{
			uniqueId.GenerateId();

			if (!Application.isPlaying)
			{
				EditorUtility.SetDirty(uniqueId);
				EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
			}
		}
	}
}