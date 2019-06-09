using UnityEngine;
using System.Collections;

public class PlayAgain : MonoBehaviour {

    void Start()
    {
        StartCoroutine(ClickToPlay());
    }

    IEnumerator ClickToPlay()
    {
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            if (Input.GetButtonDown("Beer"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "PlayAgain")
                    {
                        Application.LoadLevel("Start");
                    }
                }
            }
            yield return null;
        }
    }
}
