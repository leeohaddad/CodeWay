/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContextMenuBehaviors : MonoBehaviour
{
	public ContextMenuOnClick relatedContextMenu;
	public SourceCode sourceCodeComponent;
	
	public void Portuguese ()
	{
		PlayerPrefs.SetString("Language","PT");
		PlayerPrefs.Save();
		TranslationManager.UpdateTokens();
		Exit();
	}
	
	public void English ()
	{
		PlayerPrefs.SetString("Language","EN");
		PlayerPrefs.Save();
		TranslationManager.UpdateTokens();
		Exit();
	}
	
	public void Spanish ()
	{
		PlayerPrefs.SetString("Language","ES");
		PlayerPrefs.Save();
		TranslationManager.UpdateTokens();
		Exit();
	}
	
	public void LoadCode (Dropdown sourceCode)
	{
		int result = -1;
		result = PanelLoadBg.TryToLoadCodeToProgrammableObjectStatic(sourceCode);
		if (result != 0 && sourceCodeComponent != null)
			result = sourceCodeComponent.LoadCode(sourceCode);
		if (result == 0)
		{
			DebugMessage.SuccessMessage(TranslationManager.GetMessage("LoadedSuccessfully"));
		} else
		{
			DebugMessage.ErrorMessage(TranslationManager.GetMessage("ErrorLoading"));
		}
		PanelLoadBg.ExitAnimation();
	}
	
	public void SaveCode (Text nameTextComponent)
	{
		int result = -1;
		string text = "";
		if (nameTextComponent != null)
			text = nameTextComponent.text;
		if (sourceCodeComponent != null)
			result = sourceCodeComponent.SaveCode(text);
		if (result == 0)
		{
			DebugMessage.SuccessMessage(TranslationManager.GetMessage("SavedSuccessfully"));
			PanelSaveBg.ExitAnimation();
		} else
		{
			DebugMessage.ErrorMessage(TranslationManager.GetMessage("ErrorSaving"));
		}
	}
	
	public void Exit ()
	{
		relatedContextMenu.ContextMenuButtonClicked();
	}
}
