using UnityEngine;
using System.Collections;

// Simple Splash Screen for Unity
// No copyright asserted on this code or included files.  They may be used for any purpose.
// See http://apps.burlock.org/unity-splash-screen/ for instructions on how to use this class
// Written by Neil Burlock
// Version 1.0, 09 Jun 2014

public class SplashScreen : MonoBehaviour {
	public string loadLevelName; // Name of the level to load after the splash screen appears
	private bool isLoading = false;

	public static SplashScreen me;
	void Awake() {
		me = this;
	}
	
	void Start () {
		// Adjust the width of the image to fill the screen while maintaining the image aspect ratio
		GUITexture image = gameObject.GetComponent<GUITexture>();
		float imageRatio = (float) image.texture.width / (float) image.texture.height;		
		float screenRatio = (float) Screen.width / (float) Screen.height;
		Vector3 scale = Vector3.one;
		if (Screen.width >= Screen.height) scale.x *= imageRatio / screenRatio;
		else scale.y *= screenRatio / imageRatio;
		Debug.Log(transform.localScale + "=>" + scale);
	}
	
	void Update() {
		// Start loading the level on the next frame
		if (!isLoading) {
			StartCoroutine(TimerLoadLevelAdditive(1.5f,me.loadLevelName));
			isLoading = true;
		}
	}

	private IEnumerator TimerLoadLevelAdditive (float seconds, string levelName)
	{
		yield return new WaitForSeconds(seconds);
		Application.LoadLevelAdditive(levelName);
	}
	
	// Call from the loaded level to hide the splash
	public static void Hide() {
		if (me != null) me.gameObject.SetActive(false);
	}
}
