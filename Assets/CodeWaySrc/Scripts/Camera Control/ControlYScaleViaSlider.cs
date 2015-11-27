/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlYScaleViaSlider : MonoBehaviour
{
	public Transform objectTransform;
	public Slider slider;
	public float minYScale;
	public float maxYScale;

	void Start ()
	{
		if (objectTransform == null)
			Debug.LogError("ControlYScaleViaSlider.Start() - ObjectTransform is null for GameObject " + this.gameObject.name + ".");
		if (slider == null)
		{
			slider = this.gameObject.GetComponent<Slider>();
			if (slider == null)
				Debug.LogError("ControlYScaleViaSlider.Start() - Slider is null for GameObject " + this.gameObject.name + ".");
		}
		slider.normalizedValue = YScaleToNormalizedValue(objectTransform.localScale.y);
	}

	void Update ()
	{
		float x = objectTransform.localScale.x;
		float y = NormalizedValueToYScale(slider.normalizedValue);
		float z = objectTransform.localScale.z;
		objectTransform.localScale = new Vector3(x,y,z);
	}

	private float YScaleToNormalizedValue (float yScale)
	{
		if (yScale < minYScale)
			yScale = minYScale;
		if (yScale > maxYScale)
			yScale = maxYScale;
		return ((yScale - minYScale) / (maxYScale - minYScale));
	}

	private float NormalizedValueToYScale (float normalizedValue)
	{
		return (minYScale + normalizedValue * (maxYScale - minYScale));
	}
}
