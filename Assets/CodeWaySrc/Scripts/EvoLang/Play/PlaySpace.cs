/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlaySpace : MonoBehaviour
{
	public bool autoregistrationOn;
	public CopyGlobalPosition cameraPivot;
	public Text selectedObjectNameText;
	private string selectedObjectName = "CodeWayProject";
	private int selectedObject = -1;
	public List<ProgrammableObject> programmableObjects; 
	private Dictionary<string,ProgrammableObject> programmableObjectsDic;
	private Dictionary<string,ProgrammableObject> objectsInScene;
	private static float velocity = 1.0f;
	private static float baseDistance = 5.0f;

	void Awake ()
	{
		if (PlayerPrefs.GetFloat("PlayVelocity") != 0)
			velocity = PlayerPrefs.GetFloat("PlayVelocity");
		if (programmableObjects != null && programmableObjects.Count>0)
		{
			if (programmableObjectsDic == null)
				programmableObjectsDic = new Dictionary<string,ProgrammableObject>();
			foreach (ProgrammableObject programmableObject in programmableObjects)
			{
				if (!programmableObjectsDic.ContainsKey(programmableObject.gameObject.name))
					programmableObjectsDic.Add(programmableObject.gameObject.name,programmableObject);
			}
		}
	}

	void Start ()
	{
		DebugLog();
		/*
		InstantiateObjectInScene("Kyle Instance", "Robot Kyle");
		InstantiateObjectInScene("Cube Instance", "Cube");
		InstantiateObjectInScene("Bruce Instance", "Dragon Bruce");
		InstantiateObjectInScene("Golem Instance", "Ice Golem");
		InstantiateObjectInScene("Lion Instance", "Lion");
		InstantiateObjectInScene("Spider Instance", "Giant Spider").SetScript("square.evo");
		*/
	}

	void Update ()
	{
		if (selectedObjectNameText != null)
		{
			if (selectedObject == -1)
				selectedObjectNameText.text = TranslationManager.GetMessage("CodeWayDictionary",selectedObjectName);
			else
				selectedObjectNameText.text = TranslationManager.GetMessage("ProgrammableObjectsDictionary",selectedObjectName);
			if (selectedObjectNameText.text == null || selectedObjectNameText.text.Length == 0)
				selectedObjectNameText.text = selectedObjectName;
		}
	}
	
	public void Play ()
	{
		Debug.Log("PlaySpace.Play()");
		foreach (string thisProgrammableObjectName in objectsInScene.Keys)
			objectsInScene[thisProgrammableObjectName].RunScript();
	}

	private void ResetObject (int index)
	{
		Transform objectTransform = GetIthObjectInSceneTransform(index);
		objectTransform.parent.localPosition = Vector3.zero;
		objectTransform.parent.localEulerAngles = Vector3.zero;
		objectTransform.parent.localScale = Vector3.one;
	}
	
	public void ResetSelectedObject ()
	{
		if (selectedObject == -1)
		{
			DebugMessage.ErrorMessage(TranslationManager.GetMessage("SelectObjectFirst"));
			return;
		}
		Debug.Log("Resetting object " + selectedObjectName + ".");
		ResetObject(selectedObject);
	}

	public void ResetAllObjects ()
	{
		int objectsInSceneCount = objectsInScene.Count;
		for (int i=0; i<objectsInSceneCount; i++)
			ResetObject(i);
	}
	
	public void PrepareSourceCodeEditorToReceiveBehavior ()
	{
		PanelLoadBg.SetProgrammableObjectStatic(null);
	}

	public void PrepareSelectedObjectToReceiveBehavior ()
	{
		if (selectedObject == -1)
		{
			DebugMessage.ErrorMessage(TranslationManager.GetMessage("SelectObjectFirst"));
			PanelLoadBg.AbortLoadStatic();
			return;
		}
		Debug.Log("Preparing object " + selectedObjectName + " to receive behavior.");
		PanelLoadBg.SetProgrammableObjectStatic(GetIthObjectInSceneTransform(selectedObject).GetComponent<ProgrammableObject>());
	}
	
	public void SelectNextObject ()
	{
		if (cameraPivot == null)
		{
			Debug.LogError("PlaySpace.SelectNextObject() - Cannot select object because cameraPivot is null!");
			selectedObject = -1;
			return;
		}
		if (objectsInScene == null || objectsInScene.Keys.Count == 0)
		{
			selectedObject = -1;
			if (selectedObjectNameText != null)
				selectedObjectNameText.text = TranslationManager.GetMessage("CodeWayDictionary","CodeWayProject");
			return;
		}
		int index = selectedObject + 1;
		if (index >= objectsInScene.Keys.Count)
			index = 0;
		selectedObject = index;
		Transform targetTransform = GetIthObjectInSceneTransform(selectedObject);
		ProgrammableObject programmableObject = targetTransform.GetComponent<ProgrammableObject>();
		selectedObjectName = targetTransform.gameObject.name;
		if (programmableObject.cameraTarget != null)
			cameraPivot.SetTarget(programmableObject.cameraTarget);
		else
			cameraPivot.SetTarget(targetTransform);
	}
	
	public void SelectPreviousObject ()
	{
		if (cameraPivot == null)
		{
			Debug.LogError("PlaySpace.SelectNextObject() - Cannot select object because cameraPivot is null!");
			return;
		}
		if (objectsInScene == null || objectsInScene.Keys.Count == 0)
		{
			if (selectedObjectNameText != null)
				selectedObjectNameText.text = TranslationManager.GetMessage("CodeWayDictionary","CodeWayProject");
			return;
		}
		int index = selectedObject - 1;
		if (index <= -1)
			index = objectsInScene.Keys.Count - 1;
		selectedObject = index;
		Transform targetTransform = GetIthObjectInSceneTransform(selectedObject);
		ProgrammableObject programmableObject = targetTransform.GetComponent<ProgrammableObject>();
		selectedObjectName = targetTransform.gameObject.name;
		if (programmableObject.cameraTarget != null)
			cameraPivot.SetTarget(programmableObject.cameraTarget);
		else
			cameraPivot.SetTarget(targetTransform);
	}
	
	private Transform GetIthObjectInSceneTransform (int i)
	{
		int loop = 0;
		foreach (string key in objectsInScene.Keys)
		{
			if (loop == i)
			{
				Transform thisTransform = objectsInScene[key].GetComponent<Transform>();
				return thisTransform;
			}
			loop++;
		}
		return null;
	}

	private ProgrammableObject InstantiateObjectInScene (string instanceName, ProgrammableObject programmableObject)
	{
		ProgrammableObject instance;
		if (objectsInScene == null)
			objectsInScene = new Dictionary<string,ProgrammableObject>();
		Debug.Log("PlaySpace.RegisterObjectInScene() - Adding object " + programmableObject.name + " to the scene as " + instanceName + ".");
		if (!objectsInScene.ContainsKey(instanceName))
		{
			instance = programmableObject.Instantiate();
			Transform objectTransform = instance.gameObject.GetComponent<Transform>();
			if (objectTransform != null)
			{
				Transform parentTransform = objectTransform.parent;
				if (parentTransform != null)
					parentTransform.gameObject.name = instanceName;
			}
			objectsInScene.Add(instanceName, instance);
		}
		else instance = objectsInScene[instanceName];
		return instance;
	}

	public ProgrammableObject InstantiateObjectInScene (string instanceName, string programmableObjectName)
	{
		return InstantiateObjectInScene (instanceName,programmableObjectsDic[programmableObjectName]);
	}
	
	public void RegisterProgrammableObject (string instanceName, ProgrammableObject programmableObject)
	{
		if (!autoregistrationOn)
			return;
		if (programmableObjects == null)
			programmableObjects = new List<ProgrammableObject>();
		//Debug.Log("PlaySpace.RegisterProgrammableObject() - Adding programmable object " + programmableObject.name + ".");
		if (!programmableObjects.Contains(programmableObject))
		{
			programmableObjects.Add(programmableObject);
			if (programmableObjectsDic == null)
				programmableObjectsDic = new Dictionary<string,ProgrammableObject>();
			programmableObjectsDic.Add(instanceName,programmableObject);
		}
	}

	public static float GetVelocity ()
	{
		return velocity;
	}
	
	public static float GetBaseDistance ()
	{
		return baseDistance;
	}

	public void DebugLog ()
	{
		string log = "PlaySpace.DebugLog()" + "\n";
		if (programmableObjectsDic != null)
		{
			log = log + programmableObjectsDic.Keys.Count + " programmableObject(s):" + "\n";
			foreach (string key in programmableObjectsDic.Keys)
				log = log + " " + key + ": " + programmableObjectsDic[key] + "\n";
		}
		else log = log + "0 programmableObjects" + "\n";
		if (objectsInScene != null)
		{
			log = log + objectsInScene.Keys.Count + " object(s)InScene:" + "\n";
			foreach (string key in objectsInScene.Keys)
				log = log + " " + key + ": " + objectsInScene[key] + "\n";
		}
		else log = log + "0 objectsInScene" + "\n";
		Debug.Log(log);
	}
}
