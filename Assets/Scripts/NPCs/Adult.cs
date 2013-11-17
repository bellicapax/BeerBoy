using UnityEngine;
using System.Collections;

public class Adult : MonoBehaviour {

    public Sprite[] adultSprites;

    private bool haveBeenHit = false;
    private Quaternion originalRotation;
    private SpriteRenderer mySpriteRenderer;
    private Animator myAnimator;
    private Transform parentTransform;
    private Transform myTransform;

	// Use this for initialization
	void Start () 
    {
        myTransform = this.transform;
        originalRotation = myTransform.rotation;
        parentTransform = myTransform.parent.transform;
        mySpriteRenderer = parentTransform.GetComponent<SpriteRenderer>();
        myAnimator = parentTransform.GetComponent<Animator>();
        parentTransform.rotation = Camera.main.transform.rotation;
        //myTransform.rotation = originalRotation;
	}

    void OnTriggerEnter(Collider other)
    {
        if (!haveBeenHit)
        {
            if (other.tag == "Beer")
            {
                myAnimator.SetBool("Beer", true);
                haveBeenHit = true;
            }
            else if (other.tag == "Soda")
            {
                myAnimator.SetBool("Soda", true);
                haveBeenHit = true;
            }
        }
    }
}
