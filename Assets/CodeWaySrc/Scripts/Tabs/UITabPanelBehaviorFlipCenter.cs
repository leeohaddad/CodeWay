/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

// Makes panels change with animation that flips the current panel though the middle, showing the new panel in the old panel's back side.
public class UITabPanelBehaviorFlipCenter : UITabPanelBehavior
{
	public enum axis
	{
		x, y, z
	}
	public axis rotationAxisEnum = axis.x;
	public float animationTime = 0.5f;
	public bool addAlphaTweenEffect = false;
	private Vector3 rotationAxis;
	private bool readyToVacate = false;
	
	private bool finishedRotationAnimation = true;

	private GameObject GetCanvas (GameObject thisObject)
	{
		Transform thisTransform = (Transform)thisObject.gameObject.GetComponent<RectTransform>();
		while (thisTransform.parent) thisTransform = thisTransform.parent;
		return thisTransform.gameObject;
	}

	float GetRTAngleByAxis (RectTransform thisRectTransform, Vector3 axis)
	{
		if (axis == Vector3.right || axis == Vector3.left) // Does this work? Is right the same axis as left or is it inverted?
			return thisRectTransform.localRotation.eulerAngles.x;
		if  (axis == Vector3.up || axis == Vector3.down)
			return thisRectTransform.localRotation.eulerAngles.y;
		if (axis == Vector3.forward || axis == Vector3.back)
			return thisRectTransform.localRotation.eulerAngles.z;
		return -1;
	}

	float DistanceBetweenAngles (float finalAngle, float initialAngle)
	{
		while (finalAngle < initialAngle) finalAngle += 360;
		return finalAngle - initialAngle;
	}
	
	// Rotation animation responsible for swapping the panels.
	IEnumerator PlayRotationAnimation (RectTransform thisRectTransform, Vector3 axis, float finalRotation, float duration, Vector3 rotationPivot)
	{
		if (!tabManager.busyWithPanelAnimation) tabManager.busyWithPanelAnimation = true;
		else readyToVacate = true;
		finishedRotationAnimation = false;
		float initialRotation = 0.0f, stepDuration = 0.1f;
		// Get Current Rotation
		initialRotation = GetRTAngleByAxis(thisRectTransform, axis);
		Debug.Log("PlayRotationAnimation(" + thisRectTransform.name + ": " + initialRotation + " -> " + finalRotation + ")");
		// Execute Animation
		for (float currentDuration=0; currentDuration<duration; currentDuration+=stepDuration)
		{
			float progress = currentDuration/duration;
			float easeProgress = (Mathf.Sin((-90f + 180f*progress)*Mathf.Deg2Rad)+1f)*0.5f;
			float rotationAngle = initialRotation + easeProgress * DistanceBetweenAngles(finalRotation, initialRotation);
			thisRectTransform.localRotation = Quaternion.identity;
			thisRectTransform.RotateAround(rotationPivot, axis, rotationAngle);
			yield return new WaitForSeconds(stepDuration);
		}
		thisRectTransform.localRotation = Quaternion.identity;
		thisRectTransform.RotateAround(rotationPivot, axis, finalRotation);
		finishedRotationAnimation = true;
		if (readyToVacate) 
		{
			tabManager.busyWithPanelAnimation = false;
			readyToVacate = false;
		}
	}
	
	// Alpha animation responsible for smoothing the panels swap animation.
	IEnumerator PlayAlphaAnimation (CanvasGroup thisCanvasGroup, float finalAlpha, float duration)
	{
		float initialAlpha = 0.0f, stepDuration = 0.1f;
		// Get Current Alpha
		initialAlpha = thisCanvasGroup.alpha;
		Debug.Log("PlayAlphaAnimation(" + thisCanvasGroup.name + ": " + initialAlpha + " -> " + finalAlpha + ")");
		// Execute Animation
		for (float currentDuration=0; currentDuration<duration; currentDuration+=stepDuration)
		{
			float progress = currentDuration/duration;
			float easeProgress = (Mathf.Sin((-90f + 180f*progress)*Mathf.Deg2Rad)+1f)*0.5f;
			float currentAlpha = initialAlpha + easeProgress * (finalAlpha - initialAlpha);
			thisCanvasGroup.alpha = currentAlpha;
			yield return new WaitForSeconds(stepDuration);
		}
		thisCanvasGroup.alpha = finalAlpha;
	}
	IEnumerator PlayAlphaAnimationAfterDelay (CanvasGroup thisCanvasGroup, float finalAlpha, float duration, float delay)
	{
		yield return new WaitForSeconds(delay);
		StartCoroutine(PlayAlphaAnimation(thisCanvasGroup, finalAlpha, duration));
	}
	
	IEnumerator ShowAndRotatePanelWhenPossible (RectTransform thisRectTransform, Vector3 axis, float finalRotation, float duration, Vector3 rotationPivot)
	{
		Debug.Log("ShowAndRotatePanelWhenPossible()");
		thisRectTransform.gameObject.SetActive(false);
		while (!finishedRotationAnimation) yield return null;
		float flipDelta = GetPanelFlipDelta(thisRectTransform, axis, rotationPivot);
		thisRectTransform.gameObject.SetActive(false);
		thisRectTransform.localRotation = Quaternion.identity;
		thisRectTransform.RotateAround(rotationPivot, axis, 270f + flipDelta);
		thisRectTransform.gameObject.SetActive(true);
		StartCoroutine(PlayRotationAnimation(thisRectTransform, axis, finalRotation, duration, rotationPivot));
		//tabManager.busyWithPanelAnimation = false;
		if (addAlphaTweenEffect)
		{
			CanvasGroup thisCanvasGroup = thisRectTransform.gameObject.GetComponent<CanvasGroup>();
			thisCanvasGroup.alpha = 0.0f;
			StartCoroutine(PlayAlphaAnimation(thisCanvasGroup, 1.0f, duration/4));
		}
	}
	
