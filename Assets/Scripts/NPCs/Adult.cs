using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Adult : MonoBehaviour {

    public AudioClip[] myClips;
    public GameObject goScore;

    private Dictionary<string, int> dictSound = new Dictionary<string, int>();
    private Vector3 screenPos;
    private bool haveBeenHit = false;
    private bool thirsty = true;
    private Vector3 scoreLocalPosition = new Vector3(0.0f, 11.33743f, 0.0f);
    private SpriteRenderer mySpriteRenderer;
    private Animator myAnimator;
    private AudioSource myAudioSource;
    private Transform parentTransform;
    private Transform myTransform;
    private PlayerInput scriptInput;

	// Use this for initialization
	void Start () 
    {
        dictSound.Add("Eww", 0);
        dictSound.Add("Gulp", 1);
        dictSound.Add("Ow", 2);

        myTransform = this.transform;
        parentTransform = myTransform.parent.transform;
        mySpriteRenderer = parentTransform.GetComponent<SpriteRenderer>();
        myAnimator = parentTransform.GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();

        scriptInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();

        parentTransform.rotation = Camera.main.transform.rotation;
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
            if (other.tag == "Beer" && other.gameObject.GetComponent<BeerProjectile>().reachedTarget != true)
            {
                myAnimator.SetBool("Beer", true);
                PlaySound("Gulp");
                SpawnScore(Scorekeeper.rewardAdultGetsBeer * Scorekeeper.comboMultiplier, Scorekeeper.addColor);
                thirsty = false;
            }
            else if (other.tag == "Soda" && other.gameObject.GetComponent<SodaProjectile>().reachedTarget != true)
            {
                myAnimator.SetBool("Soda", true);
                PlaySound("Eww");
                SpawnScore(Scorekeeper.penaltyAdultGetsSoda * Scorekeeper.comboMultiplier, Scorekeeper.minusColor);
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
        scoreClone.GetComponent<Animator>().SetBool("IsDude", true);
    }
}
