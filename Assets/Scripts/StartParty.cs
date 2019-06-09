using UnityEngine;
using System.Collections;

public class StartParty : MonoBehaviour {

    public GameObject goInstructions;

    private bool instructionsUp = false;
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetButtonDown("Beer"))
            {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Start")
                    Application.LoadLevel("Level1");
                else if (hit.transform.tag == "Instructions")
                {
                    if (!instructionsUp)
                    {
                        goInstructions.SetActive(true);
                        instructionsUp = true;
                    }
                    else
                    {
                        goInstructions.SetActive(false);
                        instructionsUp = false;
                    }
                }
            }
        }
	}
}