	IEnumerator RotatePanelAndHideAfter(RectTransform thisRectTransform, Vector3 axis, float finalRotation, float duration, Vector3 rotationPivot)
	{
		Debug.Log("RotatePanelAndHideAfter()");
		finishedRotationAnimation = false;
		StartCoroutine(PlayRotationAnimation(thisRectTransform, axis, finalRotation, duration, rotationPivot));
		if (addAlphaTweenEffect)
		{
			CanvasGroup thisCanvasGroup = thisRectTransform.gameObject.GetComponent<CanvasGroup>();
			thisCanvasGroup.alpha = 1.0f;
			StartCoroutine(PlayAlphaAnimationAfterDelay(thisCanvasGroup, 0.0f, duration/4, 3*duration/4));
		}
		while (!finishedRotationAnimation) yield return null;
		thisRectTransform.gameObject.SetActive(false);
	}

	float GetPanelFlipDelta (RectTransform panelToFlipRT, Vector3 axis, Vector3 pivotPosition)
	{
		GameObject canvasObject = GetCanvas(panelToFlipRT.gameObject);
		Canvas canvasComponent = canvasObject.GetComponent<Canvas>();
		RectTransform canvasRT = canvasObject.GetComponent<RectTransform>();
		float fieldOfView = canvasComponent.worldCamera.fieldOfView;
		float aspectRatio = (float) Screen.width / Screen.height;
		float flipRatio, flipDelta, panelPositionRelativeToCanvasCenter; //, canvasScaledSize;
		
		if (axis == Vector3.right || axis == Vector3.left)
		{
			flipRatio = fieldOfView;
			//canvasScaledSize = canvasRT.sizeDelta.y * canvasRT.localScale.y;
			//panelPositionRelativeToCanvasCenter = ((pivotPosition.y - canvasRT.position.y) / canvasScaledSize);
			panelPositionRelativeToCanvasCenter = canvasComponent.worldCamera.WorldToScreenPoint(panelToFlipRT.position).y/canvasRT.sizeDelta.y - 0.5f;
			flipDelta = -flipRatio * panelPositionRelativeToCanvasCenter; // Why 0.5f?
		} else if (axis == Vector3.up || axis == Vector3.down)
		{
			flipRatio = fieldOfView * aspectRatio;
			//canvasScaledSize = canvasRT.sizeDelta.x * canvasRT.localScale.x;
			//panelPositionRelativeToCanvasCenter = ((pivotPosition.x - canvasRT.position.x) / canvasScaledSize);
			panelPositionRelativeToCanvasCenter = canvasComponent.worldCamera.WorldToScreenPoint(panelToFlipRT.position).x/canvasRT.sizeDelta.x - 0.5f;
			flipDelta = flipRatio * panelPositionRelativeToCanvasCenter;
		} else flipDelta = 0; // (axis == Vector3.forward || axis == Vector3.back)

		Debug.Log("flipDelta: " + flipDelta);
		return flipDelta;
	}

	public override void AnimatePanelSwitchOn (GameObject panelToAnimate)
	{
		Debug.Log("AnimatePanelSwitchOn()");
		RectTransform panelToAnimateRT = panelToAnimate.GetComponent<RectTransform>();
		Vector3 pivotPositionInWorld = panelToAnimateRT.localToWorldMatrix.MultiplyPoint(panelToAnimateRT.pivot);
		//float flipDelta = GetPanelFlipDelta(panelToAnimateRT, rotationAxis, pivotPositionInWorld);
		panelToAnimate.SetActive(true);
		StartCoroutine(ShowAndRotatePanelWhenPossible(panelToAnimateRT, rotationAxis, 0, animationTime/2, pivotPositionInWorld));
	}
	
	public override void AnimatePanelSwitchOff (GameObject panelToAnimate)
	{
		Debug.Log("AnimatePanelSwitchOff()");
		RectTransform panelToAnimateRT = panelToAnimate.GetComponent<RectTransform>();
		Vector3 pivotPositionInWorld = panelToAnimateRT.localToWorldMatrix.MultiplyPoint(panelToAnimateRT.pivot);
		float flipDelta = GetPanelFlipDelta(panelToAnimateRT, rotationAxis, pivotPositionInWorld);
		StartCoroutine(RotatePanelAndHideAfter(panelToAnimateRT, rotationAxis, 90 + flipDelta, animationTime/2, pivotPositionInWorld));
	}
	
	void Awake ()
	{
		if (rotationAxisEnum == axis.x) rotationAxis = Vector3.right;
		else if (rotationAxisEnum == axis.y) rotationAxis = Vector3.up;
		else if (rotationAxisEnum == axis.z) rotationAxis = Vector3.forward;
		else rotationAxis = Vector3.one;

		if (addAlphaTweenEffect)
		{
			RectTransform[] childrenRT = this.gameObject.GetComponentsInChildren<RectTransform>();
			Transform thisTransform = this.gameObject.transform;
			foreach (RectTransform thisRectTransform in childrenRT)
			{
				CanvasGroup thisCanvasGroup = thisRectTransform.gameObject.GetComponent<CanvasGroup>();
				if (thisRectTransform.parent == thisTransform && !thisCanvasGroup)
				{
					thisRectTransform.gameObject.AddComponent<CanvasGroup>();
					Debug.Log("Added a new CanvasGroup component to " + thisRectTransform.name + "!");
				}
			}
		}
	}
}