using UnityEngine;
using System.Collections;

public class BeerBoyGUI : MonoBehaviour {

    public Font bitOut;
    public Material fontMat;

    private GUIStyle myStyle;


	// Use this for initialization
	void Start () 
    {
        bitOut.material = fontMat;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
