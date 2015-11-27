/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class CopyGlobalPosition : MonoBehaviour
{
	private Transform thisObjectTransform = null;
	private Transform targetObjectTransform = null;

	void Start ()
	{
		thisObjectTransform = this.gameObject.GetComponent<Transform>();
	}
	
	void Update ()
	{
		if (targetObjectTransform != null)
		{
			float x = targetObjectTransform.position.x;
			float y = targetObjectTransform.position.y;
			float z = targetObjectTransform.position.z;
			thisObjectTransform.position = new Vector3(x,y,z);
		}
	}

	public void SetTarget (Transform argTargetObjectTransform)
	{
		targetObjectTransform = argTargetObjectTransform;
	}
}
