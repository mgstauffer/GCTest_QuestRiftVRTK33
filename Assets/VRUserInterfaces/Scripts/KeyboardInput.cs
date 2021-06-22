//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// This script is a central keyboard handler. It can be setup so all of the buttons
// on the keyboard call a corrosponding method like for instance KeyPressed with the key
// as the data string. Then you can have a script like VRKeyboardTextBox that can listen
// for the event handlers and interpret the input.
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRUserInterfaces
{
	public delegate void VRKeyboardEventHandler(object sender, string data);

	public class KeyboardInput : MonoBehaviour {

		public VRKeyboardEventHandler keyPressed;
		public VRKeyboardEventHandler backSpacePressed;
		public VRKeyboardEventHandler enterPressed;

		public void KeyPressed(string data)
		{
			if (keyPressed != null)
				keyPressed(this, data);
		}

		public void Backspace(string data)
		{
			if (backSpacePressed != null)
				backSpacePressed(this, data);
		}

		public void Enter(string data)
		{
			if (enterPressed != null)
				enterPressed(this, data);
		}
	}
}
#endif