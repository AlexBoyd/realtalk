using UnityEngine;
using System.Collections;

public class MenuButtonClick : MonoBehaviour {

	public void onClickControl()
	{
		Application.LoadLevel (0);
	}

	public void onClickSlider ()
	{
		Application.LoadLevel (1);
	}

	public void onClickToggle()
	{
		Application.LoadLevel (2);
	}

}
