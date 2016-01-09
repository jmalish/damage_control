using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public int speed = 50;

	// Use this for initialization
	void Update () {

        GetComponent<Rigidbody2D>().AddForce(transform.up * speed);
    }
	
	void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
