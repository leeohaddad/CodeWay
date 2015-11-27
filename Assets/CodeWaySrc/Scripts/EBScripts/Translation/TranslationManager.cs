/*
   Copyright © 2015 EvoBooks
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Xml;

public class TranslationManager
{
    [Serializable]
    protected class LanguageFile
    {
        public LanguageFile(string name)
        { 
			this.name = name; 
		}

        public string name = "";

        public bool isLoaded = false;

        public class LanguageMessage
        {
            public string defaultMessage = "";

            public Dictionary<string, string> messageByLang = new Dictionary<string, string>();
        }

        public Dictionary<string, LanguageMessage> messages = new Dictionary<string, LanguageMessage>();
    }

    private static List<LanguageFile> languageFiles;
    private static Dictionary<string, LanguageFile> files = new Dictionary<string, LanguageFile>();

	private static List<TranslationToken> tokens;

	public static void RegisterToken (TranslationToken token)
	{
		if (tokens == null)
			tokens  = new List<TranslationToken>();
		tokens.Add(token);
	}

	public static void UpdateTokens ()
	{
		if (tokens == null)
			Debug.LogError("TranslationManager(): 'tokens' var is null!");
		foreach (TranslationToken token in tokens)
			token.UpdateText ();
	}

    private static void DoLoadFile(LanguageFile file)
    {
        TextAsset xmlText = (TextAsset)Resources.Load("Translation/" + file.name);

        if (xmlText != null)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlText.text);

            XmlNodeList nodes = xml.GetElementsByTagName("message");

            for (int i = 0; i < nodes.Count; i++)
            {
                LanguageFile.LanguageMessage message = new LanguageFile.LanguageMessage();

                string id = nodes[i].Attributes["id"].Value;

                XmlNodeList childNodes = nodes[i].ChildNodes;
                
                for (int j = 0; j < childNodes.Count; j++)
                    if (childNodes[j].LocalName == "language")
                    {

                        if (childNodes[j].Attributes["type"] != null)
                        {
                            if (childNodes[j].Attributes["type"].Value == "PT")
                            {
                                if (!message.messageByLang.ContainsKey("PT"))
                                    message.messageByLang.Add("PT", childNodes[j].InnerXml);
                            }
                            else if (childNodes[j].Attributes["type"].Value == "EN")
                            {
                                if (!message.messageByLang.ContainsKey("EN"))
                                    message.messageByLang.Add("EN", childNodes[j].InnerXml);
                            }
                            else if (childNodes[j].Attributes["type"].Value == "ES")
                            {
                                if (!message.messageByLang.ContainsKey("ES"))
                                    message.messageByLang.Add("ES", childNodes[j].InnerXml);
                            }
							else if (childNodes[j].Attributes["type"].Value == "PT_PT")
							{
								if (!message.messageByLang.ContainsKey("PT_PT"))
									message.messageByLang.Add("PT_PT", childNodes[j].InnerXml);
							}
                        } else message.defaultMessage = childNodes[j].InnerXml;

						if (message.defaultMessage == null || message.defaultMessage == "")
                        {
                            if (message.messageByLang.ContainsKey("EN"))
                                message.defaultMessage = message.messageByLang["EN"];
                            else
                                foreach (KeyValuePair<string, string> messageByLang in message.messageByLang)
                                { message.defaultMessage = messageByLang.Value; break; }
                        }

                        
                    }

				if (!(id == null || id == ""))
				{
                    file.messages.Add(id, message);
				}
            }

            file.isLoaded = true;
        }
		else
			Debug.Log("Fail on loading \'Translation/" + file.name + "\'");
    }

    private enum LanguageBypass { EN, PT, ES, PT_PT };

    public static string GetMessage(string fileId, string messageId)
    {
        if (!files.ContainsKey(fileId))
            files.Add(fileId, new LanguageFile(fileId));
           
        if (files.ContainsKey(fileId))
        {
            if (!files[fileId].isLoaded)
                DoLoadFile(files[fileId]);

            if (files[fileId].messages.ContainsKey(messageId))
            {
				string langKey = PlayerPrefs.GetString("Language");
				if (langKey.Trim().Length == 0)
				{
						langKey = "PT";
						PlayerPrefs.SetString("Language",langKey);
						PlayerPrefs.Save();
				}
				if (files[fileId].messages[messageId].messageByLang.ContainsKey(langKey))
                    return files[fileId].messages[messageId].messageByLang[langKey];
				 
                return files[fileId].messages[messageId].defaultMessage;
            }

            Debug.LogWarning("Message ID: '" + messageId + "' (from File ID: '" + fileId + "') not found in LanguageManager.");

            return "";
        }

        Debug.LogWarning("File ID: '" + fileId + "' not found in LanguageManager.");
        return "";
    }

    public static string GetMessage(string messageId)
    { 
		string defaultFileId = "CodeWayDictionary";
		return GetMessage(defaultFileId, messageId); 
	}

}
