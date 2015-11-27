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

public class ContextMenuTranslationTokenizer : MonoBehaviour
{

	public string tagFileName;

	void Start ()
	
	{
		if (tagFileName.Trim ().Length == 0)
			Debug.LogError("ContextMenuTranslationTokenizer requires the Tag File Name field.");
		StartCoroutine(CreateTokens());	
	}

	private IEnumerator CreateTokens()
	{
		ContextMenuOnClick cm = this.gameObject.GetComponent<ContextMenuOnClick>();
		if (cm == null)
			Debug.LogError("ContextMenuTranslationTokenizer must be attached to element with ContextMenuOnClick component.");
		while (cm.isInitialized() == false)
			yield return new WaitForSeconds(0.1f);
		List<RectTransform> buttonsRTs = cm.GetContextMenuButtonsRTs();
		Transform parent = buttonsRTs[0].parent;
		bool shouldDeactivateParent = false;
		if (!parent.gameObject.activeSelf)
		{
			parent.gameObject.SetActive(true);
			shouldDeactivateParent = true;
		}
		foreach (RectTransform buttonRT in buttonsRTs)
		{
			bool shouldDeactivateButton = false;
			if (!buttonRT.gameObject.activeSelf)
			{
				buttonRT.gameObject.SetActive(true);
				shouldDeactivateButton = true;
			}
			Text txt = buttonRT.gameObject.GetComponentInChildren<Text>();
			string text = txt.text;
			TranslationToken token = txt.gameObject.AddComponent<TranslationToken>();
			token.targetText = txt;
			token.token = text;
			token.tagFileName = tagFileName;
			txt.text = TranslationManager.GetMessage(tagFileName,text);
			if (shouldDeactivateButton == true)
				buttonRT.gameObject.SetActive (false);
		}
		if (shouldDeactivateParent == true)
			parent.gameObject.SetActive (false);
	}
}
