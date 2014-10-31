using UnityEngine;
using System.Collections;

public class VAControl : MonoBehaviour {

	public float VAChartScale = 25;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float horizontal = Input.GetAxis ("Horizontal");
		//float vertical = Input.GetAxis ("Vertical");
		transform.localPosition = new Vector2 (horizontal * VAChartScale, 0f);
	}
}
