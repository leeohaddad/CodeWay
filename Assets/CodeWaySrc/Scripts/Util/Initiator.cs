/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Initiator : MonoBehaviour
{
	public List<GameObject> activateOnAwake;
	public List<GameObject> deactivateOnAwake;

	void Awake ()
	{
		if (activateOnAwake != null)
			foreach (GameObject gameObject in activateOnAwake)
				gameObject.SetActive(true);
		if (deactivateOnAwake != null)
			foreach (GameObject gameObject in deactivateOnAwake)
				gameObject.SetActive(false);
	}
}
