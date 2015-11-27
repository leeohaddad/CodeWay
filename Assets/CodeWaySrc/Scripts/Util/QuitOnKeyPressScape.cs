/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class QuitOnKeyPressScape : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
			QuitApp(); 
	}

	public void QuitApp ()
	{
		Application.Quit();
	}
}
