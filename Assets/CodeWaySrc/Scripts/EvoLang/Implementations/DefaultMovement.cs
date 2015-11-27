/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class DefaultMovement : EvoLangImplementation
{	
	private IEnumerator Move_Coroutine_X (Transform transform, float distance, float time)
	{
		//Debug.Log("Move_Coroutine_X()\nTransform: " + transform.gameObject.name + "\nDistance: " + distance + "\nTime: " + time);
		float x, y, z, step, interval;
		float targetX = transform.localPosition.x + distance;
		float timer = 0;
		while (timer < time)
		{
			yield return null;
			interval = Time.deltaTime;
			step = distance / (time / interval);
			timer += interval;
			x = transform.localPosition.x + step;
			y = transform.localPosition.y;
			z = transform.localPosition.z;
			transform.localPosition = new Vector3(x,y,z);
		}
		x = targetX;
		y = transform.localPosition.y;
		z = transform.localPosition.z;
		transform.localPosition = new Vector3(x,y,z);
	}

	private IEnumerator Move_Coroutine_Z (Transform transform, float distance, float time)
	{
		//Debug.Log("Move_Coroutine_Z()\nTransform: " + transform.gameObject.name + "\nDistance: " + distance + "\nTime: " + time);
		float x, y, z, step, interval;
		float targetZ = transform.localPosition.z + distance;
		float timer = 0;
		while (timer < time)
		{
			yield return null;
			interval = Time.deltaTime;
			step = distance / (time / interval);
			timer += interval;
			x = transform.localPosition.x;
			y = transform.localPosition.y;
			z = transform.localPosition.z + step;
			transform.localPosition = new Vector3(x,y,z);
		}
		x = transform.localPosition.x;
		y = transform.localPosition.y;
		z = targetZ;
		transform.localPosition = new Vector3(x,y,z);
	}
	
	private IEnumerator Rotate_Coroutine_Y (Transform transform, float angle, float time)
	{
		//Debug.Log("Rotate_Coroutine_Y()\nTransform: " + transform.gameObject.name + "\nAngle: " + angle + "\nTime: " + time);
		float x, y, z, step, interval;
		float targetY = transform.localEulerAngles.y + angle;
		float timer = 0;
		while (timer < time)
		{
			yield return null;
			interval = Time.deltaTime;
			step = angle / (time / interval);
			timer += interval;
			x = transform.localEulerAngles.x;
			y = transform.localEulerAngles.y + step;
			z = transform.localEulerAngles.z;
			transform.localEulerAngles = new Vector3(x,y,z);
		}
		x = transform.localEulerAngles.x;
		y = targetY;
		z = transform.localEulerAngles.z;
		transform.localEulerAngles = new Vector3(x,y,z);
	}

	public override void execute_command (string command)
	{
		float time = 1.0f / PlaySpace.GetVelocity();
		string cmd = command.ToLower();
		if (cmd.Equals ("walk"))
		{
			float xDistance = Mathf.Sin (myTransform.parent.localEulerAngles.y * Mathf.PI / 180.0f) * PlaySpace.GetBaseDistance ();
			float zDistance = Mathf.Cos (myTransform.parent.localEulerAngles.y * Mathf.PI / 180.0f) * PlaySpace.GetBaseDistance ();
			StartCoroutine (Move_Coroutine_X (myTransform.parent, xDistance, time));
			StartCoroutine (Move_Coroutine_Z (myTransform.parent, zDistance, time));
		} else if (cmd.Equals ("run"))
		{
			float xDistance = Mathf.Sin (myTransform.parent.localEulerAngles.y * Mathf.PI / 180.0f) * 3*PlaySpace.GetBaseDistance ();
			float zDistance = Mathf.Cos (myTransform.parent.localEulerAngles.y * Mathf.PI / 180.0f) * 3*PlaySpace.GetBaseDistance ();
			StartCoroutine (Move_Coroutine_X (myTransform.parent, xDistance, time));
			StartCoroutine (Move_Coroutine_Z (myTransform.parent, zDistance, time));
		}
		else if (cmd.Equals("turnright"))
			StartCoroutine(Rotate_Coroutine_Y(myTransform.parent, 90, time));
		else if (cmd.Equals("turnleft"))
			StartCoroutine(Rotate_Coroutine_Y(myTransform.parent, -90, time));
		else Debug.Log("Could not run cmd " + cmd + ".");
		execute_animations(cmd, time);
	}
}
