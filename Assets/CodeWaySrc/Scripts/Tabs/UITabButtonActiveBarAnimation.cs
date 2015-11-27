/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class UITabButtonActiveBarAnimation : UITabButtonActiveBar
{
	public float scaleAnimationDuration = 0.1f;
	public enum Axis
	{
		X,
		Y,
		Z
	}

	// Scale animation responsible for hiding and showing the element when required.
	IEnumerator PlayScaleAnimation (RectTransform thisRectTransform, Axis axis, float finalScale, float duration)
	{
		tabManager.busyWithButtonAnimation = true;
		float currentScale = 0.0f, stepDuration = 0.1f;
		// Get Current Scale
		if (axis == Axis.X)
		{
			currentScale = thisRectTransform.localScale.x;
		} else
			if  (axis == Axis.Y)
		{
			currentScale = thisRectTransform.localScale.y;
		}
		// Show Child Elements Back, If Showing
		if (finalScale > currentScale)
			foreach (RectTransform thatRectTransform in thisRectTransform.GetComponentsInChildren<RectTransform>())
				if (thatRectTransform!=thisRectTransform)
					thatRectTransform.localScale = new Vector3(1,1,1);
		// Ensure At Least 2 Animation Steps
		if (stepDuration > duration/2) stepDuration = duration/2;
		// Calculate Steps
		// float scaleStep = (finalScale-currentScale)/(duration/stepDuration); // Remove Scale Step
		// Execute Animation
		for (float currentDuration=0; currentDuration<duration; currentDuration+=stepDuration)
		{
			float progress = currentDuration/duration;
			float easeProgress = (Mathf.Sin((-90f + 180f*progress)*Mathf.Deg2Rad)+1f)*0.5f;
			if (axis == Axis.X) thisRectTransform.localScale = new Vector3(currentScale + easeProgress * (finalScale - currentScale), thisRectTransform.localScale.y, thisRectTransform.localScale.z);
			else if (axis == Axis.Y) thisRectTransform.localScale = new Vector3(thisRectTransform.localScale.x, currentScale + easeProgress * (finalScale - currentScale), thisRectTransform.localScale.z);
			yield return new WaitForSeconds(stepDuration);
		}
		if (axis == Axis.X && thisRectTransform.localScale.x!=finalScale) thisRectTransform.localScale = new Vector3(finalScale, thisRectTransform.localScale.y, thisRectTransform.localScale.z);
		else if (axis == Axis.Y && thisRectTransform.localScale.y!=finalScale) thisRectTransform.localScale = new Vector3(thisRectTransform.localScale.x, finalScale, thisRectTransform.localScale.z);
		// Hide Child Elements, If Hiding
		if (currentScale > finalScale)
			foreach (RectTransform thatRectTransform in thisRectTransform.GetComponentsInChildren<RectTransform>())
				if (thatRectTransform!=thisRectTransform)
					thatRectTransform.localScale = new Vector3(0,0,0);
		tabManager.busyWithButtonAnimation = false;
	}

	protected override void ScaleElementY (RectTransform thisRectTransform, float newScaleY)
	{
		StartCoroutine(PlayScaleAnimation(thisRectTransform, Axis.Y, newScaleY, scaleAnimationDuration));
	}
	
	protected override void Awake ()
	{
		base.Awake();
		RectTransform thisRectTransform = activeImage.rectTransform;
		thisRectTransform.localScale = new Vector3(thisRectTransform.localScale.x, 0, thisRectTransform.localScale.z);
	}
}
