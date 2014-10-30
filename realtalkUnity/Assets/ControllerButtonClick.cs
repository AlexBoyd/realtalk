using UnityEngine;
using System.Collections;
using System.Linq;

public class ControllerButtonClick : MonoBehaviour {

    public KeyCode joystickButton;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.anyKey)
        {
            Debug.Log(FetchKey());
        }
        if (Input.GetKeyDown (joystickButton))
        {
            gameObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
        }
	}

    KeyCode FetchKey()
    {
        KeyCode[] validKeyCodes=(KeyCode[])System.Enum.GetValues(typeof(KeyCode));

        foreach(KeyCode i in validKeyCodes)
        {
            if (Input.GetKey (i))
            {
                return i;
            }
        }
        
        return KeyCode.None;
    }
     
}
