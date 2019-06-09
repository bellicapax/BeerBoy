using UnityEngine;
using System.Collections;

public class AdultPlayerCollision : MonoBehaviour {

    private bool haveBeenHit = false;
    private PlayerInput scriptInput;
    private Adult scriptAdult;

	// Use this for initialization
	void Start () 
    {
        scriptAdult = GetComponentInChildren<Adult>();
        scriptInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (!haveBeenHit)
        {
            if (other.tag == "PlayerCollider")
            {
                Scorekeeper.popularityMeter -= Scorekeeper.penaltyHitPartyer;
                scriptAdult.PlaySound("Ow");
                scriptInput.PlaySound(0, Random.Range(0.75f, 1.25f));
                scriptAdult.SpawnScore(Scorekeeper.penaltyHitPartyer, Scorekeeper.minusColor);
                haveBeenHit = true;
            }
        }
    }
}
