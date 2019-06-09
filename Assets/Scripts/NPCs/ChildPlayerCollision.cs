using UnityEngine;
using System.Collections;

public class ChildPlayerCollision : MonoBehaviour {

    private bool haveBeenHit = false;
    private PlayerInput scriptInput;
    private Child scriptChild;

	// Use this for initialization
	void Start () 
    {
        scriptChild = GetComponentInChildren<Child>();
        scriptInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (!haveBeenHit)
        {
            if (other.tag == "PlayerCollider")
            {
                Scorekeeper.popularityMeter -= Scorekeeper.penaltyHitPartyer;
                scriptChild.PlaySound("Hit");
                scriptInput.PlaySound(0, Random.Range(0.75f, 1.25f));
                scriptChild.SpawnScore(Scorekeeper.penaltyHitPartyer, Scorekeeper.minusColor);
                haveBeenHit = true;
            }
        }
    }
}
