/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class InsertBlock : MonoBehaviour
{
	public SourceBlocks sourceBlocks;
	public int index = -1;
	public bool isStarter = false;

	void Start ()
	{
		if (sourceBlocks == null)
			Debug.LogError("InsertBlock.Start() - sourceBlocks is null!");
	}

	void OnMouseOver ()
	{
		if (isStarter)
		{
			if (sourceBlocks.GetBlocksCount() > 0)
				return;
		}
		if (Input.GetMouseButtonUp(0))
		{
			string cmdToAdd = sourceBlocks.GetCommandToAdd();
			if (cmdToAdd.Length > 0)
			{
				Debug.Log("Index: " + index + " ; sourceBlocks: " + cmdToAdd);
				sourceBlocks.AddCommandBlock(index,cmdToAdd);
			}
		}
	}
}
