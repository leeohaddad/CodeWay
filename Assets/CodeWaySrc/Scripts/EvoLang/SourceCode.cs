/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class SourceCode : MonoBehaviour
{
	private InputField inputFieldComponent;
	private Text textComponent;

	void Start ()
	{
		inputFieldComponent = this.gameObject.GetComponent<InputField>();
		if (inputFieldComponent == null)
			inputFieldComponent = this.gameObject.GetComponentInChildren<InputField>();
	}

	/*public void RunCode (string sourceCode, ProgrammableObject programmableObject)
	{
		string line = sourceCode;
		string[] tokens = line.Split(' ');
		//string cmd = tokens[0];
		//string arg = tokens[1];
		//foreach (EvoLangLib lib in programmableObject.librariesSupported) {
		//	if (lib.Commands () [cmd] != null)
		//	{
		//		Debug.Log ("Invoking " + cmd + " with arg " + arg + ".");
		//		lib.Commands () [cmd].Invoke ();
		//	}
		//}
	}
	public void RunCode (ProgrammableObject programmableObject)
	{
		InputField inputField = this.gameObject.GetComponent<InputField>();
		Debug.Log("Source Code: " + inputField.text);
		RunCode(inputField.text, programmableObject);
	}*/
	
	public int LoadCode (Dropdown sourceCode)
	{
		string path = Application.persistentDataPath + "/SourceCodes/" + sourceCode.captionText.text;
		Debug.Log("Sourceode.LoadCode(): " + sourceCode.captionText.text);
		Debug.Log("Sourceode.LoadCode() - Path: " + path);
		if (!File.Exists (path))
		{
			Debug.LogError("SourceCode.LoadCode() - Script '" + sourceCode.captionText.text + "' was not found.");
			return -2;
		}
		try
		{
			inputFieldComponent.text = File.ReadAllText(path);
			return 0;
		}
		catch (System.Exception e)
		{
			Debug.LogError("SourceCode.LoadCode() - Problem loading " + sourceCode.captionText.text + ".");
			Debug.LogError("Exception  Messagge: ' " + e.Message + " '.");
		}
		return -1;
	}
	
	public int SaveCode (string fileName)
	{
		string path = Application.persistentDataPath + "/SourceCodes";
		Debug.Log("SourceCode.SaveCode(): " + inputFieldComponent.text);
		Debug.Log("SourceCode.SaveCode() - Path: " + path);
		if (!Directory.Exists(path))
			Directory.CreateDirectory(path);
		try
		{
			File.WriteAllText(path + "/" + fileName + ".evo", inputFieldComponent.text);
			return 0;
		}
		catch (System.Exception e)
		{
			Debug.LogError("SourceCode.SaveCode() - Problem saving " + fileName + ".evo.");
			Debug.LogError("Exception  Messagge: ' " + e.Message + " '.");
		}
		return -1;
	}
}
