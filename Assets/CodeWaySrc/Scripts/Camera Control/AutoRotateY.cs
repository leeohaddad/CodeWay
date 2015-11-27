/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class AutoRotateY : MonoBehaviour
{
	public Transform thisTransform;
	public float anglePerSecond;
	public float rotationPrecision = 10;
	private float srx=0, sry=0, srz=0;

	//TODO; find out why coroutine timing is wrong!

	void Start ()
	{
		if (thisTransform == null)
			thisTransform = this.gameObject.GetComponent<Transform>();
		srx = thisTransform.localEulerAngles.x;
		sry = thisTransform.localEulerAngles.y;
		srz = thisTransform.localEulerAngles.z;
	}
	
	void OnEnable ()
	{
		thisTransform.localEulerAngles = new Vector3 (srx, sry, srz);
		StartCoroutine(RotateYForever(anglePerSecond/rotationPrecision, 1.0f/rotationPrecision));
	}

	private IEnumerator RotateYForever (float deltaAngleY, float interval)
	{
		while (this.gameObject.activeSelf)
		{
			yield return new WaitForSeconds(interval);
			float rx = thisTransform.localEulerAngles.x;
			float ry = thisTransform.localEulerAngles.y + deltaAngleY;
			float rz = thisTransform.localEulerAngles.z;
			thisTransform.localEulerAngles = new Vector3 (rx, ry, rz);
		}
	}
}
