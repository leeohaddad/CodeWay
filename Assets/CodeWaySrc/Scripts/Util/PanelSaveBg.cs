/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class PanelSaveBg : MonoBehaviour
{
	public static PanelSaveBg instance;
	public Animator myAnimator;

	void Start ()
	{
		if (myAnimator == null)
			myAnimator = this.gameObject.GetComponent<Animator>();
		instance = this;
		this.gameObject.SetActive(false);
	}
	
	public void EnterAnimation ()
	{
		this.gameObject.SetActive(false);
		this.gameObject.SetActive(true);
	}
	
	public static void ExitAnimation ()
	{
		if (instance != null)
			instance.myAnimator.Play("PanelLoadSaveExitAnimation");
		else
			Debug.LogError("PanelLoadBg.ExitAnimation() - instance is null!");
	}
	
	public void Deactivate ()
	{
		this.gameObject.SetActive(false);
	}
}
