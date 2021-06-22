//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Attach this script to the VRInteractableItem script on a controller menu.
// Clicking the MENU action will swap the menu and hidden menu. You can use this
// to have a collapsable menu
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRInteraction;

namespace VRUserInterfaces
{
	public class ControllerMenu : MonoBehaviour 
	{
		public GameObject menuRoot;
		public GameObject hiddenMenuRoot;

		public bool hideOnStart = false;
		public bool allowRotation = true;
		public float rotateWithPadSpeed = 1f;

		private VRInteractableItem _item;
		private bool _showingUI;
		private bool _touched;
		private float _lastTouchX;
		private Vector3 _rotationOffset = Vector3.zero;

		void Start()
		{
			if (menuRoot == null || hiddenMenuRoot == null)
			{
				Debug.LogError("Menu root is not referenced");
				return;
			}
			_item = GetComponent<VRInteractableItem>();
			if (_item == null) Debug.LogError("This script should be on the same object as an interactable item");
			_showingUI = !hideOnStart;
			menuRoot.SetActive(_showingUI);
			hiddenMenuRoot.SetActive(!_showingUI);
		}

		void MENU(VRInteractor hand)
		{
			_showingUI = !_showingUI;
			menuRoot.SetActive(_showingUI);
			hiddenMenuRoot.SetActive(!_showingUI);
		}

		void Update()
		{
			if (!allowRotation || rotateWithPadSpeed == 0f || _item == null || _item.heldBy == null) return;

			if (!_touched && _item.heldBy.vrInput.PadTouched)
			{
				_touched = true;
				_lastTouchX = _item.heldBy.vrInput.PadPosition.x;
			} else if (_touched && !_item.heldBy.vrInput.PadTouched)
			{
				_touched = false;
			}

			if (!_touched || _item.heldBy.vrInput.PadPosition.x == 0) return;

			float diff = 0;
			if (_item.heldBy.vrInput.hmdType == VRInput.HMDType.VIVE)
				diff = (_lastTouchX - _item.heldBy.vrInput.PadPosition.x)*(rotateWithPadSpeed*100f);
			else
				diff = -_item.heldBy.vrInput.PadPosition.x * rotateWithPadSpeed;
			
			_rotationOffset = new Vector3(0f, 0f, diff);
			_item.heldRotation = _item.heldRotation * Quaternion.Euler(_rotationOffset);
			_lastTouchX = _item.heldBy.vrInput.PadPosition.x;
		}
	}
}
#endif