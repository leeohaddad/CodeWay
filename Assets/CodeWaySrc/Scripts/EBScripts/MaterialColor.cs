/*
   Copyright © 2015 EvoBooks
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaterialColor : MonoBehaviour
{
	public List<MaterialColorSetup> materialColorSetups;

	void Start ()
	{
		foreach (MaterialColorSetup mcs in materialColorSetups)
		{
			mcs.highlightable.materials[mcs.materialIndex].color = mcs.color;
		}
	}
}

[System.Serializable]
public class MaterialColorSetup
{
	public Renderer highlightable;
	public int materialIndex = 0;
	public Color color = Color.blue;
}
