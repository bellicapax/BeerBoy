using UnityEngine;
using System.Collections;

public class ChildPrime : MonoBehaviour {

    public GameObject goPolice;
    public int penaltyDidntGetRefreshment = 2;


    private bool haveBeenHit = false;

    private Vector3 offScreen = new Vector3(0.0f, -1000.0f, 0.0f);
    private Vector3 screenPos;

    private Animator myAnimator;
    private Transform myTransform;
    private LevelSpawner scriptSpawn;

    // Use this for initialization
    void Start()
    {
        myTransform = this.transform;
        myAnimator = GetComponent<Animator>();

        scriptSpawn = GameObject.FindWithTag("Creation").GetComponent<LevelSpawner>();
    }

    void Update()
    {
        OffScreenDestroyAndUpdateRefreshment();
    }

    void OffScreenDestroyAndUpdateRefreshment()
    {
        screenPos = Camera.main.WorldToScreenPoint(myTransform.position);
        if (screenPos.x < 0 || screenPos.y < 0)
        {
            if (!haveBeenHit)
                Scorekeeper.popularityMeter -= penaltyDidntGetRefreshment;
            GameObject.Destroy(this.gameObject);
        }
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
