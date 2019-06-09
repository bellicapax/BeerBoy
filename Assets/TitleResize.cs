using UnityEngine;
using System.Collections;

public class TitleResize : MonoBehaviour {

    private Vector3 myScale;

	// Use this for initialization
	void Start () 
    {
        myScale = transform.localScale;
        myScale *= (9.0f / 16.0f);
        myScale *= Camera.main.aspect;
        transform.localScale = myScale;
    }
	
}
