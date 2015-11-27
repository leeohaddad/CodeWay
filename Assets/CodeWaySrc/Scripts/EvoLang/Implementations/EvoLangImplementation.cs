/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class EvoLangImplementation : MonoBehaviour
{
	public Transform myTransform;
	public bool hasAnimatorComponent;
	public Animator myAnimatorComponent;
	public bool hasAnimationComponent;
	public Animation myAnimationComponent;
	protected float precision = 10.0f;
	
	void Awake ()
	{
		if (myTransform == null)
			Debug.LogError("EvoLangImplementation.Awake() - myTransform is null!");
		if (hasAnimatorComponent && myAnimatorComponent == null)
			Debug.LogError("EvoLangImplementation.Awake() - myAnimatorComponent is null!");
		if (hasAnimationComponent && myAnimationComponent == null)
			Debug.LogError("EvoLangImplementation.Awake() - hasAnimationComponent is null!");
	}
	public virtual void execute_command (string command)
	{
		Debug.Log("EvoLangImplementation.execute_command()");
		execute_animations(command.ToLower(), 1.0f / PlaySpace.GetVelocity());
	}
	
	protected void execute_animations (string cmd, float time)
	{
		if (hasAnimatorComponent)
		{
			myAnimatorComponent.Play(cmd);
		}
		if (hasAnimationComponent)
		{
			myAnimationComponent.Play(cmd);
		}
		if (time <= 1.0f)
			time = 0.9f;
		else
			time = time - 0.1f;
		StartCoroutine(StopAnimationsAfterDelay(time));
	}
	
	protected IEnumerator StopAnimationsAfterDelay (float delay)
	{
		yield return new WaitForSeconds(delay);
		string idleText = "idle";
		if (hasAnimatorComponent)
		{
			myAnimatorComponent.Play(idleText);
		}
		if (hasAnimationComponent)
		{
			myAnimationComponent.Play(idleText);
		}
	}
}
