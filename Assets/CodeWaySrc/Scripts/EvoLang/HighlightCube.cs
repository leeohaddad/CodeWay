/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class HighlightCube : MonoBehaviour
{
	public SourceBlocks sourceBlocks;

	void Start ()
	{
		if (sourceBlocks == null)
			Debug.LogError("HighlightCube.Start() - sourceBlocks is null!");
	}

	void OnMouseOver ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			sourceBlocks.SetSelectedBlock(-1);
		}
	}
}
