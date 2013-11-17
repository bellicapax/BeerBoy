using UnityEngine;
using System.Collections;

public class CalculateSize : MonoBehaviour {

    public Vector3 mySize;

	// Use this for initialization
	void Start () 
    {
        mySize = transform.renderer.bounds.size;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
