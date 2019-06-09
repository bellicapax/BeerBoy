using UnityEngine;
using System.Collections;

public class EndScreenLeftAlignment : MonoBehaviour {

    private float xOffsetFrom0 = 809.0f;

	// Use this for initialization
	void Start () 
    {
        //transform.XPosition(Camera.main.ScreenToWorldPoint(new Vector3(xOffsetFrom0, Camera.main.WorldToScreenPoint(transform.position).y, Camera.main.WorldToScreenPoint(transform.position).z)).x);
	    print(Camera.main.WorldToScreenPoint(transform.position).x);
	}

}
