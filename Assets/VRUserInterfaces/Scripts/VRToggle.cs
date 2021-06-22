//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Inherits from VRPointerReceiver. Implements the activate method and keeps
// track of being enabled, will activate two events or messages, one for activating
// and another for deactivating. Will also play two animations for activating and
// deactivating.
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRInteraction;

namespace VRUserInterfaces
{
	public class VRToggle : VRPointerReceiver
	{
		public bool startEnabled = true;

		//	Visual Settings
		public Sprite inactiveNormalSprite;
		public Sprite inactiveHoverSprite;
		public Sprite inactiveClickedSprite;

		public Material inactiveNormalMat;
		public Material inactiveHoverMat;
		public Material inactiveClickedMat;

		public Color inactiveNormalColor;
		public Color inactiveHoverColor;
		public Color inactiveClickedColor;

		public GameObject inactiveNormalObject;
		public GameObject inactiveHoverObject;
		public GameObject inactiveClickedObject;

		private bool _enabled;
		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				DisableHover();
			}
		}

		override protected void Init()
		{
			_enabled = startEnabled;
			base.Init();
		}

		override public void EnableHover(VRInteractor hand = null)
		{
			if (locked) return;
			if (_enabled) base.EnableHover(hand);
			else
			{
				if (image != null)
				{
					image.color = inactiveHoverColor;
					if (inactiveHoverSprite != null)
						image.sprite = inactiveHoverSprite;
				}
				if (meshRenderer != null && inactiveHoverMat != null)
					meshRenderer.material = inactiveHoverMat;
				if (text != null) text.color = inactiveHoverColor;
				if (useToggleableObjects)
				{
					if (inactiveHoverObject != null) inactiveHoverObject.SetActive(true);
					if (inactiveNormalObject != null) inactiveNormalObject.SetActive(false);
					if (inactiveClickedObject != null) inactiveClickedObject.SetActive(false);
					if (lockedObject != null) lockedObject.SetActive(false);
				}
			}
		}

		override public void DisableHover(VRInteractor hand = null)
		{
			if (locked) return;
			if (_enabled) base.DisableHover(hand);
			else
			{
				if (image != null)
				{
					image.color = inactiveNormalColor;
					if (inactiveNormalSprite != null)
						image.sprite = inactiveNormalSprite;
				}
				if (meshRenderer != null && inactiveNormalMat != null)
					meshRenderer.material = inactiveNormalMat;
				if (text != null) text.color = inactiveNormalColor;
				if (useToggleableObjects)
				{
					if (inactiveNormalObject != null) inactiveNormalObject.SetActive(true);
					if (inactiveHoverObject != null) inactiveHoverObject.SetActive(false);
					if (inactiveClickedObject != null) inactiveClickedObject.SetActive(false);
					if (lockedObject != null) lockedObject.SetActive(false);
				}
			}
		}

		override public bool Pickup(VRInteractor hand)
		{
			if (locked) return false;
			if (_enabled) base.Pickup(hand);
			else
			{
				if (image != null)
				{
					image.color = inactiveClickedColor;
					if (inactiveClickedSprite != null)
						image.sprite = inactiveClickedSprite;
				}
				if (meshRenderer != null && inactiveClickedMat != null)
					meshRenderer.material = inactiveClickedMat;
				if (text != null) text.color = inactiveClickedColor;
				if (useToggleableObjects)
				{
					if (inactiveClickedObject != null) inactiveClickedObject.SetActive(true);
					if (inactiveNormalObject != null) inactiveNormalObject.SetActive(false);
					if (inactiveHoverObject != null) inactiveHoverObject.SetActive(false);
					if (lockedObject != null) lockedObject.SetActive(false);
				}
				if (whenToActivate == WhenToActivate.ACTIVATE_ON_CLICK) Activate(hand);
			}
			return true;
		}

		override public void Activate(VRInteractor hand)
		{
			if (locked) return;

			_enabled = !_enabled;

			if (anim != null)
			{
				if (_enabled && onActivateAnimStateName != "")
					anim.Play(onActivateAnimStateName);
				else if (!_enabled && onDeactivateAnimStateName != "")
					anim.Play(onDeactivateAnimStateName);
			}

			if (useUnityEvent && unityEvent != null)
			{
				if (_enabled)
					unityEvent.Invoke();
				else
					unityEventDeactivate.Invoke();

			}
			if (useSendMessage)
			{
				if (!_enabled && targetObjectDeactivate != null && targetMethodDeactivate != "")
					targetObjectDeactivate.SendMessage(targetMethodDeactivate, stringDataDeactivate, SendMessageOptions.DontRequireReceiver);
				else if (targetObject != null && targetMethod != "")
					targetObject.SendMessage(targetMethod, stringData, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
#endif