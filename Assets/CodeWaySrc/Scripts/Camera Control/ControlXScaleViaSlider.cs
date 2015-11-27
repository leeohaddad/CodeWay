/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlXScaleViaSlider : MonoBehaviour
{
	public Transform objectTransform;
	public Slider slider;
	public float minXScale;
	public float maxXScale;

	void Start ()
	{
		if (objectTransform == null)
			Debug.LogError("ControlXScaleViaSlider.Start() - ObjectTransform is null for GameObject " + this.gameObject.name + ".");
		if (slider == null)
		{
			slider = this.gameObject.GetComponent<Slider>();
			if (slider == null)
				Debug.LogError("ControlXScaleViaSlider.Start() - Slider is null for GameObject " + this.gameObject.name + ".");
		}
		slider.normalizedValue = XScaleToNormalizedValue(objectTransform.localScale.x);
	}

	void Update ()
	{
		float x = NormalizedValueToXScale(slider.normalizedValue);
		float y = objectTransform.localScale.y;
		float z = objectTransform.localScale.z;
		objectTransform.localScale = new Vector3(x,y,z);
	}

	private float XScaleToNormalizedValue (float xScale)
	{
		if (xScale < minXScale)
			xScale = minXScale;
		if (xScale > maxXScale)
			xScale = maxXScale;
		return ((xScale - minXScale) / (maxXScale - minXScale));
	}

	private float NormalizedValueToXScale (float normalizedValue)
	{
		return (minXScale + normalizedValue * (maxXScale - minXScale));
	}
}
