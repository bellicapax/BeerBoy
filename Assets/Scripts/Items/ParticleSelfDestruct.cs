using UnityEngine;
using System.Collections;

public class ParticleSelfDestruct : MonoBehaviour {

    private ParticleSystem particleSystem;
    private Transform myTransform;
    private Vector3 screenPos;

	// Use this for initialization
	void Start () 
    {
        particleSystem = GetComponent<ParticleSystem>();
        myTransform = this.transform;
	}

    void LateUpdate()
    {
        if (particleSystem != null && particleSystem.particleCount == 0)
            Destroy(gameObject);

        screenPos = Camera.main.WorldToScreenPoint(myTransform.position);
        if (screenPos.x < 0 || screenPos.y < 0)
            GameObject.Destroy(this.gameObject);
    }
}
