/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;
using System.IO;

public class TakeScreenshot : MonoBehaviour
{
	public int superSize = 4;

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			string path = "ScreenShots/";
			string fileName = "CodeWay-SS-";
			int fileNumber = 1;
			string sufix = ".png";
			while (File.Exists(path + fileName + fileNumber + sufix))
				fileNumber++;
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			Debug.Log("Saving SS to " + Directory.GetCurrentDirectory() + "\\" + path + "...");
			Application.CaptureScreenshot(path + fileName + fileNumber + sufix, superSize);
			Debug.Log("Saves SS successfully!");
		}
	}
}
