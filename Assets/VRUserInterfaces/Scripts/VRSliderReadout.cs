//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
//	Slider readout is an example script to demonstrate how values can be taken from
//	the slider script, you can grab the percentages by the public getters or you
//	can add a listender to the delegate, this gets called while the value is changing.
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRUserInterfaces
{
	public class VRSliderReadout : MonoBehaviour 
	{
		public enum OutputField
		{
			X,Y,Z
		}

		private Text text;
		public VRSlider slider;
		public string prefix;
		public string postfix;
		public OutputField field;

		void Start()
		{
			if (text == null) text = GetComponent<Text>();
			if (text == null) Debug.LogError("Slider Readout requires text reference to output to", gameObject);
			slider.sliderEvent += SliderEvent;
			SliderEvent(slider, new Vector3(slider.XPercent, slider.YPercent, slider.ZPercent));
		}

		void SliderEvent(object sender, Vector3 output)
		{
			string field = "";
			switch(this.field)
			{
			case OutputField.X:
				field = (output.x*100).ToString("N0");
				break;
			case OutputField.Y:
				field = (output.y*100).ToString("N0");
				break;
			case OutputField.Z:
				field = (output.z*100).ToString("N0");
				break;
			}
			text.text = prefix + field + postfix;
		}
	}
}
#endif