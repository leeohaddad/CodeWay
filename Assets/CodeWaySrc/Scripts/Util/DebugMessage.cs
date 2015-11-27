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

public class DebugMessage : MonoBehaviour
{
	private static float rS=000f/255, gS=255f/255, bS=000f/255, aS=255f/255;
	private static float rW=255f/255, gW=255f/255, bW=000f/255, aW=255f/255;
	private static float rE=255f/255, gE=160f/255, bE=000f/255, aE=255f/255;
	private static List<DebugMessage> instances;
	public Text textComponent;
	public Image panelImageComponent;
	private int panelId;

	void Start ()
	{
		if (textComponent == null)
			textComponent = this.gameObject.GetComponentInChildren<Text>();
		if (panelImageComponent == null)
			panelImageComponent = this.gameObject.GetComponent<Image>();
		if (instances == null)
			instances = new List<DebugMessage>();
		panelId = instances.Count;
		instances.Add(this);
		this.gameObject.SetActive(false);
	}

	private static void Message (int thisPanelId, string message, Color thisColor)
	{
		if (instances == null)
		{
			Debug.LogError("DebugMessage.LogError() - Failed because instances var is null.");
			Debug.LogError("DebugMessage.LogError() - Your DebugMessage GameObjects must start active.");
			return;
		}
		if (instances.Count <= thisPanelId)
		{
			Debug.LogError("DebugMessage.LogError() - Failed to load instance #" + thisPanelId + " (" + (thisPanelId+1)  + ") because only " + instances.Count + " instances were found.");
			Debug.LogError("DebugMessage.LogError() - Your DebugMessage GameObjects must start active.");
			return;
		}
		instances[thisPanelId].gameObject.SetActive(false);
		if (instances [thisPanelId].panelImageComponent != null)
			instances [thisPanelId].panelImageComponent.color = thisColor;
		instances[thisPanelId].textComponent.text = message;
		instances[thisPanelId].gameObject.SetActive(true);
	}
	
	public static void SuccessMessage (int thisPanelId, string thisMessage)
	{
		Message(thisPanelId, thisMessage, new Color(rS,gS,bS,aS));
	}
	
	public static void WarningMessage (int thisPanelId, string thisMessage)
	{
		Message(thisPanelId, thisMessage, new Color(rW,gW,bW,aW));
	}
	
	public static void ErrorMessage (int thisPanelId, string thisMessage)
	{
		Message(thisPanelId, thisMessage, new Color(rE,gE,bE,aE));
	}
	
	public static void SuccessMessage (string thisMessage)
	{
		SuccessMessage(0,thisMessage);
	}
	
	public static void WarningMessage (string thisMessage)
	{
		WarningMessage(0,thisMessage);
	}
	
	public static void ErrorMessage (string thisMessage)
	{
		ErrorMessage(0,thisMessage);
	}
}
