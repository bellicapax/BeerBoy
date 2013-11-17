using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Child : MonoBehaviour {

    public GameObject goPolice;

    private bool haveBeenHit = false;

    private Vector3 offScreen = new Vector3(0.0f, -1000.0f, 0.0f);

    private Quaternion originalRotation;
    private SpriteRenderer mySpriteRenderer;
    private Animator myAnimator;
    private Transform parentTransform;
    private Transform myTransform;
    private LevelSpawner scriptSpawn;

    // Use this for initialization
    void Start()
    {
        myTransform = this.transform;
        originalRotation = myTransform.rotation;
        parentTransform = myTransform.parent.transform;

        mySpriteRenderer = parentTransform.GetComponent<SpriteRenderer>();
        myAnimator = parentTransform.GetComponent<Animator>();

        scriptSpawn = GameObject.FindWithTag("Creation").GetComponent<LevelSpawner>();

        parentTransform.rotation = Camera.main.transform.rotation;      // Face the camera
        //myTransform.rotation = originalRotation;                        // But keep the 
    }

    void OnTriggerEnter(Collider other)
    {
        if (!haveBeenHit)
        {
            if (other.tag == "Soda")
            {
                myAnimator.SetBool("Soda", true);
                haveBeenHit = true;
            }
            else if (other.tag == "Beer")
            {
                myAnimator.SetBool("Beer", true);
                SpawnPoliceman();
                haveBeenHit = true;
            }
        }
    }

    void SpawnPoliceman()
    {
        int spawnPosition = Random.Range(0, scriptSpawn.remainingTrashCanPositions.Count - 1);
        GameObject policeClone = GameObject.Instantiate(goPolice, offScreen, goPolice.transform.rotation) as GameObject;
        policeClone.transform.parent = scriptSpawn.tileTransform;
        policeClone.transform.localPosition = scriptSpawn.remainingTrashCanPositions[spawnPosition];
    }
}
