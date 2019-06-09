using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public float cameraScrollSpeed;

    private Vector3 dirToPlayer;
    private Vector3 dirToScroll;

    private Transform myTransform;
    private GameObject goPlayer;

	// Use this for initialization
	void Awake () 
    {
        goPlayer = GameObject.FindWithTag("Player");
        cameraScrollSpeed = goPlayer.GetComponent<PlayerInput>().scrollSpeed;
        myTransform = this.transform;
        //myTransform.LookAt(goPlayer.transform.position);
        dirToScroll = myTransform.InverseTransformDirection(Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () 
    {
        myTransform.Translate(dirToScroll * cameraScrollSpeed * Time.deltaTime);
	}
}
