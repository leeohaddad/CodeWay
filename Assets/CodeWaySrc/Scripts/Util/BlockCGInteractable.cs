/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockCGInteractable : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public List<GameObject> blockers;

	void Update ()
	{
		bool interactable = true;
		if (blockers != null)
		{
			foreach (GameObject blocker in blockers)
			{
				if (blocker.activeSelf)
					interactable = false;
			}
		}
		canvasGroup.interactable = interactable;
	}
}
