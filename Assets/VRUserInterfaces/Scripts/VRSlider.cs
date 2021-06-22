//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Inherits from VRPointerReceiver. Implements sliding behaviour between to local
// points. Has getters for percentages as well as a delegate that can be listened to,
// fires while slider is moving.
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRInteraction;

namespace VRUserInterfaces
{
	//Output X, Y, Z between 0 and 1 as a percentage distance between min and max positions
	public delegate void SliderEventHandler(object sender, Vector3 output);

	public class VRSlider : VRPointerReceiver 
	{
		public event SliderEventHandler sliderEvent;

		public bool constrainX;
		public bool constrainY;
		public bool constrainZ;

		public float XMin;
		public float XMax;
		public float YMin;
		public float YMax;
		public float ZMin;
		public float ZMax;

		public float editorArrowSize = 0.3f;

		protected VRPointer pointer;
		protected Vector3 lastPos;
		protected Vector3 localLastPos;

		public float XPercent
		{
			get { return (transform.localPosition.x - XMin) / (XMax-XMin); }
		}
		public float YPercent
		{
			get { return (transform.localPosition.y - YMin) / (YMax-YMin); }
		}
		public float ZPercent
		{
			get { return (transform.localPosition.z - ZMin) / (ZMax-ZMin); }
		}

		virtual protected void FixedUpdate()
		{
			if (heldBy == null || transform.parent == null) return;

			Vector3 localDiff = Vector3.zero;
			if (pointer != null && pointer.isPointerReceiver)
			{
				localDiff = transform.parent.InverseTransformPoint(pointer.lastHit.point) - localLastPos;
				localLastPos =  transform.parent.InverseTransformPoint(pointer.lastHit.point);
			} else
			{
				localDiff = transform.parent.InverseTransformPoint(heldBy.getControllerAnchorOffset.position) - localLastPos;
				localLastPos = transform.parent.InverseTransformPoint(heldBy.getControllerAnchorOffset.position);
			}

			Vector3 newPosition = transform.localPosition;

			newPosition.x = VRUtils.ClosestPointOnLine(
				new Vector3((constrainX ? transform.localPosition.x : XMax), 0, 0),
				new Vector3((constrainX ? transform.localPosition.x : XMin), 0, 0),
				transform.localPosition + localDiff).x;

			newPosition.y = VRUtils.ClosestPointOnLine(
				new Vector3(0, (constrainY ? transform.localPosition.y : YMax), 0),
				new Vector3(0, (constrainY ? transform.localPosition.y : YMin), 0),
				transform.localPosition + localDiff).y;

			newPosition.z = VRUtils.ClosestPointOnLine(
				new Vector3(0, 0, (constrainZ ? transform.localPosition.z : ZMax)),
				new Vector3(0, 0, (constrainZ ? transform.localPosition.z : ZMin)),
				transform.localPosition + localDiff).z;

			transform.localPosition = newPosition;

			if (sliderEvent != null) sliderEvent(this, new Vector3(XPercent,YPercent,ZPercent));
		}

		override public void EnableHover(VRInteractor hand)
		{
			if (locked || transform.parent == null) return;
			base.EnableHover(hand);
			pointer = null;
		}

		override public bool Pickup(VRInteractor hand)
		{
			if (locked || transform.parent == null) return false;
			base.Pickup(hand);
			if (hand != null) pointer = hand.GetComponent<VRPointer>();
			if (pointer != null && pointer.isPointerReceiver)
				localLastPos =  transform.parent.InverseTransformPoint(pointer.lastHit.point);
			else if (hand != null)
				localLastPos = transform.parent.InverseTransformPoint(hand.getControllerAnchorOffset.position);

			if (sliderEvent != null) sliderEvent(this, new Vector3(XPercent,YPercent,ZPercent));
			return true;
		}

		override public void Drop(bool addControllerVelocity, VRInteractor hand = null)
		{
			if (locked || transform.parent == null) return;
			if (sliderEvent != null) sliderEvent(this, new Vector3(XPercent,YPercent,ZPercent));
			base.Drop(addControllerVelocity, hand);
			pointer = null;
		}
	}
}
#endif