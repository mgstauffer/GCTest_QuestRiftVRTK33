//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Inherits from VRPointerReceiver and implements the Activate method to fire
// any event or message. Will also play a connected animation if one is supplied
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRInteraction;

namespace VRUserInterfaces
{
	public class VRButton : VRPointerReceiver
	{
		override public void Activate(VRInteractor hand)
		{
			if (locked) return;

			if (anim != null && onActivateAnimStateName != "")
				anim.Play(onActivateAnimStateName);

			if (useUnityEvent && unityEvent != null)
				unityEvent.Invoke();
			if (useSendMessage && targetObject != null && targetMethod != "")
				targetObject.SendMessage(targetMethod, stringData, SendMessageOptions.DontRequireReceiver);
		}
	}
}
#endif