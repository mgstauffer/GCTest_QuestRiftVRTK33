//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Receives input from VRPointer and relays SendMessage to a given target object
// This script should be on an object with a collider (Make sure the layer
// corrosponds with the VRPointer LayerMask setting).
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VRInteraction;

namespace VRUserInterfaces
{
	public class VRPointerReceiver : VRInteractableItem
	{
		public enum WhenToActivate
		{
			ACTIVATE_ON_CLICK,
			ACTIVATE_ON_RELEASE
		}

		//	Settings
		public Transform target;
		public bool startLocked;
		public bool disableObjectOnStart;
		public WhenToActivate whenToActivate;

		//	Visual Settings
		public Sprite normalSprite;
		public Sprite hoverSprite;
		public Sprite clickedSprite;
		public Sprite lockedSprite;

		public Material normalMat;
		public Material hoverMat;
		public Material clickedMat;
		public Material lockedMat;

		public Color normalColor = Color.white;
		public Color hoverColor = Color.white;
		public Color clickedColor = Color.white;
		public Color lockedColor = Color.white;

		public bool useToggleableObjects;
		public GameObject normalObject;
		public GameObject hoverObject;
		public GameObject clickedObject;
		public GameObject lockedObject;

		public string onActivateAnimStateName = "";
		public string onDeactivateAnimStateName = "";
		//	Event Settings
		public bool useUnityEvent;
		public UnityEvent unityEvent;
		public UnityEvent unityEventDeactivate;

		public bool useSendMessage;
		public GameObject targetObject;
		public string targetMethod = "";
		public string stringData = "";

		public GameObject targetObjectDeactivate;
		public string targetMethodDeactivate = "";
		public string stringDataDeactivate = "";

		protected UnityEngine.UI.Image image;
		protected MeshRenderer meshRenderer;
		protected Text text;
		protected Animator anim;

		public Transform getTarget
		{
			get 
			{
				if (target == null) target = transform;
				return target; 
			}
		}

		protected bool _locked;
		virtual public bool locked
		{
			get { return _locked; }
			set 
			{ 
				_locked = value;
				if (_locked)
				{
					if (meshRenderer != null && lockedMat != null)
						meshRenderer.material = lockedMat;
					if (image != null)
					{
						image.color = lockedColor;
						if (lockedSprite != null)
							image.sprite = lockedSprite;
					}
					if (text != null) text.color = lockedColor;
					if (useToggleableObjects)
					{
						if (lockedObject != null) lockedObject.SetActive(true);
						if (normalObject != null) normalObject.SetActive(false);
						if (hoverObject != null) hoverObject.SetActive(false);
						if (clickedObject != null) clickedObject.SetActive(false);
					}
				} else DisableHover();
			}
		}

		override protected void Init()
		{
			base.Init();
			anim = getTarget.GetComponent<Animator>();
			image = getTarget.GetComponent<UnityEngine.UI.Image>();
			meshRenderer = getTarget.GetComponent<MeshRenderer>();
			text = getTarget.GetComponent<Text>();

			if (startLocked || locked) locked = true;
			else DisableHover();
			if (disableObjectOnStart) getTarget.gameObject.SetActive(false);
		}

		override protected void OnDisable()
		{
			base.OnDisable();
			DisableHover();
		}

		override public void EnableHover(VRInteractor hand = null)
		{
			if (locked || activeHover || interactionDisabled) return;
			base.EnableHover(hand);

			if (image != null)
			{
				image.color = hoverColor;
				if (hoverSprite != null)
					image.sprite = hoverSprite;
			}
			if (meshRenderer != null && hoverMat != null)
				meshRenderer.material = hoverMat;
			if (text != null) text.color = hoverColor;
			if (useToggleableObjects)
			{
				if (hoverObject != null) hoverObject.SetActive(true);
				if (normalObject != null) normalObject.SetActive(false);
				if (clickedObject != null) clickedObject.SetActive(false);
				if (lockedObject != null) lockedObject.SetActive(false);
			}
		}

		override public void DisableHover(VRInteractor hand = null)
		{
			if (locked || !activeHover || interactionDisabled) return;

			base.DisableHover(hand);

			if (image != null)
			{
				image.color = normalColor;
				if (normalSprite != null)
					image.sprite = normalSprite;
			}
			if (meshRenderer != null && normalMat != null)
				meshRenderer.material = normalMat;
			if (text != null) text.color = normalColor;
			if (useToggleableObjects)
			{
				if (normalObject != null) normalObject.SetActive(true);
				if (hoverObject != null) hoverObject.SetActive(false);
				if (clickedObject != null) clickedObject.SetActive(false);
				if (lockedObject != null) lockedObject.SetActive(false);
			}
		}

		override public bool Pickup(VRInteractor hand)
		{
			if (locked) return false;
			base.Pickup(hand);

			if (image != null)
			{
				image.color = clickedColor;
				if (clickedSprite != null)
					image.sprite = clickedSprite;
			}
			if (meshRenderer != null && clickedMat != null)
				meshRenderer.material = clickedMat;
			if (text != null) text.color = clickedColor;
			if (useToggleableObjects)
			{
				if (clickedObject != null) clickedObject.SetActive(true);
				if (normalObject != null) normalObject.SetActive(false);
				if (hoverObject != null) hoverObject.SetActive(false);
				if (lockedObject != null) lockedObject.SetActive(false);
			}

			if (whenToActivate == WhenToActivate.ACTIVATE_ON_CLICK) Activate(hand);
			return true;
		}

		override public void Drop(bool addControllerVelocity, VRInteractor hand = null)
		{
			if (locked) return;
			base.Drop(addControllerVelocity, hand);
			activeHover = true;
			DisableHover(hand);
			if (whenToActivate == WhenToActivate.ACTIVATE_ON_RELEASE) Activate(hand);
		}

		virtual public void Activate(VRInteractor hand)
		{}
	}
}
#endif