/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class EvoLangCmd : MonoBehaviour
{
	public string command;
	public Material material;
	public Sprite sprite;
	public Color color = Color.white;
	
	void Awake ()
	{
		if (command.Length == 0)
			command = this.gameObject.name;
		if (material == null)
			Debug.LogError("EvoLangCmg.Awake() - material is null!");
		if (sprite == null)
			Debug.LogError("EvoLangCmg.Awake() - sprite is null!");
	}
}
