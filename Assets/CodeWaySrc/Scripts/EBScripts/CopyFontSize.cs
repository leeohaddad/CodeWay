/*
   Copyright © 2015 EvoBooks
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CopyFontSize : MonoBehaviour 
{
	public RectTransform referenceHeight;
	private Text myText;

	public int maxSize = 0;

	void Start () 
	{
		myText = gameObject.GetComponent<Text>();
		if (referenceHeight == null || myText == null)
		{
			Debug.Log("Improper setup of CopyFontSize on " + gameObject.name);
			this.enabled = false;
		}
	}

	void Update () 
	{
		if (maxSize > 0)
			myText.fontSize = Mathf.Min((int) referenceHeight.rect.height, maxSize);
		else
			myText.fontSize = (int) referenceHeight.rect.height;
	}
}
