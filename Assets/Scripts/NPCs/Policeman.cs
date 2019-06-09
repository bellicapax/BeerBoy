using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]

public class Policeman : MonoBehaviour {

    public float copSpeed = 30.0f;
    public float slowRate = 1.0f;
    public float secondsTillPoliceSlow;

    private bool onScreen = false;
    private float amountOffscreenY = -175.0f;
    private float amountOffscreenX = -50.0f;
    private Vector3 screenPos;
    private Vector3 direction;
    private Transform transPlayer;
    private Transform myTransform;
    private CharacterController myController;

	// Use this for initialization
	void Start () 
    {
        transPlayer = GameObject.FindWithTag("Player").transform;
        myTransform = this.transform;
        myController = GetComponent<CharacterController>();

        StartCoroutine(GetToSpawnPosition());
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (onScreen)
        {
            copSpeed -= slowRate * Time.deltaTime;
        }
        direction = transPlayer.position - myTransform.position;
        myController.Move(direction.normalized * copSpeed * Time.deltaTime);
	}

    private IEnumerator GetToSpawnPosition()
    {
        Vector3 spawnPosition = transPlayer.position;
        Vector3 spawnScreenPosition = Camera.main.WorldToScreenPoint(spawnPosition);

        while (spawnScreenPosition.x > amountOffscreenX && spawnScreenPosition.y > amountOffscreenY)
        {
            //print("X: " + spawnScreenPosition.x + " Y: " + spawnScreenPosition.y);
            yield return null;
            spawnScreenPosition = Camera.main.WorldToScreenPoint(spawnPosition);
        }
        myTransform.position = spawnPosition;
        StartCoroutine(AreWeOnScreen());
    }

    private IEnumerator AreWeOnScreen()
    {
        screenPos = Camera.main.WorldToScreenPoint(myTransform.position);
        while(screenPos.x < 0 || screenPos.y < 0)
        {
            yield return null;
            screenPos = Camera.main.WorldToScreenPoint(myTransform.position);
        }
        yield return new WaitForSeconds(secondsTillPoliceSlow);
        onScreen = true;
    }
}
