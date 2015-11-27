/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class OnEnableResetGameObject : MonoBehaviour
{
	public GameObject gameObjectToRestart;

	void OnEnable ()
	{
		gameObjectToRestart.SetActive(false);
		gameObjectToRestart.SetActive(true);
		StartCoroutine(ResetActiveAfterDelay(0.1f));
	}

	private IEnumerator ResetActiveAfterDelay (float delayInSeconds)
	{
		yield return new WaitForSeconds(delayInSeconds);
		gameObjectToRestart.SetActive(false);
		gameObjectToRestart.SetActive(true);
	}
	
	public void ResetIfActive ()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			this.gameObject.SetActive(true);
		}
	}
}
