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
using System.IO;

public class PanelLoadBg : MonoBehaviour
{
	public static PanelLoadBg instance;
	public Animator myAnimator;
	public Dropdown mydropdown;
	private ProgrammableObject programmableObjectToLoadTo;
	private bool abort = false;

	void Start ()
	{
		if (myAnimator == null)
			myAnimator = this.gameObject.GetComponent<Animator>();
		if (mydropdown == null)
			mydropdown = this.gameObject.GetComponentInChildren<Dropdown>();
		instance = this;
		this.gameObject.SetActive(false);
	}

	void OnEnable ()
	{
		bool setElement = false;
		string sourceCodesFolder = "/SourceCodes";
		string path = Application.persistentDataPath + sourceCodesFolder;
		mydropdown.options.Clear();
		List<string> scripts = new List<string>(Directory.GetFiles(path, "*.evo"));
		foreach (string script in scripts)
		{
			string scriptName = script.Split(new string[]{sourceCodesFolder}, System.StringSplitOptions.None)[1];
			scriptName = scriptName.Substring(1,scriptName.Length-1);
			mydropdown.options.Add(new Dropdown.OptionData(scriptName));
			if (!setElement)
			{
				mydropdown.captionText.text = scriptName;
				setElement = true;
				mydropdown.value = 0;
			}
		}
	}
	
	public void EnterAnimation ()
	{
		if (abort)
		{
			abort = false;
			return ;
		}
		this.gameObject.SetActive(false);
		this.gameObject.SetActive(true);
	}
	
	public static void ExitAnimation ()
	{	if (instance != null)
			instance.myAnimator.Play("PanelLoadSaveExitAnimation");
		else
			Debug.LogError("PanelLoadBg.ExitAnimation() - instance is null!");
	}
	
	public void SetProgrammableObject (ProgrammableObject programmableObject)
	{
		programmableObjectToLoadTo = programmableObject;
	}
	
	public static void SetProgrammableObjectStatic (ProgrammableObject programmableObject)
	{
		if (instance != null)
			instance.SetProgrammableObject(programmableObject);
	}

	public int TryToLoadCodeToProgrammableObject (Dropdown sourceCode)
	{
		if (programmableObjectToLoadTo == null)
			return -1;
		programmableObjectToLoadTo.SetScript(sourceCode.captionText.text);
		programmableObjectToLoadTo = null;
		return 0;
	}

	public static int TryToLoadCodeToProgrammableObjectStatic (Dropdown sourceCode)
	{
		if (instance == null)
			return -2;
		return instance.TryToLoadCodeToProgrammableObject(sourceCode);
	}

	public void AbortLoad ()
	{
		abort = true;
	}

	public static void AbortLoadStatic()
	{
		if (instance != null)
			instance.AbortLoad();
		else
			Debug.LogError("PanelLoadBg.AbortLoadStatic - instance is null!");
	}
	
	public void Deactivate ()
	{
		this.gameObject.SetActive(false);
	}
}
