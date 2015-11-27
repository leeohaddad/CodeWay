/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ProgrammableObject : MonoBehaviour
{
	public static Dictionary<string,Transform> allObjects;
	public PlaySpace playSpace;
	public List<EvoLangImplementation> librariesSupported;
	public Transform cameraTarget;
	private string scriptName;
	private Transform objTransform, parentTransform, playSpaceTransform;
	private bool isRoot = true;

	void Awake ()
	{
		objTransform = this.GetComponent<Transform>();
		parentTransform = objTransform.parent;
		if (playSpace == null)
			playSpace = GetPlaySpaceInAncestrals(objTransform);
		playSpaceTransform = playSpace.GetComponent<Transform>();
		if (isRoot)
		{
			parentTransform.gameObject.SetActive(false);
			Register();
		}
	}

	public ProgrammableObject Instantiate ()
	{
		GameObject instance = GameObject.Instantiate(parentTransform.gameObject);
		Transform instanceTransform = instance.GetComponent<Transform>();
		instanceTransform.SetParent(playSpaceTransform);
		ProgrammableObject[] instancePO = instance.GetComponentsInChildren<ProgrammableObject>(true);
		instancePO[0].SetIsRoot(false);
		instance.SetActive(true);
		return instancePO[0];
	}

	private IEnumerator RunScriptCoroutine ()
	{
		float time = 1.0f / PlaySpace.GetVelocity();
		string path = Application.persistentDataPath + "/SourceCodes/" + scriptName;
		string sourceCode = File.ReadAllText(path);
		List<string> commands = new List<string>(sourceCode.Split(';'));
		foreach (string splittedCommand in commands)
		{
			string cmd = splittedCommand.Trim();
			if (cmd.Length > 0)
			{
				//Debug.Log(this.gameObject.name + " running command " + cmd + ".");
				foreach (EvoLangImplementation libraryImplementation in librariesSupported)
					libraryImplementation.execute_command(cmd);
				yield return new WaitForSeconds(time);
			}
		}
	}

	public void RunScript ()
	{
		if (scriptName == null || scriptName.Length == 0)
			return;
		StartCoroutine(RunScriptCoroutine());
	}
	
	public void SetScript (string pScriptName)
	{
		scriptName = pScriptName;
	}
	
	public string GetScriptName ()
	{
		return scriptName;
	}
	
	void Register ()
	{
		if (allObjects == null)
			allObjects = new Dictionary<string,Transform>();
		if (!allObjects.ContainsKey(objTransform.gameObject.name))
		{
			allObjects.Add(objTransform.gameObject.name, parentTransform);
		}
		else Debug.LogError ("ProgrammableObject.Register() - Error! Object model " + objTransform.gameObject.name + " already exists!");
		if (playSpace != null)
		{
			playSpace.RegisterProgrammableObject(this.gameObject.name,this);
		}
		InstancesManager.RegisterProgrammableObjectStatic(this);
	}

	PlaySpace GetPlaySpaceInAncestrals (Transform thisTransform)
	{
		PlaySpace thisPlaySpace;
		while (thisTransform.parent != null)
		{
			thisPlaySpace = thisTransform.parent.gameObject.GetComponent<PlaySpace>();
			if (thisPlaySpace != null)
				return thisPlaySpace;
			thisTransform = thisTransform.parent;
		}
		return null;
	}

	public void SetIsRoot (bool arg)
	{
		isRoot = arg;
	}
}
