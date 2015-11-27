/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITabButtonActiveBar : UITabButton
{
	[Tooltip("Image to show or hide when the corresponding panel is shown or hidden.")]
	public Image activeImage;
	private float imageScaleY;

	protected virtual void ScaleElementY (RectTransform thisRectTransform, float newScaleY)
	{
		thisRectTransform.localScale = new Vector3(thisRectTransform.localScale.x, newScaleY, thisRectTransform.localScale.z);
	}
	
	// Animates the button to set it as selected.
	public override void AnimateButtonOn ()
	{
		ScaleElementY(activeImage.rectTransform, imageScaleY);
	}
	
	// Animates the button to set it as not selected.
	public override void AnimateButtonOff ()
	{
		ScaleElementY(activeImage.rectTransform, 0);
	}

	protected override void Awake ()
	{
		base.Awake();
		buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, (float)3/255);
		activeImage.rectTransform.pivot = new Vector2(activeImage.rectTransform.pivot.x, 0);
		imageScaleY = activeImage.rectTransform.localScale.y;
	}
}