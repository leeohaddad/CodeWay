/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

// Makes panels change with animation that grows the new panel in front of the current panel.
public class UITabPanelBehaviorGrow : UITabPanelBehavior
{
	public float animationTime = 0.5f;
	public bool x = true;
	public bool y = true;
	public Vector2 pivotPoint = new Vector2(0.5f, 0.5f);

	// Scale animation responsible for growing the next panel.
	IEnumerator PlayScaleAnimation (RectTransform thisRectTransform, Vector3 axis, float finalScale, float duration)
	{
		tabManager.busyWithPanelAnimation = true;
		float currentScale = 0.0f, stepDuration = 0.1f;
		// Get Current Scale
		if (axis == Vector3.right || axis == Vector3.left)
		{
			currentScale = thisRectTransform.localScale.x;
		} else if  (axis == Vector3.up || axis == Vector3.down)
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
			if (axis == Vector3.right || axis == Vector3.left) thisRectTransform.localScale = new Vector3(currentScale + easeProgress * (finalScale - currentScale), thisRectTransform.localScale.y, thisRectTransform.localScale.z);
			else if (axis == Vector3.up || axis == Vector3.down) thisRectTransform.localScale = new Vector3(thisRectTransform.localScale.x, currentScale + easeProgress * (finalScale - currentScale), thisRectTransform.localScale.z);
			yield return new WaitForSeconds(stepDuration);
		}
		if ((axis == Vector3.right || axis == Vector3.left) && thisRectTransform.localScale.x!=finalScale) thisRectTransform.localScale = new Vector3(finalScale, thisRectTransform.localScale.y, thisRectTransform.localScale.z);
		else if ((axis == Vector3.up || axis == Vector3.down) && thisRectTransform.localScale.y!=finalScale) thisRectTransform.localScale = new Vector3(thisRectTransform.localScale.x, finalScale, thisRectTransform.localScale.z);
		// Hide Child Elements, If Hiding
		if (currentScale > finalScale)
			foreach (RectTransform thatRectTransform in thisRectTransform.GetComponentsInChildren<RectTransform>())
				if (thatRectTransform!=thisRectTransform)
					thatRectTransform.localScale = new Vector3(0,0,0);
		tabManager.busyWithPanelAnimation = false;
	}
	
	public override void AnimatePanelSwitchOn (GameObject panelToAnimate)
	{
		RectTransform panelToAnimateRT = panelToAnimate.GetComponent<RectTransform>();
		panelToAnimateRT.pivot = pivotPoint;
		panelToAnimateRT.SetAsLastSibling();
		float initialScaleX = 0;
		float initialScaleY = 0;
		float finalScaleX = panelToAnimateRT.localScale.x;
		float finalScaleY = panelToAnimateRT.localScale.y;
		if (!x) initialScaleX = finalScaleX;
		if (!y) initialScaleY = finalScaleY;
		panelToAnimateRT.localScale = new Vector3(initialScaleX, initialScaleY, panelToAnimateRT.localScale.z);
		if (x) StartCoroutine(PlayScaleAnimation(panelToAnimateRT, Vector3.right, finalScaleX, animationTime));
		if (y) StartCoroutine(PlayScaleAnimation(panelToAnimateRT, Vector3.up, finalScaleY, animationTime));
		panelToAnimate.SetActive(true);
	}
	
	public override void AnimatePanelSwitchOff (GameObject panelToAnimate)
	{
		//RectTransform panelToAnimateRT = panelToAnimate.GetComponent<RectTransform>();
		panelToAnimate.SetActive(false);
	}
}
