/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class ControlGUITextureOrientation : MonoBehaviour
{
	public GUITexture thisGUITexture;
	public Texture portrait;
	public Texture landscape;

	void Start ()
	{
		if (thisGUITexture == null)
			thisGUITexture = this.gameObject.GetComponent<GUITexture>();
		if (thisGUITexture == null)
		{
			Debug.LogError("ControlGUITextureOrientation.Start() - thisGUITexture is null for object " + this.gameObject.name + ".");
			this.gameObject.SetActive(false);
		}
		if (portrait == null)
			portrait = landscape;
		if (landscape == null)
		{
			if (portrait == null)
			{
				Debug.LogError("ControlGUITextureOrientation.Start() - portrait & landscape are null for object " + this.gameObject.name + ".");
				this.gameObject.SetActive(false);
			}
			else
				landscape = portrait;
		}
	}

	void Update ()
	{
		if (Screen.width > Screen.height)
		{
			if (thisGUITexture.texture == portrait)
				thisGUITexture.texture = landscape;
		}
		else			
		{
			if (thisGUITexture.texture == landscape)
				thisGUITexture.texture = portrait;
		}
	}
}
