using UnityEngine;
using System.Collections;

public class BeerProjectile : MonoBehaviour {

    public float moveSpeed = 25.0f;
    public float rotationsPerSecond = 2.0f;
    public GameObject bubbles;
    public Vector3 xProjectileTarget;

    private bool reachedTarget = false;
    private float angle;
    private float scrollSpeed;
    private float distance;
    private float oneAndOneFifthDistance;
    private float arcHeight;
    private float xEquation = 0.0f;
    private string adultWantsBeer = "AdultWantsBeer";
    private string childWantsSoda = "ChildWantsSoda";
    private string player = "Player";
    private string spawn = "Spawn";
    private Vector3 direction;
    private Transform myTransform;
    private Rigidbody myRigidbody;
    private CapsuleCollider myCollider;
    private AudioSource myAudioSource;

	// Use this for initialization
    void Start()
    {
        // Get the scroll speed from the player
        scrollSpeed = GameObject.FindWithTag("Player").GetComponent<PlayerInput>().scrollSpeed;

        //Assign local variables
        myTransform = this.transform;
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();
        myAudioSource = GetComponent<AudioSource>();

        xProjectileTarget.y = myTransform.position.y;
        distance = Vector3.Distance(myTransform.position, xProjectileTarget);
        oneAndOneFifthDistance = distance * 1.2f;
        arcHeight = xProjectileTarget.y * 2.0f;
        direction = (xProjectileTarget - myTransform.position).normalized;

        myTransform.rotation = Quaternion.LookRotation(direction);          // Face towards where we are throwing

        //angle = Mathf.Atan2(direction.z, direction.x);                                                                                  // Get the angle of the direction to the xProjectileTarget.  This is what we need the addition of our two vectors to match
        //direction = new Vector3(moveSpeed * Mathf.Cos(angle), 0.0f, moveSpeed * Mathf.Sin(angle) - scrollSpeed).normalized;                        // Find the new direction that equals the angle and includes the scroll speed.
    }

    // Update is called once per frame
    void Update()
    {
        OffscreenDestroy();
        if (!reachedTarget)
        {
            myTransform.Rotate(new Vector3(rotationsPerSecond * 6.0f, 0.0f, 0.0f));
            myTransform.Translate(myTransform.InverseTransformDirection(direction * moveSpeed * Time.deltaTime));
            myTransform.YPosition((arcHeight * (Mathf.Sin(((Mathf.PI / oneAndOneFifthDistance) * xEquation) + (Mathf.PI / 6.0f)))));      // y = arcHeight * sin((PI/distance * 1.2f) * x  + (PI/6.0f))  This ensures that we start 3/4 of the way up the ascent and land on the ground before it smoothes out in the descent.` 
            xEquation += moveSpeed * Time.deltaTime;
        }
    }

    void OffscreenDestroy()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(myTransform.position);
        if (screenPos.x > Screen.width || screenPos.x < 0 || screenPos.y > Screen.height || screenPos.y < 0)
            GameObject.Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == adultWantsBeer)
        {
            Scorekeeper.playerScore++;
            //print(Scorekeeper.playerScore);
            GameObject.Destroy(this.gameObject);
        }
        else if(other.tag == childWantsSoda)
        {
            Scorekeeper.playerScore--;
            //print(Scorekeeper.playerScore);
            GameObject.Destroy(this.gameObject);
        }
        else if (other.tag != player && other.tag != spawn)
        {
            // Let the bottle fall to the ground
            myCollider.isTrigger = false;
            myRigidbody.useGravity = true;

            // Change animation to broken bottle

            // Spawn bubbles
            GameObject bubbleClone = GameObject.Instantiate(bubbles, myTransform.position, bubbles.transform.rotation) as GameObject;

            // Play breaking sound
            myAudioSource.pitch = Random.Range(0.75f, 1.25f);
            myAudioSource.Play();

            // Stop movement
            reachedTarget = true;
        }
    }
}
