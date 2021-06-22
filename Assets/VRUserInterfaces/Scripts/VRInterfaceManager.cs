//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// The SceneManagerExample implements some simple methods that are called in the
// example scene by VRButton's.
// LoadScene is called by the button on the Change Scene menu
// Quit is called by the Exit button in the menu
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRUserInterfaces
{
	public class VRInterfaceManager : MonoBehaviour 
	{
		public void LoadScene(string sceneName)
		{
			SceneManager.LoadScene(sceneName);
		}

		public void Quit()
		{
			#if UNITY_EDITOR
			if (Application.isEditor)
			{
				Debug.Log("Exiting Playmode");
				UnityEditor.EditorApplication.isPlaying = false;
			}
			#else
			Application.Quit();
			#endif
		}
	}
}
#endif