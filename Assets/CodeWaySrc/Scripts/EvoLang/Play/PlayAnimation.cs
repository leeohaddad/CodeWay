/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayAnimation : MonoBehaviour
{
	[System.Serializable]
	public struct AnimationData
	{
		public string name;
		public Animator animator;
	}
	public List<AnimationData> animationsList;
	public Dictionary<string,Animator> animations;

	void Awake ()
	{
		animations = new Dictionary<string, Animator>();
		foreach (AnimationData data in animationsList)
		{
			animations.Add(data.name,data.animator);
		}
	}

	void Start() {
		//StartCoroutine(PlaySpecificAnimationConstantly("CubeMovementForwardAnimation"));
	}

	public IEnumerator PlaySpecificAnimationConstantly (string name)
	{
		yield return new WaitForSeconds(1.5f);
		PlaySpecificAnimation(name);
		Debug.Log("Played!");
		yield return new WaitForSeconds(1.5f);
		PlaySpecificAnimation(name);
		Debug.Log("Played!");
		yield return new WaitForSeconds(1.5f);
		PlaySpecificAnimation(name);
		Debug.Log("Played!");
		yield return new WaitForSeconds(1.5f);
		PlaySpecificAnimation(name);
		Debug.Log("Played!");
	}
	
	public void PlaySpecificAnimation (string name)
	{
		Debug.Log("PlaySpecificAnimation(" + name + ")");
		if (animations.ContainsKey(name))
		{
			if (animations[name] != null)
			{
				Debug.Log("Playing " + name + " animation!");
				animations[name].Play(name);
			}
			else Debug.Log("Animation '" + name + "' is null!");
		}
		else Debug.Log("Animation '" + name + "' not found!");
	}
}
