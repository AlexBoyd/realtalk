using UnityEngine;
using System.Collections;

public class PlayVideo : MonoBehaviour {

	// Use this for initialization

	
	void Awake ()
	{
		((MovieTexture)guiTexture.texture).Play();

		audio.clip = ((MovieTexture)guiTexture.texture).audioClip;
		audio.Play ();
		Debug.Log ("Vid");
	}

}
