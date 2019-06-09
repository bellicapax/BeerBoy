using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Child : MonoBehaviour {

    public GameObject goPolice;
    public GameObject goScore;
    public AudioClip[] myClips;
    public float amountOffscreenY = -100.0f;
    public float amountOffscreenX = -50.0f;

    private bool thirsty = true;

    private Vector3 offScreen = new Vector3(0.0f, -1000.0f, 0.0f);
    private Vector3 screenPos;
    private Vector3 direction;
    private Vector3 scoreLocalPosition = new Vector3(-1.211898f, 7.492757f, 0.0f);

    private Dictionary<string, int> dictSound = new Dictionary<string, int>();
    private SpriteRenderer mySpriteRenderer;
    private Animator myAnimator;
    private Transform parentTransform;
    private Transform myTransform;
    private Transform cameraTransform;
    private Transform playerTransform;
    private AudioSource myAudioSource;
    private PlayerInput scriptInput;

    // Use this for initialization
    void Start()
    {
        dictSound.Add("Siren", 0);
        dictSound.Add("MMM", 1);
        dictSound.Add("Hit", 2);

        myTransform = this.transform;
        parentTransform = myTransform.parent.transform;
        cameraTransform = Camera.main.transform;
        playerTransform = GameObject.FindWithTag("Player").transform;

        mySpriteRenderer = parentTransform.GetComponent<SpriteRenderer>();
        myAnimator = parentTransform.GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();

        scriptInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();

        parentTransform.rotation = Camera.main.transform.rotation;      // Face the camera
    }

    void Update()
    {
        OffScreenDestroyAndUpdateRefreshment();
    }

    void OffScreenDestroyAndUpdateRefreshment()
    {
        if (!myAudioSource.isPlaying)
        {
            screenPos = Camera.main.WorldToScreenPoint(myTransform.position);
            if (screenPos.x < 0 || screenPos.y < 0)
            {
                if (thirsty)
                    Scorekeeper.popularityMeter -= Scorekeeper.penaltyDidntGetRefreshment;
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (thirsty)
        {
            if (other.tag == "Soda" && other.gameObject.GetComponent<SodaProjectile>().reachedTarget != true)
            {
                myAnimator.SetBool("Soda", true);
                myAudioSource.clip = myClips[1];
                myAudioSource.pitch = Random.Range(0.95f, 1.05f);
                myAudioSource.Play();
                SpawnScore(Scorekeeper.rewardChildGetsSoda * Scorekeeper.comboMultiplier, Scorekeeper.addColor);
                thirsty = false;
            }
            else if (other.tag == "Beer" && other.gameObject.GetComponent<BeerProjectile>().reachedTarget != true)
            {
                myAnimator.SetBool("Beer", true);
                PlaySound("Siren");
                GameObject policeClone = GameObject.Instantiate(goPolice, offScreen, goPolice.transform.rotation) as GameObject;
                SpawnScore(Scorekeeper.penaltyChildGetsBeer * Scorekeeper.comboMultiplier, Scorekeeper.minusColor);
                thirsty = false;
            }
        }
    }

    public void PlaySound(string clipName)
    {
        myAudioSource.Stop();
        myAudioSource.clip = myClips[dictSound[clipName]];
        myAudioSource.pitch = Random.Range(0.9f, 1.1f);
        myAudioSource.Play();
    }

    public void SpawnScore(int displayedNumber, Color addOrMinus)
    {
        GameObject scoreClone = GameObject.Instantiate(goScore, Vector3.zero, parentTransform.rotation) as GameObject;
        scoreClone.SetActive(false);
        scoreClone.transform.parent = myTransform.parent;
        scoreClone.transform.localPosition = scoreLocalPosition;
        if (addOrMinus == Scorekeeper.addColor)
        {
            scoreClone.GetComponent<TextMesh>().text = "+" + displayedNumber.ToString();
        }
        else
        {
            scoreClone.GetComponent<TextMesh>().text = "-" + displayedNumber.ToString();
        }
        scoreClone.GetComponent<TextMesh>().color = addOrMinus;
        scoreClone.SetActive(true);
        scoreClone.GetComponent<Animator>().SetBool("IsGirl", true);
    }

}
