//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Editor for PointerReceiver. Looks for Image, MeshRenderer, Animator and Text
// components and displays corrosponding values.
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using VRInteraction;

namespace VRUserInterfaces
{
	[CustomEditor(typeof(VRPointerReceiver))]
	public class VRPointerReceiverEditor : VRInteractableItemEditor
	{
		// target component
		public VRPointerReceiver receiver = null;

		protected SerializedProperty item;

		protected SerializedProperty targetRef;
		protected SerializedProperty startLocked;
		protected SerializedProperty disableObjectOnStart;
		protected SerializedProperty whenToActivate;

		protected SerializedProperty useToggleableObjects;
		protected SerializedProperty normalObject;
		protected SerializedProperty hoverObject;
		protected SerializedProperty clickedObject;
		protected SerializedProperty lockedObject;

		protected SerializedProperty normalSprite;
		protected SerializedProperty hoverSprite;
		protected SerializedProperty clickedSprite;
		protected SerializedProperty lockedSprite;

		protected SerializedProperty normalMat;
		protected SerializedProperty hoverMat;
		protected SerializedProperty clickedMat;
		protected SerializedProperty lockedMat;

		protected SerializedProperty normalColor;
		protected SerializedProperty hoverColor;
		protected SerializedProperty clickedColor;
		protected SerializedProperty lockedColor;

		protected SerializedProperty onActivateAnimStateName;
		protected SerializedProperty onDeactivateAnimStateName;
		protected SerializedProperty useSendMessage;
		protected SerializedProperty useUnityEvent;
		protected SerializedProperty unityEvent;
		protected SerializedProperty unityEventDeactivate;
		protected SerializedProperty targetObject;
		protected SerializedProperty targetMethod;
		protected SerializedProperty stringData;
		protected SerializedProperty targetObjectDeactivate;
		protected SerializedProperty targetMethodDeactivate;
		protected SerializedProperty stringDataDeactivate;

		protected UnityEngine.UI.Image image;
		protected MeshRenderer meshRenderer;
		protected Animator anim;
		protected Text text;

		protected bool showEvents = true;

