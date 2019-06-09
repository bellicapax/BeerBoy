using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour {

    private PlayerInput scriptInput;

	// Use this for initialization
	void Start () 
    {
        scriptInput = this.transform.parent.GetComponent<PlayerInput>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AmmoBeer")
        {
            scriptInput.AmmoBeerCollision(other.gameObject);
        }
        else if (other.tag == "AmmoSoda")
        {
            scriptInput.AmmoSodaCollision(other.gameObject);
        }
        else if (other.tag == "Police")
        {
            Application.LoadLevel("End");
        }
    }
}
