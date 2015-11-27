/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class UITabEvents : MonoBehaviour
{
	public string onPanelShowMethodId;
	public string onPanelHideMethodId;
	public RectTransform relatedRectTransform;
	private UITabButton tabButton;

	public void OnShowPanel ()
	{
	}

	public void OnHidePanel ()
	{
	}
	
	void Start ()
	{
		// tabButton = this.GetComponent<UITabButton>();
	}
}
