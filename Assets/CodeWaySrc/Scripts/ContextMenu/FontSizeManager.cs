/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FontSizeManager : MonoBehaviour
{

	public int defaultFontSize = 14;
	public string PlayerPrefsKey = "FontSize";
	private static List<FontSizeManager> instances = null;

	void Start ()
	{
		if (instances == null)
			instances = new List<FontSizeManager>();
		instances.Add(this);
		if (PlayerPrefsKey.Length > 0)
		{
			int fontSize = PlayerPrefs.GetInt(PlayerPrefsKey);
			if (fontSize > 0)
				SetFontSize(fontSize);
			else
			{
				if (defaultFontSize > 0)
				{
					PlayerPrefs.SetInt(PlayerPrefsKey,defaultFontSize);
					PlayerPrefs.Save();
					SetFontSize(defaultFontSize);
				}
				else
				{
					Text textComponent = this.gameObject.GetComponent<Text>();
					if (textComponent != null)
					{
						PlayerPrefs.SetInt(PlayerPrefsKey,textComponent.fontSize);
						PlayerPrefs.Save();
						SetFontSize(textComponent.fontSize);
					}
					else Debug.LogError("Cannot define font size!");
				}
			}
		}
	}

	public void SetFontSize (int fontSize)
	{
		//Debug.Log("Setting FontSize " + fontSize + " for " + this.gameObject.name);
		Text text = this.gameObject.GetComponent<Text>();
		if (text != null)
			text.fontSize = fontSize;
	}

	public void SetGroupFontSize (int fontSize)
	{
		if (PlayerPrefsKey.Length > 0)
		{
			PlayerPrefs.SetInt(PlayerPrefsKey, fontSize);
			PlayerPrefs.Save();
		}
		Debug.Log("FontSizeManager.SetGroupFontSize() - Instances: " + instances.Count);
		foreach (FontSizeManager instance in instances)
		{
			if (PlayerPrefsKey.Length == 0 || PlayerPrefsKey.Equals(instance.PlayerPrefsKey))
				instance.SetFontSize(fontSize);
		}
	}
}
