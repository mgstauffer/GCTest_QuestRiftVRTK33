//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Inherits from VRPointerReceiverEditor. Adds the min max variables and displays
// arrow lines to in the scene to demonstrate the range of movement.
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VRUserInterfaces
{
	[CustomEditor(typeof(VRSlider))]
	public class VRSliderEditor : VRPointerReceiverEditor
	{
		private SerializedProperty constrainX;
		private SerializedProperty constrainY;
		private SerializedProperty constrainZ;

		private SerializedProperty XMin;
		private SerializedProperty XMax;
		private SerializedProperty YMin;
		private SerializedProperty YMax;
		private SerializedProperty ZMin;
		private SerializedProperty ZMax;

		private SerializedProperty editorArrowSize;

		public override void OnEnable()
		{
			constrainX = serializedObject.FindProperty("constrainX");
			constrainY = serializedObject.FindProperty("constrainY");
			constrainZ = serializedObject.FindProperty("constrainZ");

			XMin = serializedObject.FindProperty("XMin");
			XMax = serializedObject.FindProperty("XMax");
			YMin = serializedObject.FindProperty("YMin");
			YMax = serializedObject.FindProperty("YMax");
			ZMin = serializedObject.FindProperty("ZMin");
			ZMax = serializedObject.FindProperty("ZMax");

			editorArrowSize = serializedObject.FindProperty("editorArrowSize");
			showEvents = false;
			base.OnEnable();
		}

		public override void OnInspectorGUI()
		{
			if (receiver == null) return;
			if (receiver.transform.parent == null)
			{
				EditorGUILayout.HelpBox("Slider must have a parent transform to work", MessageType.Error);
				return;
			}

			if (Application.isPlaying) return;
			serializedObject.Update();

			var oldSize = editorArrowSize.floatValue;
			editorArrowSize.floatValue = EditorGUILayout.Slider("Editor Arrow Size", editorArrowSize.floatValue, 0.01f, 1);
			if (oldSize != editorArrowSize.floatValue) SceneView.RepaintAll();

			constrainX.boolValue = EditorGUILayout.Toggle("Constrain X", constrainX.boolValue);
			if (!constrainX.boolValue)
			{
				XMin.floatValue = EditorGUILayout.FloatField("XMin", XMin.floatValue);
				XMax.floatValue = EditorGUILayout.FloatField("XMax", XMax.floatValue);
				if (XMin.floatValue > XMax.floatValue) XMax.floatValue = XMin.floatValue + 0.1f;
			}
			constrainY.boolValue = EditorGUILayout.Toggle("Constrain Y", constrainY.boolValue);
			if (!constrainY.boolValue)
			{
				YMin.floatValue = EditorGUILayout.FloatField("YMin", YMin.floatValue);
				YMax.floatValue = EditorGUILayout.FloatField("YMax", YMax.floatValue);
				if (YMin.floatValue > YMax.floatValue) YMax.floatValue = YMin.floatValue + 0.1f;
			}
			constrainZ.boolValue = EditorGUILayout.Toggle("Constrain Z", constrainZ.boolValue);
			if (!constrainZ.boolValue)
			{
				ZMin.floatValue = EditorGUILayout.FloatField("ZMin", ZMin.floatValue);
				ZMax.floatValue = EditorGUILayout.FloatField("ZMax", ZMax.floatValue);
				if (ZMin.floatValue > ZMax.floatValue) ZMax.floatValue = ZMin.floatValue + 0.1f;
			}
			serializedObject.ApplyModifiedProperties();

			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

			base.OnInspectorGUI();
		}

		void OnSceneGUI()
		{
			if (receiver == null || receiver.transform.parent == null) return;
			Handles.color = Color.blue;
			if (!constrainX.boolValue)
			{
				Vector3 lineStart = receiver.transform.parent.TransformPoint(new Vector3(XMin.floatValue, receiver.transform.localPosition.y, receiver.transform.localPosition.z));
				Vector3 lineEnd = receiver.transform.parent.TransformPoint(new Vector3(XMax.floatValue, receiver.transform.localPosition.y, receiver.transform.localPosition.z));
				if (lineStart != lineEnd)
				{
					Handles.DrawLine(lineStart, lineEnd);
					Handles.ArrowHandleCap(0, lineStart+(receiver.transform.parent.rotation*new Vector3(editorArrowSize.floatValue,0,0)), Quaternion.LookRotation(lineStart-lineEnd), editorArrowSize.floatValue, EventType.Repaint);
					Handles.ArrowHandleCap(0, lineEnd-(receiver.transform.parent.rotation*new Vector3(editorArrowSize.floatValue,0,0)), Quaternion.LookRotation(lineEnd-lineStart), editorArrowSize.floatValue, EventType.Repaint);
				}
			}

			if (!constrainY.boolValue)
			{
				Vector3 lineStart = receiver.transform.parent.TransformPoint(new Vector3(receiver.transform.localPosition.x, YMin.floatValue, receiver.transform.localPosition.z));
				Vector3 lineEnd = receiver.transform.parent.TransformPoint(new Vector3(receiver.transform.localPosition.x, YMax.floatValue, receiver.transform.localPosition.z));
				if (lineStart != lineEnd)
				{
					Handles.DrawLine(lineStart, lineEnd);
					Handles.ArrowHandleCap(0, lineStart+(receiver.transform.parent.rotation*new Vector3(0,editorArrowSize.floatValue,0)), Quaternion.LookRotation(lineStart-lineEnd), editorArrowSize.floatValue, EventType.Repaint);
					Handles.ArrowHandleCap(0, lineEnd-(receiver.transform.parent.rotation*new Vector3(0,editorArrowSize.floatValue,0)), Quaternion.LookRotation(lineEnd-lineStart), editorArrowSize.floatValue, EventType.Repaint);
				}
			}

			if (!constrainZ.boolValue)
			{
				Vector3 lineStart = receiver.transform.parent.TransformPoint(new Vector3(receiver.transform.localPosition.x, receiver.transform.localPosition.y, ZMin.floatValue));
				Vector3 lineEnd = receiver.transform.parent.TransformPoint(new Vector3(receiver.transform.localPosition.x, receiver.transform.localPosition.y, ZMax.floatValue));
				if (lineStart != lineEnd)
				{
					Handles.DrawLine(lineStart, lineEnd);
					Handles.ArrowHandleCap(0, lineStart+(receiver.transform.parent.rotation*new Vector3(0,0,editorArrowSize.floatValue)), Quaternion.LookRotation(lineStart-lineEnd), editorArrowSize.floatValue, EventType.Repaint);
					Handles.ArrowHandleCap(0, lineEnd-(receiver.transform.parent.rotation*new Vector3(0,0,editorArrowSize.floatValue)), Quaternion.LookRotation(lineEnd-lineStart), editorArrowSize.floatValue, EventType.Repaint);
				}
			}
		}
	}
}
#endif