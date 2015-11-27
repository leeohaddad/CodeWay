/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlZPositionViaSlider : MonoBehaviour
{
	public Transform objectTransform;
	public Slider slider;
	public float minZ;
	public float maxZ;

	void Start ()
	{
		if (objectTransform == null)
			Debug.LogError("ControlZPositionViaSlider.Start() - ObjectTransform is null for GameObject " + this.gameObject.name + ".");
		if (slider == null)
		{
			slider = this.gameObject.GetComponent<Slider>();
			if (slider == null)
				Debug.LogError("ControlZPositionViaSlider.Start() - Slider is null for GameObject " + this.gameObject.name + ".");
		}
		slider.normalizedValue = ZPositionToNormalizedValue(objectTransform.localPosition.z);
	}

	void Update ()
	{
		float x = objectTransform.localPosition.x;
		float y = objectTransform.localPosition.y;
		float z = NormalizedValueToZPosition(slider.normalizedValue);
		objectTransform.localPosition = new Vector3(x,y,z);
	}

	private float ZPositionToNormalizedValue (float zPosition)
	{
		if (zPosition < minZ)
			zPosition = minZ;
		if (zPosition > maxZ)
			zPosition = maxZ;
		return ((zPosition - minZ) / (maxZ - minZ));
	}

	private float NormalizedValueToZPosition (float normalizedValue)
	{
		return (minZ + normalizedValue * (maxZ - minZ));
	}
}
