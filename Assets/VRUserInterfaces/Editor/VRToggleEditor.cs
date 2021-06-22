//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Editor for VRToggle. Repeats VRPointerReceiverEditor but adds inactive values
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VRUserInterfaces
{
	[CustomEditor(typeof(VRToggle))]
	public class VRToggleEditor : VRPointerReceiverEditor
	{
		private SerializedProperty startEnabled;

		private SerializedProperty inactiveNormalSprite;
		private SerializedProperty inactiveHoverSprite;
		private SerializedProperty inactiveClickedSprite;

		private SerializedProperty inactiveNormalMat;
		private SerializedProperty inactiveHoverMat;
		private SerializedProperty inactiveClickedMat;

		private SerializedProperty inactiveNormalColor;
		private SerializedProperty inactiveHoverColor;
		private SerializedProperty inactiveClickedColor;

		private SerializedProperty inactiveNormalObject;
		private SerializedProperty inactiveHoverObject;
		private SerializedProperty inactiveClickedObject;

		public override void OnEnable()
		{
			startEnabled = serializedObject.FindProperty("startEnabled");

			inactiveNormalSprite = serializedObject.FindProperty("inactiveNormalSprite");
			inactiveHoverSprite = serializedObject.FindProperty("inactiveHoverSprite");
			inactiveClickedSprite = serializedObject.FindProperty("inactiveClickedSprite");

			inactiveNormalMat = serializedObject.FindProperty("inactiveNormalMat");
			inactiveHoverMat = serializedObject.FindProperty("inactiveHoverMat");
			inactiveClickedMat = serializedObject.FindProperty("inactiveClickedMat");

			inactiveNormalColor = serializedObject.FindProperty("inactiveNormalColor");
			inactiveHoverColor = serializedObject.FindProperty("inactiveHoverColor");
			inactiveClickedColor = serializedObject.FindProperty("inactiveClickedColor");

			inactiveNormalObject = serializedObject.FindProperty("inactiveNormalObject");
			inactiveHoverObject = serializedObject.FindProperty("inactiveHoverObject");
			inactiveClickedObject = serializedObject.FindProperty("inactiveClickedObject");

			base.OnEnable();
		}

		public override void OnInspectorGUI()
		{
			if (item.objectReferenceValue == null)
			{
				serializedObject.Update();
				item.objectReferenceValue = (Object)receiver.transform;
				serializedObject.ApplyModifiedProperties();
			}

			if (Application.isPlaying) return;
			serializedObject.Update();

			EditorGUILayout.PropertyField(targetRef);

			EditorGUILayout.PropertyField(whenToActivate);

			startEnabled.boolValue = EditorGUILayout.Toggle("Start Enabled", startEnabled.boolValue);

			//	Settings
			GUIContent startLockedContent = new GUIContent("Start Locked", "Must use the locked setter to unlock");
			startLocked.boolValue = EditorGUILayout.Toggle(startLockedContent, startLocked.boolValue);
			disableObjectOnStart.boolValue = EditorGUILayout.Toggle("Disable Object On Start", disableObjectOnStart.boolValue);

			if (targets.Length == 1 && anim != null)
			{
				onActivateAnimStateName.stringValue = EditorGUILayout.TextField("On Activate Anim State Name", onActivateAnimStateName.stringValue);
				if (receiver != null && receiver.GetType() == typeof(VRToggle))
					onDeactivateAnimStateName.stringValue = EditorGUILayout.TextField("On Deactivate Anim State Name", onDeactivateAnimStateName.stringValue);
			}

			//	Visual Settings
			GUIContent toggleableObjectsContent = new GUIContent("Use Toggleable Objects", "Assigned object will be enabled and disabled based on state");
			useToggleableObjects.boolValue = EditorGUILayout.Toggle(toggleableObjectsContent, useToggleableObjects.boolValue);
			if (useToggleableObjects.boolValue)
			{
				EditorGUILayout.LabelField("When Active", EditorStyles.boldLabel);
				normalObject.objectReferenceValue = EditorGUILayout.ObjectField("Default", normalObject.objectReferenceValue, typeof(GameObject), true);
				hoverObject.objectReferenceValue = EditorGUILayout.ObjectField("Hover", hoverObject.objectReferenceValue, typeof(GameObject), true);
				clickedObject.objectReferenceValue = EditorGUILayout.ObjectField("Clicked", clickedObject.objectReferenceValue, typeof(GameObject), true);
				EditorGUILayout.LabelField("When Inactive", EditorStyles.boldLabel);
				inactiveNormalObject.objectReferenceValue = EditorGUILayout.ObjectField("Inactive Default", inactiveNormalObject.objectReferenceValue, typeof(GameObject), true);
				inactiveHoverObject.objectReferenceValue = EditorGUILayout.ObjectField("Inactive Hover", inactiveHoverObject.objectReferenceValue, typeof(GameObject), true);
				inactiveClickedObject.objectReferenceValue = EditorGUILayout.ObjectField("Inactive Clicked", inactiveClickedObject.objectReferenceValue, typeof(GameObject), true);

				lockedObject.objectReferenceValue = EditorGUILayout.ObjectField("Locked", lockedObject.objectReferenceValue, typeof(GameObject), true);
			}

			if (targets.Length == 1 && image != null)
			{
				EditorGUILayout.LabelField("When Active", EditorStyles.boldLabel);
				if (image.sprite != null && image.sprite != (Sprite)normalSprite.objectReferenceValue) normalSprite.objectReferenceValue = (Object)image.sprite;
				var oldNormalSprite = normalSprite.objectReferenceValue;
				normalSprite.objectReferenceValue = EditorGUILayout.ObjectField("Default", normalSprite.objectReferenceValue, typeof(Sprite), false);
				if (normalSprite.objectReferenceValue != oldNormalSprite) image.sprite = (Sprite)normalSprite.objectReferenceValue;

				if (image.color != normalColor.colorValue) normalColor.colorValue = image.color;
				var oldNormalColor = normalColor.colorValue;
				normalColor.colorValue = EditorGUILayout.ColorField("Default", normalColor.colorValue);
				if (normalColor.colorValue != oldNormalColor) image.color = normalColor.colorValue;

				hoverSprite.objectReferenceValue = EditorGUILayout.ObjectField("Hover", hoverSprite.objectReferenceValue, typeof(Sprite), false);
				hoverColor.colorValue = EditorGUILayout.ColorField("Hover", hoverColor.colorValue);
				clickedSprite.objectReferenceValue = EditorGUILayout.ObjectField("Clicked", clickedSprite.objectReferenceValue, typeof(Sprite), false);
				clickedColor.colorValue = EditorGUILayout.ColorField("Clicked", clickedColor.colorValue);

				EditorGUILayout.LabelField("When Inactive", EditorStyles.boldLabel);

				inactiveNormalSprite.objectReferenceValue = EditorGUILayout.ObjectField("Inactive Default", inactiveNormalSprite.objectReferenceValue, typeof(Sprite), false);
				inactiveNormalColor.colorValue = EditorGUILayout.ColorField("Inactive Default", inactiveNormalColor.colorValue);
				inactiveHoverSprite.objectReferenceValue = EditorGUILayout.ObjectField("Inactive Hover", inactiveHoverSprite.objectReferenceValue, typeof(Sprite), false);
				inactiveHoverColor.colorValue = EditorGUILayout.ColorField("Inactive Hover", inactiveHoverColor.colorValue);
				inactiveClickedSprite.objectReferenceValue = EditorGUILayout.ObjectField("Inactive Clicked", inactiveClickedSprite.objectReferenceValue, typeof(Sprite), false);
				inactiveClickedColor.colorValue = EditorGUILayout.ColorField("Inactive Clicked", inactiveClickedColor.colorValue);

				lockedSprite.objectReferenceValue = EditorGUILayout.ObjectField("Locked", lockedSprite.objectReferenceValue, typeof(Sprite), false);
				lockedColor.colorValue = EditorGUILayout.ColorField("Locked", lockedColor.colorValue);

			} else if (targets.Length == 1 && meshRenderer != null)
			{
				EditorGUILayout.LabelField("When Active", EditorStyles.boldLabel);
				if (meshRenderer.sharedMaterial != null && meshRenderer.sharedMaterial != (Material)normalMat.objectReferenceValue) normalMat.objectReferenceValue = (Object)meshRenderer.sharedMaterial;
				var oldNormalMat = normalMat.objectReferenceValue;
				normalMat.objectReferenceValue = EditorGUILayout.ObjectField("Default", normalMat.objectReferenceValue, typeof(Material), false);
				if (normalMat.objectReferenceValue != oldNormalMat) meshRenderer.sharedMaterial = (Material)normalMat.objectReferenceValue;
				hoverMat.objectReferenceValue = EditorGUILayout.ObjectField("Hover", hoverMat.objectReferenceValue, typeof(Material), false);
				clickedMat.objectReferenceValue = EditorGUILayout.ObjectField("Clicked", clickedMat.objectReferenceValue, typeof(Material), false);

				EditorGUILayout.LabelField("When Inactive", EditorStyles.boldLabel);
				inactiveNormalMat.objectReferenceValue = EditorGUILayout.ObjectField("Inactive Default", inactiveNormalMat.objectReferenceValue, typeof(Material), false);
				inactiveHoverMat.objectReferenceValue = EditorGUILayout.ObjectField("Inactive Hover", inactiveHoverMat.objectReferenceValue, typeof(Material), false);
				inactiveClickedMat.objectReferenceValue = EditorGUILayout.ObjectField("Inactive Clicked", inactiveClickedMat.objectReferenceValue, typeof(Material), false);

				lockedMat.objectReferenceValue = EditorGUILayout.ObjectField("Locked", lockedMat.objectReferenceValue, typeof(Material), false);
			} else if (targets.Length == 1 && text != null)
			{
				EditorGUILayout.LabelField("When Active", EditorStyles.boldLabel);
				if (text.color != normalColor.colorValue) normalColor.colorValue = text.color;
				var oldNormalColor = normalColor.colorValue;
				normalColor.colorValue = EditorGUILayout.ColorField("Default", normalColor.colorValue);
				if (normalColor.colorValue != oldNormalColor) text.color = normalColor.colorValue;
				hoverColor.colorValue = EditorGUILayout.ColorField("Hover", hoverColor.colorValue);
				clickedColor.colorValue = EditorGUILayout.ColorField("Clicked", clickedColor.colorValue);

				EditorGUILayout.LabelField("When Inactive", EditorStyles.boldLabel);
				inactiveNormalColor.colorValue = EditorGUILayout.ColorField("Inactive Default", inactiveNormalColor.colorValue);
				inactiveHoverColor.colorValue = EditorGUILayout.ColorField("Inactive Hover", inactiveHoverColor.colorValue);
				inactiveClickedColor.colorValue = EditorGUILayout.ColorField("Inactive Clicked", inactiveClickedColor.colorValue);

				lockedColor.colorValue = EditorGUILayout.ColorField("Locked", lockedColor.colorValue);
			} else if (!useToggleableObjects.boolValue)
				EditorGUILayout.HelpBox("Use toggleable objects or add either an Image or Mesh Renderer component for visual options", MessageType.Warning);

			//	Event Settings
			useSendMessage.boolValue = EditorGUILayout.Toggle("Use Send Message", useSendMessage.boolValue);
			if (useSendMessage.boolValue)
			{
				EditorGUILayout.HelpBox("Target Object: Calls Method Name on every MonoBehaviour in this Gameobject when activated\n" +
					"Target Method Name: Method that will be called on target object when activated\n" + 
					"String Data: string parameter that will be sent on method call\n" +
					"Target method should look something like this:\n" +
					"void methodname(string data){}", MessageType.Info);

				if (targetObject.objectReferenceValue == null) EditorGUILayout.HelpBox("No assigned target object. Clicking will not activate anything", MessageType.Warning);
				targetObject.objectReferenceValue = EditorGUILayout.ObjectField("Target Object", targetObject.objectReferenceValue, typeof(GameObject), true);
				targetMethod.stringValue = EditorGUILayout.TextField("Target Method Name", targetMethod.stringValue);
				stringData.stringValue = EditorGUILayout.TextField("String Data", stringData.stringValue);

				if (receiver != null && receiver.GetType() == typeof(VRToggle))
				{
					if (targetObjectDeactivate.objectReferenceValue == null) EditorGUILayout.HelpBox("No assigned target object. Clicking will not activate anything", MessageType.Warning);
					targetObjectDeactivate.objectReferenceValue = EditorGUILayout.ObjectField("Target Object Deactivate", targetObjectDeactivate.objectReferenceValue, typeof(GameObject), true);
					targetMethodDeactivate.stringValue = EditorGUILayout.TextField("Target Method Name Deactivate", targetMethodDeactivate.stringValue);
					stringDataDeactivate.stringValue = EditorGUILayout.TextField("String Data Deactivate", stringDataDeactivate.stringValue);
				}
			}
			useUnityEvent.boolValue = EditorGUILayout.Toggle("Use Unity Event", useUnityEvent.boolValue);
			if (useUnityEvent.boolValue)
			{
				EditorGUILayout.PropertyField(unityEvent);
				if (receiver != null && receiver.GetType() == typeof(VRToggle)) EditorGUILayout.PropertyField(unityEventDeactivate);
			}

			serializedObject.ApplyModifiedProperties();

			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

			ItemSection(serializedObject, false);

		}
	}
}
#endif