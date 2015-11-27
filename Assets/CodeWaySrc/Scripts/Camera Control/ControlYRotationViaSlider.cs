/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlYRotationViaSlider : MonoBehaviour
{
	public Transform objectTransform;
	public Slider slider;
	public float minYRotation = 0.0f;
	public float maxYRotation = 360.0f;

	void Start ()
	{
		if (objectTransform == null)
			Debug.LogError("ControlYRotationViaSlider.Start() - ObjectTransform is null for GameObject " + this.gameObject.name + ".");
		if (slider == null)
		{
			slider = this.gameObject.GetComponent<Slider>();
			if (slider == null)
				Debug.LogError("ControlYRotationViaSlider.Start() - Slider is null for GameObject " + this.gameObject.name + ".");
		}
		slider.normalizedValue = EulerAngleToNormalizedValue(objectTransform.localEulerAngles.y);
	}

	void Update ()
	{
		float x = objectTransform.localEulerAngles.x;
		float y = NormalizedValueToEulerAngle(slider.normalizedValue);
		float z = objectTransform.localEulerAngles.z;
		objectTransform.localEulerAngles = new Vector3(x,y,z);
	}

	private float EulerAngleToNormalizedValue (float objectRotation)
	{
		while (objectRotation < minYRotation && (objectRotation + 360) <= maxYRotation)
			objectRotation = objectRotation + 360;
		while (objectRotation > maxYRotation && (objectRotation - 360) >= minYRotation)
			objectRotation = objectRotation - 360;
		if (objectRotation < minYRotation)
			objectRotation = minYRotation;
		if (objectRotation > maxYRotation)
			objectRotation = maxYRotation;
		return ((objectRotation - minYRotation) / (maxYRotation - minYRotation));
	}

	private float NormalizedValueToEulerAngle (float normalizedValue)
	{
		return (minYRotation + normalizedValue * (maxYRotation - minYRotation));
	}
}
