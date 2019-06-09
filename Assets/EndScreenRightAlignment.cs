using UnityEngine;
using System.Collections;

public class EndScreenRightAlignment : MonoBehaviour {

    private float xOffsetFromScreenWidth = -70.42688f;

	// Use this for initialization
	void Start () 
    {
        transform.XPosition(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - xOffsetFromScreenWidth, Camera.main.WorldToScreenPoint(transform.position).y, Camera.main.WorldToScreenPoint(transform.position).z)).x);
        //print(Camera.main.pixelWidth - Camera.main.WorldToScreenPoint(transform.position).x);
	}
	
}
