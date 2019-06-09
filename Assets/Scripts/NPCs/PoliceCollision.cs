using UnityEngine;
using System.Collections;

public class PoliceCollision : MonoBehaviour {

    public float beerSlow = -1.0f;
    public float sodaFast = 2.0f;
    public GameObject goScore;

    public AudioClip gulpClip;
    public AudioClip breakClip;

    private Vector3 scoreLocalPosition = new Vector3(0.01093577f, 10.65613f, 0.0f);
    private Transform myTransform;
    private Policeman scriptPolice;
    private AudioSource mySource;

    void Start()
    {
        myTransform = this.transform;
        scriptPolice = this.transform.parent.GetComponent<Policeman>();
        mySource = this.transform.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Beer")
        {
            scriptPolice.copSpeed += beerSlow;
            mySource.clip = breakClip;
            mySource.Play();
            SpawnScore(Scorekeeper.penaltyDidntGetRefreshment, Scorekeeper.minusColor);
            Scorekeeper.popularityMeter -= Scorekeeper.penaltyDidntGetRefreshment;
        }
        
        else if (other.tag == "Soda")
        {
            scriptPolice.copSpeed += sodaFast;
            mySource.clip = gulpClip;
            mySource.Play();
        }
    }

    public void SpawnScore(int displayedNumber, Color addOrMinus)
    {
        GameObject scoreClone = GameObject.Instantiate(goScore, Vector3.zero, myTransform.parent.transform.rotation) as GameObject;
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
        scoreClone.GetComponent<Animator>().SetBool("IsPolice", true);
    }
}
