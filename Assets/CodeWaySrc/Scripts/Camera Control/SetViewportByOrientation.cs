/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class SetViewportByOrientation : MonoBehaviour
{
	public Camera thisCamera;
	public float portraitLeft=0, portraitTop=0, portraitWidth=1, portraitHeight=1;
	public float landscapeLeft=0, landscapeTop=0, landscapeWidth=1, landscapeHeight=1;
	private int orientation = -1;

	void Start ()
	{
		if (thisCamera == null)
			thisCamera = this.gameObject.GetComponent<Camera>();
	}
	
	void Update ()
	{
		if (Screen.width > Screen.height)
		{
			if (orientation != 0) {
				Rect newRect = new Rect (landscapeLeft, landscapeTop, landscapeWidth, landscapeHeight);
				thisCamera.rect = newRect;
				orientation = 0;
			}
		} else
		{
			if (orientation != 1)
			{
				Rect newRect = new Rect (portraitLeft, portraitTop, portraitWidth, portraitHeight);
				thisCamera.rect = newRect;
				orientation = 1;
			}
		}
	}
}
