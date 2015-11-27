/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlYPositionScaledViaSlider : MonoBehaviour
{
	public Transform objectTransform;
	public Slider slider;
	public float minY;
	public float maxY;

	void Start ()
	{
		if (objectTransform == null)
			Debug.LogError("ControlYPositionScaledViaSlider.Start() - ObjectTransform is null for GameObject " + this.gameObject.name + ".");
		if (slider == null)
		{
			slider = this.gameObject.GetComponent<Slider>();
			if (slider == null)
				Debug.LogError("ControlYPositionScaledViaSlider.Start() - Slider is null for GameObject " + this.gameObject.name + ".");
		}
		slider.normalizedValue = YPositionToNormalizedValue(objectTransform.localPosition.y);
	}

	void Update ()
	{
		float x = objectTransform.localPosition.x;
		float y = NormalizedValueToYPosition(slider.normalizedValue);
		float z = objectTransform.localPosition.z;
		objectTransform.localPosition = new Vector3(x,y,z);
	}

	private float YPositionToNormalizedValue (float yPosition)
	{
		if (yPosition < minY*objectTransform.localScale.y)
			yPosition = minY*objectTransform.localScale.y;
		if (yPosition > maxY*objectTransform.localScale.y)
			yPosition = maxY*objectTransform.localScale.y;
		return ((yPosition - minY*objectTransform.localScale.y) / (maxY*objectTransform.localScale.y - minY*objectTransform.localScale.y));
	}

	private float NormalizedValueToYPosition (float normalizedValue)
	{
		return (minY*objectTransform.localScale.y + normalizedValue * (maxY*objectTransform.localScale.y - minY*objectTransform.localScale.y));
	}
}
