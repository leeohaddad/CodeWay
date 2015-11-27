/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlXRotationViaSlider : MonoBehaviour
{
	public Transform objectTransform;
	public Slider slider;
	public float minXRotation = 0.0f;
	public float maxXRotation = 360.0f;

	void Start ()
	{
		if (objectTransform == null)
			Debug.LogError("ControlXRotationViaSlider.Start() - ObjectTransform is null for GameObject " + this.gameObject.name + ".");
		if (slider == null)
		{
			slider = this.gameObject.GetComponent<Slider>();
			if (slider == null)
				Debug.LogError("ControlXRotationViaSlider.Start() - Slider is null for GameObject " + this.gameObject.name + ".");
		}
		slider.normalizedValue = EulerAngleToNormalizedValue(objectTransform.localEulerAngles.x);
	}

	void Update ()
	{
		float x = NormalizedValueToEulerAngle(slider.normalizedValue);
		float y = objectTransform.localEulerAngles.y;
		float z = objectTransform.localEulerAngles.z;
		objectTransform.localEulerAngles = new Vector3(x,y,z);
	}

	private float EulerAngleToNormalizedValue (float objectRotation)
	{
		while (objectRotation < minXRotation && (objectRotation + 360) <= maxXRotation)
			objectRotation = objectRotation + 360;
		while (objectRotation > maxXRotation && (objectRotation - 360) >= minXRotation)
			objectRotation = objectRotation - 360;
		if (objectRotation < minXRotation)
			objectRotation = minXRotation;
		if (objectRotation > maxXRotation)
			objectRotation = maxXRotation;
		return ((objectRotation - minXRotation) / (maxXRotation - minXRotation));
	}

	private float NormalizedValueToEulerAngle (float normalizedValue)
	{
		return (minXRotation + normalizedValue * (maxXRotation - minXRotation));
	}
}
