using UnityEngine;
using System.Collections;

public class OffscreenDestroy : MonoBehaviour {

    public bool isTile = true;

    private Transform myTransform;
    private Vector3 nextTilePlus = new Vector3(0.0f, 0.0f, 208.6048f);
    private Vector3 screenPos;

	// Use this for initialization
	void Start () 
    {
        myTransform = this.transform;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isTile)
        {
            screenPos = Camera.main.WorldToScreenPoint(myTransform.position + (nextTilePlus * 0.66f));
            if (screenPos.x < 0 || screenPos.y < 0)
                GameObject.Destroy(this.gameObject);
        }
        else
        {
            screenPos = Camera.main.WorldToScreenPoint(myTransform.position);
            if (screenPos.x < 0 || screenPos.y < 0)
                GameObject.Destroy(this.gameObject);
        }
	}
}
