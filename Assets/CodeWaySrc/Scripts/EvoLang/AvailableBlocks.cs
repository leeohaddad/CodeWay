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

public class AvailableBlocks : MonoBehaviour
{
	public TranslationToken libTitleTranslationToken;
	public List<EvoLangLib> libraries;
	public Transform model;
	private List<string> libsNames;
	private Dictionary< string , List<string> > libsCommandsNames;
	private Dictionary< string , EvoLangCmd > commands;
	private List<Transform> activeInstances;
	private int selectedLib = -1;

	void Start ()
	{
		if (libTitleTranslationToken == null)
			Debug.LogError("AvailableBlocks.Start() - libTitleTranslationToken is null!");
		if (model == null)
			Debug.LogError("AvailableBlocks.Start() - model is null!");
		else
			model.gameObject.SetActive(false);
		libsNames = new List<string>();
		libsCommandsNames = new Dictionary< string , List<string> >();
		commands = new Dictionary< string , EvoLangCmd >();
		activeInstances = new List<Transform>();
		foreach (EvoLangLib evoLangLib in libraries)
		{
			List<string> commandsNames = new List<string>();
			List<EvoLangCmd> commandsLocal = new List<EvoLangCmd>(evoLangLib.GetComponentsInChildren<EvoLangCmd>());
			foreach (EvoLangCmd evoLangCmd in commandsLocal)
			{
				if (!commandsNames.Contains(evoLangCmd.command))
					commandsNames.Add(evoLangCmd.command);
				if (!commands.ContainsKey(evoLangCmd.command))
					commands.Add(evoLangCmd.command, evoLangCmd);
			}
			if (!libsCommandsNames.ContainsKey(evoLangLib.libName))
				libsCommandsNames.Add(evoLangLib.libName, commandsNames);
			if (!libsNames.Contains(evoLangLib.libName))
				libsNames.Add(evoLangLib.libName);
		}
		if (libsNames.Count > 0)
			SelectLibPage(0);
	}

	private void EraseLibPageCmds ()
	{
		foreach (Transform instanceTransform in activeInstances)
			GameObject.Destroy(instanceTransform.gameObject);
		activeInstances.Clear();
	}

	private void SelectLibPage (int index)
	{
		string name, title;
		selectedLib = index;
		name = GetIthLibName(index);
		title = name.Replace("Lib","");
		libTitleTranslationToken.token = title;
		string titleText = TranslationManager.GetMessage (title);
		if (titleText == null || titleText.Length == 0)
		{
			Text textComponent = libTitleTranslationToken.gameObject.GetComponent<Text>();
			if (textComponent != null)
				textComponent.text = title;
		}
		EraseLibPageCmds();
		List<string> libsCmdNames = libsCommandsNames[name]; 
		foreach (string cmdName in libsCmdNames)
		{
			GameObject cmdInstance = GameObject.Instantiate(model.gameObject);
			EvoLangCmd cmd = commands[cmdName];
			cmdInstance.name = cmd.command;
			Transform cmdInstanceTransform = cmdInstance.GetComponent<Transform>();
			cmdInstanceTransform.parent = model.parent;
			cmdInstanceTransform.localPosition = Vector3.zero;
			cmdInstanceTransform.localEulerAngles = Vector3.zero;
			cmdInstanceTransform.localScale = Vector3.one;
			Image img = cmdInstance.GetComponent<Image>();
			img.sprite = cmd.sprite;
			img.color = cmd.color;
			cmdInstance.SetActive(true);
			activeInstances.Add(cmdInstanceTransform);
		}
	}
	
	public void SelectNextLibPage ()
	{
		if (libsNames.Count == 0)
			return;
		selectedLib++;
		if (selectedLib >= libsNames.Count)
			selectedLib = 0;
		SelectLibPage(selectedLib);
	}
	
	public void SelectPreviousLibPage ()
	{
		if (libsNames.Count == 0)
			return;
		selectedLib--;
		if (selectedLib < 0)
			selectedLib = libsNames.Count - 1;
		SelectLibPage(selectedLib);
	}
	
	private string GetIthLibName (int i)
	{
		int loop = 0;
		foreach (string name in libsNames)
		{
			if (loop == i)
				return name;
			loop++;
		}
		return null;
	}
}
