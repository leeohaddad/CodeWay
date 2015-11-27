/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UITabButton : MonoBehaviour, IPointerClickHandler
{
	[Tooltip("Panel to show when button is clicked.")]
	public GameObject correspondingPanel;
	private int tabIndex;
	public UITabEvents tabEvents;
	protected UITabManager tabManager;
	protected Image buttonImage;
	protected float buttonAlpha;
	private UITabPanelBehavior panelSwitchBehavior;

	// UITabManager uses this function to register itself in it's tabButtons. Function also looks for the panelSwitchBehavior.
	public void RegisterTabManager (UITabManager thisTabManager)
	{
		tabManager = thisTabManager;

		if (!panelSwitchBehavior)
		{
			UITabPanelBehavior thisPanelBehavior = tabManager.gameObject.GetComponent<UITabPanelBehavior>();
			if (thisPanelBehavior)
			{
				panelSwitchBehavior = thisPanelBehavior;
				panelSwitchBehavior.setTabManager(tabManager);
			}
		}
	}

	// UITabManager uses this function to register the index of this tabButton in the tabButtons array.
	public void RegisterTabIndex (int newIndex)
	{
		tabIndex = newIndex;
	}

	// Animates the button to set it as selected.
	public virtual void AnimateButtonOn ()
	{
		buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, buttonAlpha/4);
	}

	// Sets the corresponding panel as the current active panel.
	public void ShowPanel (bool animatePanel)
	{
		if (panelSwitchBehavior && animatePanel) panelSwitchBehavior.AnimatePanelSwitchOn(correspondingPanel);
		else correspondingPanel.SetActive(true);
		AnimateButtonOn();
	}
	
	// Animates the button to set it as not selected.
	public virtual void AnimateButtonOff ()
	{
		buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, buttonAlpha);
	}
	
	// Hides the corresponding panel for another panel to become the active panel.
	public void HidePanel (bool animatePanel)
	{
		if (panelSwitchBehavior && animatePanel) panelSwitchBehavior.AnimatePanelSwitchOff(correspondingPanel);
		else correspondingPanel.SetActive(false);
		AnimateButtonOff();
	}

	// Tries to show the corresponding panel elements when some pointer click is detected.
	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		if (tabManager.activeElementIndex != tabIndex) tabManager.TabButtonClicked(tabIndex);
	}

	public UITabManager GetManager ()
	{
		return tabManager;
	}
	
	protected virtual void Awake ()
	{
		buttonImage = this.gameObject.GetComponent<Image>();	
		buttonAlpha = buttonImage.color.a;
		tabEvents = this.GetComponent<UITabEvents>();
	}
}
