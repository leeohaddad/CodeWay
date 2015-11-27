/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class UITabManager : MonoBehaviour
{
	public UITabButton[] tabButtons;
	public int activeElementIndex;
	public bool animatePanelOnStart = false;
	[HideInInspector]
	public bool busyWithButtonAnimation = false;
	[HideInInspector]
	public bool busyWithPanelAnimation = false;
	private bool blockedByEvents = false;

	// When some tabButton is clicked, this function is called to hide the old elements and show the elements related to the clicked tabButton.
	public void TabButtonClicked (int tabIndex)
	{
		if (busyWithButtonAnimation || busyWithPanelAnimation) return;
		StartCoroutine(TabButtonClickedAsync(tabIndex));
	}

	private IEnumerator TabButtonClickedAsync (int tabIndex)
	{
		if (tabButtons [activeElementIndex].tabEvents != null)
			tabButtons [activeElementIndex].tabEvents.OnHidePanel ();
		while (blockedByEvents) yield return null;
		tabButtons[activeElementIndex].HidePanel(true);
		activeElementIndex = tabIndex;
		tabButtons[activeElementIndex].ShowPanel(true);
		if (tabButtons [activeElementIndex].tabEvents != null)
			tabButtons [activeElementIndex].tabEvents.OnShowPanel ();
		yield return null;
	}

	// Hide all elements except for the elements related to the active element.
	void Start ()
	{
		int index = 0;
		if (tabButtons.Length == 0 || tabButtons[0]==null)
		{
			Debug.LogError(this.name + ".UITabManager: Are you sure that you registered the tabButtons correctly in the tabButtons array?");
			return;
		}
		foreach (UITabButton thisTabButton in tabButtons)
		{
			thisTabButton.RegisterTabManager(this);
			thisTabButton.RegisterTabIndex(index);
			if (index != activeElementIndex) thisTabButton.HidePanel(false);
			else
			{
				thisTabButton.ShowPanel(animatePanelOnStart);
			}
			index++;
		}
	}
	
	public void BlockByEvents ()
	{
		blockedByEvents = true;
	}
	
	public void FreeByEvents ()
	{
		blockedByEvents = false;
	}
}
