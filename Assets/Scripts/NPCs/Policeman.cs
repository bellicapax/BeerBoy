using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]

public class Policeman : MonoBehaviour {

    public float copSpeed;

    private Vector3 direction;
    private Transform transPlayer;
    private Transform myTransform;
    private CharacterController myController;

	// Use this for initialization
	void Start () 
    {
        transPlayer = GameObject.FindWithTag("Player").transform;
        myTransform = GetComponent<Transform>();
        myController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        direction = transPlayer.position - myTransform.position;
        myController.SimpleMove(direction * copSpeed * Time.deltaTime);
	}
}
