using UnityEngine;
using System.Collections;

public class GUIPlacement : MonoBehaviour {

    public bool left = true;
    //private float rightX = 221.3166f;           // Amount in pixels that the right GUI is away from the max width of the camera at 16:9 (the resolution I am working in inside the Editor)
    private float rightX = 318.8434f;           // Amount in pixels that the right GUI is away from the max width of the camera at 16:9 (the resolution I am working in inside the Editor)
    private float leftX = 81.08488f;            // ""                        left GUI is away from the 0 "
    private float myX;
    private Transform myTransform;
    private Camera myCam;

	// Use this for initialization
	void Start () 
    {
        myTransform = this.transform;
        myCam = myTransform.parent.GetComponent<Camera>();
        if (left)
        {
            myTransform.localPosition = new Vector3(myCam.ScreenToWorldPoint(new Vector3(leftX, myCam.WorldToScreenPoint(myTransform.position).y, myCam.WorldToScreenPoint(myTransform.position).z)).x, myTransform.localPosition.y, 0.0f);
        }
        else
        {
            myTransform.localPosition = new Vector3(myCam.ScreenToWorldPoint(new Vector3(myCam.pixelWidth - rightX, myCam.WorldToScreenPoint(myTransform.position).y, myCam.WorldToScreenPoint(myTransform.position).z)).x, myTransform.localPosition.y, 0.0f);
            //print(Screen.width - myCam.WorldToScreenPoint(myTransform.position).x);
        }
	}
	
}