		public override void OnEnable()
		{
			base.OnEnable();

			if (targets.Length == 1)
			{
				receiver = (VRPointerReceiver)target;
				image = receiver.getTarget.GetComponent<UnityEngine.UI.Image>();
				meshRenderer = receiver.getTarget.GetComponent<MeshRenderer>();
				anim = receiver.getTarget.GetComponent<Animator>();
				text = receiver.getTarget.GetComponent<Text>();
			}

			item = serializedObject.FindProperty("item");
			targetRef = serializedObject.FindProperty("target");
			startLocked = serializedObject.FindProperty("startLocked");
			disableObjectOnStart = serializedObject.FindProperty("disableObjectOnStart");
			whenToActivate = serializedObject.FindProperty("whenToActivate");

			useToggleableObjects = serializedObject.FindProperty("useToggleableObjects");
			normalObject = serializedObject.FindProperty("normalObject");
			hoverObject = serializedObject.FindProperty("hoverObject");
			clickedObject = serializedObject.FindProperty("clickedObject");
			lockedObject = serializedObject.FindProperty("lockedObject");

			normalSprite = serializedObject.FindProperty("normalSprite");
			hoverSprite = serializedObject.FindProperty("hoverSprite");
			clickedSprite = serializedObject.FindProperty("clickedSprite");
			lockedSprite = serializedObject.FindProperty("lockedSprite");

			normalMat = serializedObject.FindProperty("normalMat");
			hoverMat = serializedObject.FindProperty("hoverMat");
			clickedMat = serializedObject.FindProperty("clickedMat");
			lockedMat = serializedObject.FindProperty("lockedMat");

			normalColor = serializedObject.FindProperty("normalColor");
			hoverColor = serializedObject.FindProperty("hoverColor");
			clickedColor = serializedObject.FindProperty("clickedColor");
			lockedColor = serializedObject.FindProperty("lockedColor");

			onActivateAnimStateName = serializedObject.FindProperty("onActivateAnimStateName");
			onDeactivateAnimStateName = serializedObject.FindProperty("onDeactivateAnimStateName");

			useSendMessage = serializedObject.FindProperty("useSendMessage");
			useUnityEvent = serializedObject.FindProperty("useUnityEvent");
			unityEvent = serializedObject.FindProperty("unityEvent");
			unityEventDeactivate = serializedObject.FindProperty("unityEventDeactivate");

			targetObject = serializedObject.FindProperty("targetObject");
			targetMethod = serializedObject.FindProperty("targetMethod");
			stringData = serializedObject.FindProperty("stringData");
			targetObjectDeactivate = serializedObject.FindProperty("targetObjectDeactivate");
			targetMethodDeactivate = serializedObject.FindProperty("targetMethodDeactivate");
			stringDataDeactivate = serializedObject.FindProperty("stringDataDeactivate");
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

			//	Settings
			EditorGUILayout.PropertyField(targetRef);

			EditorGUILayout.PropertyField(whenToActivate);

			GUIContent startLockedContent = new GUIContent("Start Locked", "Must use the locked setter to unlock");
			startLocked.boolValue = EditorGUILayout.Toggle(startLockedContent, startLocked.boolValue);
			disableObjectOnStart.boolValue = EditorGUILayout.Toggle("Disable Object On Start", disableObjectOnStart.boolValue);

			if (targets.Length == 1 && anim != null)
			{
				onActivateAnimStateName.stringValue = EditorGUILayout.TextField("On Activate Anim State Name", onActivateAnimStateName.stringValue);
				if (receiver.GetType() == typeof(VRToggle))
					onDeactivateAnimStateName.stringValue = EditorGUILayout.TextField("On Deactivate Anim State Name", onDeactivateAnimStateName.stringValue);
			}

			//	Visual Settings
			GUIContent toggleableObjectsContent = new GUIContent("Use Toggleable Objects", "Assigned object will be enabled and disabled based on state");
			useToggleableObjects.boolValue = EditorGUILayout.Toggle(toggleableObjectsContent, useToggleableObjects.boolValue);
			if (useToggleableObjects.boolValue)
			{
				normalObject.objectReferenceValue = EditorGUILayout.ObjectField("Default", normalObject.objectReferenceValue, typeof(GameObject), true);
				hoverObject.objectReferenceValue = EditorGUILayout.ObjectField("Hover", hoverObject.objectReferenceValue, typeof(GameObject), true);
				clickedObject.objectReferenceValue = EditorGUILayout.ObjectField("Clicked", clickedObject.objectReferenceValue, typeof(GameObject), true);
				lockedObject.objectReferenceValue = EditorGUILayout.ObjectField("Locked", lockedObject.objectReferenceValue, typeof(GameObject), true);
			}
				
			if (targets.Length == 1 && image != null)
			{
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

				lockedSprite.objectReferenceValue = EditorGUILayout.ObjectField("Locked", lockedSprite.objectReferenceValue, typeof(Sprite), false);
				lockedColor.colorValue = EditorGUILayout.ColorField("Locked", lockedColor.colorValue);

			} else if (targets.Length == 1 && meshRenderer != null)
			{
				if (meshRenderer.sharedMaterial != null && meshRenderer.sharedMaterial != (Material)normalMat.objectReferenceValue) normalMat.objectReferenceValue = (Object)meshRenderer.sharedMaterial;
				var oldNormalMat = normalMat.objectReferenceValue;
				normalMat.objectReferenceValue = EditorGUILayout.ObjectField("Default", normalMat.objectReferenceValue, typeof(Material), false);
				if (normalMat.objectReferenceValue != oldNormalMat) meshRenderer.sharedMaterial = (Material)normalMat.objectReferenceValue;
				hoverMat.objectReferenceValue = EditorGUILayout.ObjectField("Hover", hoverMat.objectReferenceValue, typeof(Material), false);
				clickedMat.objectReferenceValue = EditorGUILayout.ObjectField("Clicked", clickedMat.objectReferenceValue, typeof(Material), false);
				lockedMat.objectReferenceValue = EditorGUILayout.ObjectField("Locked", lockedMat.objectReferenceValue, typeof(Material), false);
			} else if (targets.Length == 1 && text != null)
			{
				if (text.color != normalColor.colorValue) normalColor.colorValue = text.color;
				var oldNormalColor = normalColor.colorValue;
				normalColor.colorValue = EditorGUILayout.ColorField("Default", normalColor.colorValue);
				if (normalColor.colorValue != oldNormalColor) text.color = normalColor.colorValue;
				hoverColor.colorValue = EditorGUILayout.ColorField("Hover", hoverColor.colorValue);
				clickedColor.colorValue = EditorGUILayout.ColorField("Clicked", clickedColor.colorValue);
				lockedColor.colorValue = EditorGUILayout.ColorField("Locked", lockedColor.colorValue);
			} else if (!useToggleableObjects.boolValue)
				EditorGUILayout.HelpBox("Use toggleable objects or add either an Image or Mesh Renderer component for visual options", MessageType.Warning);

			if (showEvents)
			{
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
			}

			serializedObject.ApplyModifiedProperties();

			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

			ItemSection(serializedObject, false);
		}
	}
}
#endif