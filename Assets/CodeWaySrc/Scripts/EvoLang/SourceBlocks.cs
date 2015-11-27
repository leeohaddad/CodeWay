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

public class SourceBlocks : MonoBehaviour
{
	public SourceCode sourceCode;
	public ControlYPositionScaledViaSlider yPositionSlider;
	public Transform cmdCubeModel;
	public Transform linkCubeModel;
	public Transform highlightCube;
	public List<EvoLangLib> libraries;
	private List<Transform> cmdCubes;
	private List<Transform> linkCubes;
	private Dictionary<string,Material> cmdMaterials;
	private InputField inputFieldComponent;
	private bool avoidNextTextUpdate = false;
	private int cmdCount = 0;
	private int selectedBlock = -1;
	private string commandToAdd = "";

	void Start ()
	{
		if (sourceCode == null)
			Debug.LogError("SourceBlocks.Start() - sourceCode is null!");
		else
			inputFieldComponent = sourceCode.gameObject.GetComponent<InputField>();
		if (cmdCubeModel == null)
			Debug.LogError("SourceBlocks.Start() - cmdCubeModel is null!");
		else
			cmdCubeModel.gameObject.SetActive(false);
		if (linkCubeModel == null)
			Debug.LogError("SourceBlocks.Start() - linkCubeModel is null!");
		else
			linkCubeModel.gameObject.SetActive(false);
		if (highlightCube == null)
			Debug.Log ("SourceBlocks.Start() - highlightCube is null!");
		else
			highlightCube.gameObject.SetActive(false);
		cmdCubes = new List<Transform>();
		linkCubes = new List<Transform>();
		cmdMaterials = new Dictionary<string,Material>();
		foreach (EvoLangLib evoLangLib in libraries)
		{
			List<EvoLangCmd> commands = new List<EvoLangCmd>(evoLangLib.GetComponentsInChildren<EvoLangCmd>());
			foreach (EvoLangCmd evoLangCmd in commands)
			{
				if (!cmdMaterials.ContainsKey(evoLangCmd.command))
					cmdMaterials.Add(evoLangCmd.command,evoLangCmd.material);
			}
		}
	}

	void Update ()
	{
		if (yPositionSlider != null)
			yPositionSlider.maxY = (cmdCount-1) * 1.5f;
	}

	void OnEnable ()
	{
		if (avoidNextTextUpdate)
			avoidNextTextUpdate = false;
		// Update Source Blocks!
		if (cmdCubes != null)
		{
			foreach (Transform cmdCube in cmdCubes)
			{
				GameObject.Destroy(cmdCube.gameObject);
			}
			cmdCubes.Clear();
		}
		if (linkCubes != null)
		{
			foreach (Transform linkCube in linkCubes)
			{
				GameObject.Destroy(linkCube.gameObject);
			}
			linkCubes.Clear();
		}
		cmdCount = 0;
		if (inputFieldComponent == null)
			inputFieldComponent = sourceCode.gameObject.GetComponent<InputField>();
		List<string> commands = new List<string>(inputFieldComponent.text.Split(';'));
		foreach (string splittedCommand in commands)
		{
			string cmd = splittedCommand.Trim();
			if (cmd.Length > 0)
			{
				if (cmdMaterials.ContainsKey(cmd))
				{
					AddCommandBlock(cmd);
				}
				else
				{
					DebugMessage.ErrorMessage("'" + cmd + "' " + TranslationManager.GetMessage("NotRecognizedCommand"));
				}
			}
		}
		SetSelectedBlock(-1);
	}

	void OnDisable ()
	{
		// Update Source Code!
		if (avoidNextTextUpdate)
		{
			avoidNextTextUpdate = false;
			return;
		}
		if (inputFieldComponent == null)
			inputFieldComponent = sourceCode.gameObject.GetComponent<InputField>();
		inputFieldComponent.text = "";
		foreach (Transform cmdCubeTransform in cmdCubes)
		{
			inputFieldComponent.text = inputFieldComponent.text + cmdCubeTransform.gameObject.name + ";\n";
		}
	}
	
	public void AddCommandBlock (string cmd)
	{
		AddCommandBlock(cmdCount,cmd);
	}
	
	public void AddCommandBlock (int index, string cmd)
	{
		if (index > cmdCount)
		{
			Debug.LogError("SourceBlocks.AddCommandBlock() - invalid index!");
			return;
		}
		InstantiateCmdCube(index,cmd);
		if (index == 0)
			InstantiateLinkCube(index+1);
		else
			InstantiateLinkCube(index);
		cmdCount++;
		int loop = 0;
		foreach (Transform cmdCubeTransform in cmdCubes)
		{
			if (index < loop)
			{
				cmdCubeTransform.localPosition = new Vector3(0, cmdCubeTransform.localPosition.y -  1.5f, 0);
				cmdCubeTransform.gameObject.GetComponent<EvoLangCmdCube>().SetIndex(loop);
			}
			loop++;
		}
		loop = 1;
		foreach (Transform linkCubeTransform in linkCubes)
		{
			if (index < loop)
			{
				linkCubeTransform.localPosition = new Vector3(0, linkCubeTransform.localPosition.y -  1.5f, 0);
			}
			loop++;
		}
		if (index == 0 && linkCubes.Count > 0)
		{
			Transform firstLinkCubeTransform = linkCubes[0];
			firstLinkCubeTransform.localPosition = new Vector3(0, firstLinkCubeTransform.localPosition.y +  1.5f, 0);
		}
	}
	
