/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

// Defines the behavior of the Panel during switch. This script must be attached to the same object of the related tabManager.
public class UITabPanelBehavior : MonoBehaviour
{
	protected UITabManager tabManager;

	public void setTabManager (UITabManager thisTabManager)
	{
		tabManager = thisTabManager;
	}

	public virtual void AnimatePanelSwitchOn (GameObject panelToAnimate)
	{
		Debug.Log("AnimatePanelSwitchOn(" + panelToAnimate.name + ")");
		panelToAnimate.SetActive(true);
	}

	public virtual void AnimatePanelSwitchOff (GameObject panelToAnimate)
	{
		Debug.Log("AnimatePanelSwitchOff(" + panelToAnimate.name + ")");
		panelToAnimate.SetActive(false);
	}
}
