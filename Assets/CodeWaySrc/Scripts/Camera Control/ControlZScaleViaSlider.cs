/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlZScaleViaSlider : MonoBehaviour
{
	public Transform objectTransform;
	public Slider slider;
	public float minZScale;
	public float maxZScale;

	void Start ()
	{
		if (objectTransform == null)
			Debug.LogError("ControlZScaleViaSlider.Start() - ObjectTransform is null for GameObject " + this.gameObject.name + ".");
		if (slider == null)
		{
			slider = this.gameObject.GetComponent<Slider>();
			if (slider == null)
				Debug.LogError("ControlZScaleViaSlider.Start() - Slider is null for GameObject " + this.gameObject.name + ".");
		}
		slider.normalizedValue = ZScaleToNormalizedValue(objectTransform.localScale.z);
	}

	void Update ()
	{
		float x = objectTransform.localScale.x;
		float y = objectTransform.localScale.y;
		float z = NormalizedValueToXScale(slider.normalizedValue);
		objectTransform.localScale = new Vector3(x,y,z);
	}

	private float ZScaleToNormalizedValue (float zScale)
	{
		if (zScale < minZScale)
			zScale = minZScale;
		if (zScale > maxZScale)
			zScale = maxZScale;
		return ((zScale - minZScale) / (maxZScale - minZScale));
	}

	private float NormalizedValueToXScale (float normalizedValue)
	{
		return (minZScale + normalizedValue * (maxZScale - minZScale));
	}
}
