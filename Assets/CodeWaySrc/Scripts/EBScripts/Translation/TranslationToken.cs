/*
   Copyright © 2015 EvoBooks
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TranslationToken : MonoBehaviour
{

	public Text targetText;
	public string token = "";
	public string tagFileName = "";
	public bool autoUpdate = false;

	void Awake () 
	{
		if (targetText == null)
			targetText = gameObject.GetComponent<Text>();
		TranslationManager.RegisterToken(this);
		UpdateText();
	}
	
	void Update ()
	{
		if (autoUpdate)
		{
			UpdateText();
		}
	}

	protected virtual string GetText ()
	{
		string tag;
		if (token == "")
			tag = "";
		else
		{
			if (tagFileName == "")
				tag = TranslationManager.GetMessage(token);
			else 
				tag = TranslationManager.GetMessage(tagFileName, token);
		}

		return tag;
	}
	
	public void UpdateText () 
	{
		string tag = GetText();

		targetText.text = tag;
	}

	public void SetToken (string token)
	{
		this.token = token;
		UpdateText();
	}

	public void Clear ()
	{
		this.token = "";
		UpdateText();
	}
}
