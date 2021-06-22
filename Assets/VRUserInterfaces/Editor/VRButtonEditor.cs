//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Editor for VRButton, inherits from VRPointerReceiverEditor
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VRUserInterfaces
{
	[CustomEditor(typeof(VRButton))]
	public class VRButtonEditor : VRPointerReceiverEditor
	{
	}
}
#endif