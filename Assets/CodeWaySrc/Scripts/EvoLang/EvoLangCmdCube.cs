/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class EvoLangCmdCube : MonoBehaviour
{
	public SourceBlocks sourceBlocks;
	public InsertBlock insertBefore;
	public InsertBlock insertAfter;
	private int index = -1;

	void Start ()
	{
		if (sourceBlocks == null)
			Debug.LogError ("CmdCube.Start() - sourceBlocks is null!");
	}

	void OnMouseOver ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			sourceBlocks.SetSelectedBlock(index);
		}
	}
	
	public void SetIndex (int pIndex)
	{
		index = pIndex;
		insertBefore.index = pIndex;
		insertAfter.index = pIndex + 1;
	}
}