	private void InstantiateCmdCube (int index, string cmd)
	{
		GameObject newCube = GameObject.Instantiate(cmdCubeModel.gameObject);
		newCube.name = cmd;
		Transform newCubeTransform = newCube.GetComponent<Transform>();
		newCubeTransform.parent = cmdCubeModel.parent;
		newCubeTransform.localPosition = new Vector3(0, 0 - index * 1.5f, 0);
		newCubeTransform.localEulerAngles = Vector3.zero;
		newCubeTransform.localScale = Vector3.one;
		newCube.GetComponent<Renderer>().material = cmdMaterials[cmd];
		newCube.GetComponent<EvoLangCmdCube>().SetIndex(index);
		newCube.SetActive(true);
		cmdCubes.Insert(index, newCubeTransform);
	}
	
	private void InstantiateLinkCube (int index)
	{
		if (cmdCount == 0)
			return;
		GameObject newCube = GameObject.Instantiate(linkCubeModel.gameObject);
		newCube.name = "LinkCube";
		Transform newCubeTransform = newCube.GetComponent<Transform>();
		newCubeTransform.parent = linkCubeModel.parent;
		index--;
		newCubeTransform.localPosition = new Vector3(0, -0.5f - index * 1.5f, 0);
		newCubeTransform.localEulerAngles = Vector3.zero;
		newCubeTransform.localScale = new Vector3(0.25f,1.0f,0.25f);
		newCube.SetActive(true);
		Debug.Log("Index: " + index + " (size: " + linkCubes.Count + ")");
		linkCubes.Insert(index, newCubeTransform);
	}

	public void SetSelectedBlock (int pSelectedBlock)
	{
		selectedBlock = pSelectedBlock;
		if (highlightCube != null)
		{
			if (pSelectedBlock == -1)
			{
				highlightCube.gameObject.SetActive(false);
			}
			else
			{
				highlightCube.localPosition = new Vector3(0, 0 - pSelectedBlock * 1.5f, 0);
				highlightCube.gameObject.SetActive(true);
			}
		}
	}

	public void DeleteSelectedBlock ()
	{
		Transform cmdCubeToDelete = null;
		Transform linkCubeToDelete = null;
		int index;
		if (selectedBlock == -1)
		{
			DebugMessage.ErrorMessage(TranslationManager.GetMessage("SelectObjectFirst"));
			return;
		}
		index = 0;
		foreach (Transform cmdCubeTransform in cmdCubes)
		{
			if (selectedBlock == index)
			{
				cmdCubeToDelete = cmdCubeTransform;
			}
			else if (selectedBlock < index)
			{
				cmdCubeTransform.localPosition = new Vector3(0, cmdCubeTransform.localPosition.y + 1.5f, 0);
				cmdCubeTransform.gameObject.GetComponent<EvoLangCmdCube>().SetIndex(index-1);
			}
			index++;
		}
		cmdCubes.Remove(cmdCubeToDelete);
		GameObject.Destroy(cmdCubeToDelete.gameObject);
		cmdCount--;
		if (cmdCount == selectedBlock)
		{
			if (selectedBlock > 0)
			{
				index = 1;
				foreach (Transform linkCubeTransform in linkCubes)
				{
					if (selectedBlock == index)
					{
						linkCubeToDelete = linkCubeTransform;
					}
					index++;
				}
				linkCubes.Remove(linkCubeToDelete);
				GameObject.Destroy(linkCubeToDelete.gameObject);
			}
		}
		else
		{
			index = 0;
			foreach (Transform linkCubeTransform in linkCubes)
			{
				if (selectedBlock == index)
				{
					linkCubeToDelete = linkCubeTransform;
				}
				else if (selectedBlock < index)
				{
					linkCubeTransform.localPosition = new Vector3(0, linkCubeTransform.localPosition.y + 1.5f, 0);
				}
				index++;
			}
			linkCubes.Remove(linkCubeToDelete);
			GameObject.Destroy(linkCubeToDelete.gameObject);
		}
		SetSelectedBlock(-1);
	}

	public void AvoidNextTextUpdate ()
	{
		avoidNextTextUpdate = true;
	}

	public string GetCommandToAdd ()
	{
		return commandToAdd;
	}

	public void SetCommandToAdd (GameObject gameObject)
	{
		commandToAdd = gameObject.name;
	}

	public void CancelCommandToAddDelayed ()
	{
		StartCoroutine (CancelCommandToAddAfterDelay(0.01f));
	}

	private IEnumerator CancelCommandToAddAfterDelay (float delayInSeconds)
	{
		yield return new WaitForSeconds(delayInSeconds);
		commandToAdd = "";
	}

	public int GetBlocksCount ()
	{
		return cmdCubes.Count;
	}
}
