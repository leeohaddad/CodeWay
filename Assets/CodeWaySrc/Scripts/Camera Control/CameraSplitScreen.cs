/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraSplitScreen : MonoBehaviour
{
	private enum Orientation
	{
		Null,
		Portrait,
		Landscape
	};
	[System.Serializable]
	public struct SplitCamera
	{
		public string name;
		public Camera cam;
		public int importance;
	};
	public List<SplitCamera> splitCameras;
	private int regionsCount = 0;
	private float cameraDelta = 0;
	private Orientation currentOrientation = Orientation.Null;
	
	void Start () {
		foreach (SplitCamera splitCamera in splitCameras)
		{
			int importance = splitCamera.importance;
			if (importance == 0)
				importance = 1;
			regionsCount += importance;
		}
		cameraDelta = 1.0f / regionsCount;
	}
	
	void Update () {
		if (Screen.width > Screen.height)
		{
			SetOrientation(Orientation.Landscape);
		}
		else if (Screen.width < Screen.height)
		{
			SetOrientation(Orientation.Portrait);
		}
	}

	private void SetOrientation (Orientation newOrientation)
	{
		if (newOrientation == currentOrientation)
			return;
		float left = 0, top = 0, width = 1, height = 1;
		int index=0;
		if (newOrientation == Orientation.Landscape)
		{
			width = cameraDelta;
			foreach (SplitCamera splitCamera in splitCameras)
			{
				Rect newRect = new Rect(left,top,width*Mathf.Max(1,splitCamera.importance),height);
				splitCameras[index].cam.rect = newRect;
				left += width * Mathf.Max(1,splitCamera.importance);
				index++;
			}
		}
		else if (newOrientation == Orientation.Portrait)
		{
			height = cameraDelta;
			foreach (SplitCamera splitCamera in splitCameras)
			{
				Rect newRect = new Rect(left,top,width,height*Mathf.Max(1,splitCamera.importance));
				splitCameras[index].cam.rect = newRect;
				top += height * Mathf.Max(1,splitCamera.importance);
				index++;
			}
		}
		currentOrientation = newOrientation;
	}
}
