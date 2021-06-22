//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Editor for VRPointer. Button for applying a preset set of controls and adds
// fields for modifying values related to the PointerMode it is set to.
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRInteraction;
using UnityEditor.SceneManagement;

namespace VRUserInterfaces
{
	[CustomEditor(typeof(VRPointer))]
	public class VRPointerEditor : Editor 
	{
		private VRPointer pointer;
		private VRInput input;

		public virtual void OnEnable()
		{
			pointer = (VRPointer)target;
			VRInteractor interactor = pointer.GetComponent<VRInteractor>();
			if (interactor == null) interactor = pointer.gameObject.AddComponent<VRInteractor>();
			input = pointer.GetComponent<VRInput>();
			if (input == null) input = pointer.gameObject.AddComponent<VRInput>();
			if (input.getVRActions == null) SetDefaultActions(input);
		}

		private void SetDefaultActions(VRInput input)
		{
			string[] premadeActions = {"NONE", "ACTION"};
			input.triggerKey = 1;
			input.triggerKeyOculus = 1;
			input.getVRActions = premadeActions;
			EditorUtility.SetDirty(input.gameObject);
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.HelpBox("ACTION or PICKUP_DROP: activates UI elements", MessageType.Info);

			if (GUILayout.Button("Reset Actions to Pointer Default")) SetDefaultActions(input);

			serializedObject.Update();

			SerializedProperty pointerLayer = serializedObject.FindProperty("pointerLayer");
			EditorGUILayout.PropertyField(pointerLayer);


			SerializedProperty pointerTransform = serializedObject.FindProperty("pointerTransform");
			var oldPointerTransform = pointerTransform.objectReferenceValue;
			EditorGUILayout.PropertyField(pointerTransform);
			if (pointerTransform.objectReferenceValue == null)
			{
				Transform newPointerTransform = null;
				foreach(Transform trans in pointer.transform)
				{
					if (trans.name == VRPointer.pointerTransformName)
					{
						newPointerTransform = trans;
						break;
					}
				}
				if (newPointerTransform == null) pointer.pointerTransform = pointer.getPointerTransform;
				else pointer.pointerTransform = newPointerTransform;
				EditorUtility.SetDirty(pointer);
				EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
			}

			SerializedProperty pointerMaterial = serializedObject.FindProperty("pointerMaterial");
			if (pointerMaterial.objectReferenceValue == null)
			{
				string[] results = AssetDatabase.FindAssets("ExamplePointerMat");
				foreach(string result in results)
				{
					if (result == null || result == "") continue;
					Material examplePointerMat = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(result));
					if (examplePointerMat != null)
					{
						pointerMaterial.objectReferenceValue = (Object)examplePointerMat;
						break;
					}
				}
			}
			EditorGUILayout.PropertyField(pointerMaterial);

			SerializedProperty hitPrefab = serializedObject.FindProperty("hitPrefab");
			if (hitPrefab.objectReferenceValue == null)
			{
				string[] results = AssetDatabase.FindAssets("ExampleHit");
				foreach(string result in results)
				{
					if (result == null || result == "") continue;
					GameObject exampleHit = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(result));
					if (exampleHit != null)
					{
						hitPrefab.objectReferenceValue = (Object)exampleHit;
						break;
					}
				}
			}
			hitPrefab.objectReferenceValue = EditorGUILayout.ObjectField("Hit Prefab", hitPrefab.objectReferenceValue, typeof(GameObject), false);

			SerializedProperty maxPointerRange = serializedObject.FindProperty("maxPointerRange");
			EditorGUILayout.PropertyField(maxPointerRange);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif