/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstancesManager : MonoBehaviour
{
	private static InstancesManager instance;
	private static List<ProgrammableObject> delayedRegistrations;

	public Transform objectsContainer;
	public CopyGlobalPosition cameraPivot;
	public PlaySpace playSpace;
	private int currentIndex;
	private GameObject currentObject;
	private List<string> gameObjectNames;
	private Dictionary<string,GameObject> gameObjects;
	private Dictionary<string,Transform> cameraTargets;

	void Awake ()
	{
		instance = this;
		if (objectsContainer == null)
			objectsContainer = this.gameObject.GetComponent<Transform>();
		if (cameraPivot == null)
			cameraPivot = this.gameObject.GetComponentInChildren<CopyGlobalPosition>();
		gameObjectNames = new List<string>();
		gameObjects = new Dictionary<string,GameObject>();
		cameraTargets = new Dictionary<string,Transform>();
		if (delayedRegistrations != null)
		{
			foreach (ProgrammableObject programmableObject in delayedRegistrations)
				instance.RegisterProgrammableObject(programmableObject);
		}
		//this.gameObject.SetActive(false);
	}

	private IEnumerator CompleteRegistrationAfterDelay (GameObject instanceGO, GameObject programmableObjectGO, float delayInSeconds)
	{
		yield return new WaitForSeconds(delayInSeconds);
		instanceGO.SetActive(true);
		Transform instanceTransform = programmableObjectGO.GetComponent<Transform>();
		instanceTransform.parent.parent = objectsContainer;
		gameObjects.Add(programmableObjectGO.name,instanceTransform.parent.gameObject);
		gameObjectNames.Add(programmableObjectGO.name);
		instanceTransform.parent.localPosition = Vector3.zero;
		instanceTransform.parent.localEulerAngles = Vector3.zero;
		if (gameObjects.Count == 1)
			Select(0);
		else
			instanceGO.SetActive(false);
	}
	
	public void RegisterProgrammableObject (ProgrammableObject programmableObject)
	{
		Transform thisTransform = programmableObject.gameObject.GetComponent<Transform>();
		GameObject instanceGO = GameObject.Instantiate(thisTransform.parent.gameObject);
		ProgrammableObject instanceProgrammableObject = instanceGO.GetComponentsInChildren<ProgrammableObject>(true)[0];
		if (instanceProgrammableObject.cameraTarget != null)
		{
			if (!cameraTargets.ContainsKey(instanceProgrammableObject.gameObject.name))
				cameraTargets.Add(instanceProgrammableObject.gameObject.name,instanceProgrammableObject.cameraTarget);
			//Debug.Log("Registering CameraTarget for " + instanceProgrammableObject.gameObject.name);
		}
		GameObject programmableObjectGO = instanceProgrammableObject.gameObject;
		Destroy(instanceProgrammableObject);
		StartCoroutine(CompleteRegistrationAfterDelay(instanceGO, programmableObjectGO, 0.1f));
	}
	
	public static void RegisterProgrammableObjectStatic (ProgrammableObject programmableObject)
	{
		if (instance != null)
			instance.RegisterProgrammableObject(programmableObject);
		else
			DelayRegistration(programmableObject);
	}

	private static void DelayRegistration (ProgrammableObject programmableObject)
	{
		if (delayedRegistrations == null)
			delayedRegistrations = new List<ProgrammableObject>();
		delayedRegistrations.Add(programmableObject);
	}

	private void Select (int index)
	{
		string gameObjectName = gameObjectNames[index];
		//Debug.Log("Selecting " + gameObjectName + ".");
		if (currentObject != null)
			currentObject.SetActive(false);
		currentObject = gameObjects[gameObjectName];
		currentObject.SetActive(true);
		if (cameraPivot != null)
		{
			if (cameraTargets.ContainsKey(gameObjectName))
				cameraPivot.SetTarget(cameraTargets[gameObjectName]);
			else 
				cameraPivot.SetTarget(currentObject.GetComponent<Transform>());
		}
		currentIndex = index;
	}
	
	public void SelectNext ()
	{
		currentIndex++;
		if (currentIndex >= gameObjects.Count)
			currentIndex = 0;
		Select(currentIndex);
	}
	
	public void SelectPrevious ()
	{
		currentIndex--;
		if (currentIndex < 0)
			currentIndex = gameObjects.Count - 1;
		Select(currentIndex);
	}

	public void InstantiateCurrent ()
	{
		if (playSpace == null)
		{
			Debug.LogError("InstancesManager.InstantiateCurrent() - playSpace is null!");
			return;
		}
		playSpace.InstantiateObjectInScene(gameObjectNames[currentIndex]+" (instance)", gameObjectNames[currentIndex]);
	}
}
