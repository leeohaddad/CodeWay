/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using System.Collections;

public class DefaultBattle : EvoLangImplementation
{
	public override void execute_command (string command)
	{
		float time = 1.0f / PlaySpace.GetVelocity();
		string cmd = command.ToLower();
		execute_animations(cmd, time);
	}
}
