/*
   Copyright © 2015 Leonardo Haddad Carlos
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EvoLangLib : MonoBehaviour
{
	public string libName;
	public EvoLangImplementation defaultImplementation;
	private static Dictionary<string,EvoLangImplementation> defaultImplementations;


	void Awake ()
	{
		if (libName.Length == 0)
			libName = this.gameObject.name;
		if (defaultImplementations == null)
			defaultImplementations = new Dictionary<string,EvoLangImplementation>();
		defaultImplementations.Add (libName, defaultImplementation);
	}

	public static EvoLangImplementation GetDefaultImplementation (string libName)
	{
		if (defaultImplementations == null)
			return null;
		return defaultImplementations[libName];
	}
}
