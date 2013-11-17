using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {

    public float forwardMoveSpeed = 25f;
    public float scrollSpeed = 50f;
    public float slowdownPercentage = 0.2f;
    public float slowdownDuration = 2.0f;

    public LayerMask groundMask;

    public Transform beerNE;
    public Transform beerNW;
    public Transform beerSE;
    public Transform beerSW;

    public GameObject goBeer;
    public GameObject goSoda;
    public GameObject goTest;

    public AudioClip[] playersounds;

    public int ammoBeer = 6;
    public int ammoSoda = 6;

    private bool touchingGround = false;

    // Labels for input axes
    private string vertical = "Vertical";
    private string horizontal = "Horizontal";
    private string beer = "Beer";
    private string soda = "Soda";
    private string adultWantsBeer = "AdultWantsBeer";
    private string childWantsSoda = "ChildWantsSoda";

    private float angleBetween;
    private float backwardMoveSpeed;
    private float originalForwardSpeed;
    private float slowdownDecrement;


    private Vector3 moveByThis;     // Amount to move each frame
    private Vector3 dirToMouse;     // Vector direction between player and mouse position
    private Vector3 xyForward;      // The player's forward direction in XY screen space

    private Transform myTransform;
    private CharacterController myController;
    private AudioSource myAudioSource;
    private LevelSpawner scriptSpawn;


	// Use this for initialization
	void Start () 
    {
        myTransform = this.transform;
        myController = GetComponent<CharacterController>();
        myAudioSource = GetComponent<AudioSource>();
        scriptSpawn = GameObject.FindWithTag("Creation").GetComponent<LevelSpawner>();

        originalForwardSpeed = forwardMoveSpeed;
        backwardMoveSpeed = forwardMoveSpeed + (scrollSpeed/2);
        slowdownDecrement = forwardMoveSpeed * slowdownPercentage;
	}
	
	// Update is called once per frame
	void Update () 
    {
        EndlessMovement();
        EightWayMovement();
        ThrowBeer();
        ThrowSoda();
        ApplyGravity();
        ApplyMovement();
	}

    private void EndlessMovement()
    {
        moveByThis = new Vector3(0.0f, 0.0f, scrollSpeed * Time.deltaTime);                      // Reset the Vector3 to scrolling speed
    }

    private void EightWayMovement()
    {
        if (Input.GetAxis(vertical) > 0)
        {
            moveByThis.z += Input.GetAxis(vertical) * forwardMoveSpeed * Time.deltaTime;
        }
        else if(Input.GetAxis(vertical) < 0)
        {
            moveByThis.z += Input.GetAxis(vertical) * backwardMoveSpeed * Time.deltaTime;
        }
        if (Input.GetAxis(horizontal) != 0)
        {
            moveByThis.x += Input.GetAxis(horizontal) * forwardMoveSpeed * Time.deltaTime;
        }
    }

    private void ThrowBeer()
    {
        if (Input.GetButtonDown(beer))                                                                                                     // If we are pressing the fire beer button
        {
            if (ammoBeer > 0)
            {
                Vector3 mouseWorldPosition = MousePositionToWorldPosition();                                            // Get the mouse's position in the world by a raycast through camera z-axis
                dirToMouse = mouseWorldPosition - myTransform.position;                                                 // Get the direction to the mouse's world position
                //GameObject testClone = GameObject.Instantiate(goTest, mouseWorldPosition + new Vector3(0.0f, goTest.transform.localScale.y / 2, 0.0f), Quaternion.identity) as GameObject;  // DBGR
                //print("Direction to Mouse" + dirToMouse);   // DBGR
                angleBetween = Vector3.Angle(myTransform.forward, dirToMouse);                                          // Get the angle between the forward and mouse direction vectors
                float relativeX = dirToMouse.normalized.x;                                                              // The x variable of the direction will tell us if we are to the left or right of the transform.forward
                SpawnProjectile(goBeer, relativeX, mouseWorldPosition, beer);                                                               // Instantiate the beer at the correct spawning transform based on the angle and the x variable
            }
        }
    }

    private void ThrowSoda()
    {
        if (Input.GetButtonDown(soda))
        {
            if (ammoSoda > 0)
            {
                Vector3 mouseWorldPosition = MousePositionToWorldPosition();                                            // Get the mouse's position in the world by a raycast through camera z-axis
                //GameObject testClone = GameObject.Instantiate(goTest, mouseWorldPosition + new Vector3(0.0f, goTest.transform.localScale.y/2, 0.0f), Quaternion.identity) as GameObject;  // DBGR
                dirToMouse = mouseWorldPosition - myTransform.position;                                                 // Get the direction to the mouse's world position
                //print("Direction to Mouse" + dirToMouse);   // DBGR
                angleBetween = Vector3.Angle(myTransform.forward, dirToMouse);                                          // Get the angle between the forward and mouse direction vectors
                float relativeX = dirToMouse.normalized.x;                                                              // The x variable of the direction will tell us if we are to the left or right of the transform.forward
                SpawnProjectile(goSoda, relativeX, mouseWorldPosition, soda);                                                               // Instantiate the beer at the correct spawning transform based on the angle and the x variable
            }
        }
    }

    public Vector3 MousePositionToWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            return hit.point;
        }
        else
            return Vector3.zero;
    }

    private void SpawnProjectile(GameObject goProjectile, float mouseXPos, Vector3 target, string projectileType)
    {
        GameObject cloneProjectile;
        if (angleBetween <= 90.0f)                                                                                                      // If we are less than 90 degrees (in front)  Quadrants I and II
        {
            if (mouseXPos >= 0)                                                                                                         // If x is positive, then we are to the right or east
            {
                cloneProjectile = GameObject.Instantiate(goProjectile, beerNE.position, Quaternion.identity) as GameObject;
                //print("North East"); // DBGR
            }
            else                                                                                                                        // Else x is negative and we are to the left or west
            {
                cloneProjectile = GameObject.Instantiate(goProjectile, beerNW.position, Quaternion.identity) as GameObject;
                //print("North West"); // DBGR
            }
        }
        else                                                                                                                            // Else we are more than 90 (in back) Quadrants III and IV
        {
            if (mouseXPos >= 0)                                                                                                         // If x is positive, then we are to the right or east
            {
                cloneProjectile = GameObject.Instantiate(goProjectile, beerSE.position, Quaternion.identity) as GameObject;
                //print("South East"); // DBGR
            }
            else                                                                                                                        // Else x is negative and we are to the left or west
            {
                cloneProjectile = GameObject.Instantiate(goProjectile, beerSW.position, Quaternion.identity) as GameObject;
                //print("South West"); // DBGR
            }
        }
        if (projectileType == soda)
        {
            cloneProjectile.GetComponent<SodaProjectile>().target = target;
            ammoSoda--;
        }
        else if (projectileType == beer)
        {
            cloneProjectile.GetComponent<BeerProjectile>().xProjectileTarget = target;                                                                       // Tell the beer where to move towards
            ammoBeer--;                                                                                                                     // Subtract one from our available beers
        }
    }

    private void ApplyGravity()
    {
        if (!touchingGround)
        {
            moveByThis += Physics.gravity;
        }
    }

    private void ApplyMovement()
    {
        float testX = Camera.main.WorldToScreenPoint(myTransform.position + moveByThis).x;
        float testY = Camera.main.WorldToScreenPoint(myTransform.position + moveByThis).y;

        if (testX >= Screen.width || testX <= 0.0f || testY >= Screen.height || testY <= 0.0f)
        {
            moveByThis = new Vector3(0.0f, 0.0f, scrollSpeed * Time.deltaTime);                      // Reset the Vector3 to scrolling speed
        }
            //myTransform.Translate(myTransform.InverseTransformDirection( moveByThis));
        myController.Move(moveByThis);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            touchingGround = true;
        }
        else if (other.tag == "Spawn")
        {
            scriptSpawn.StartCoroutine(scriptSpawn.CreateNextTile());
        }
        else if (other.tag == "AmmoBeer")
        {
            PlaySound(playersounds[1], Random.Range(0.5f, 1.5f));
            ammoBeer += 6;
            GameObject.Destroy(other.gameObject);
        }
        else if (other.tag == "AmmoSoda")
        {
            PlaySound(playersounds[1], Random.Range(0.5f, 1.5f));
            ammoSoda += 6;
            GameObject.Destroy(other.gameObject);
        }
        else if (other.tag == adultWantsBeer || other.tag == childWantsSoda)
        {
            StartCoroutine(SlowPlayer());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            touchingGround = false;
        }
    }

    private IEnumerator SlowPlayer()
    {
        PlaySound(playersounds[0], 1.0f);
        if (forwardMoveSpeed > forwardMoveSpeed * 0.2)
        {
            forwardMoveSpeed -= slowdownDecrement;
            if (forwardMoveSpeed > 0)
                backwardMoveSpeed = forwardMoveSpeed + (scrollSpeed / 2);
            else
                backwardMoveSpeed = 0;
        }

        yield return new WaitForSeconds(slowdownDuration);

        forwardMoveSpeed += slowdownDecrement;
        backwardMoveSpeed = forwardMoveSpeed + (scrollSpeed / 2);
    }

    private void PlaySound(AudioClip clipToPlay, float pitch)
    {
        myAudioSource.clip = clipToPlay;
        myAudioSource.pitch = pitch;
        myAudioSource.Play();
    }
}
