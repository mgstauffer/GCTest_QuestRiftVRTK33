//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// This script is an intepreter, it listens to the KeyboardInput script and adds
// any keys pressed to it's referenced text.
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace VRUserInterfaces
{
	public class VRKeyboardTextBox : MonoBehaviour {

		public Text textBox;
		public KeyboardInput input;

		public UnityEvent unityEvent;

		public KeyboardInput set_input
		{
			set 
			{
				input = value;
				Init();
			}
		}

		void OnEnable() 
		{
			Init();
		}

		void OnDisable()
		{
			if (input == null) return;
			input.keyPressed -= KeyInputPressed;
			input.backSpacePressed -= BackspacePressed;
			input.enterPressed -= EnterPressed;
		}

		void Init()
		{
			if (input == null) return;
			input.keyPressed += KeyInputPressed;
			input.backSpacePressed += BackspacePressed;
			input.enterPressed += EnterPressed;
			if (textBox == null) textBox = GetComponent<Text>();
		}

		void KeyInputPressed(object sender, string data)
		{
			if (textBox == null)
			{
				Debug.LogError("No TextBox found");
				return;
			}
			textBox.text += data;
		}

		void BackspacePressed(object sender, string data)
		{
			if (textBox == null)
			{
				Debug.LogError("No TextBox found");
				return;
			}
			if (textBox.text.Length == 0) return;
			textBox.text = textBox.text.Remove(textBox.text.Length-1);
		}

		void EnterPressed(object sender, string data)
		{
			if (unityEvent != null) unityEvent.Invoke();
		}
	}
}
#endif