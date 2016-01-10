using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public int speed = 50;  // how fast the bullet is
    public int damage = 1;  // how much damage the bullet deals

	// Use this for initialization
	void Update () {
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed);  // update bullets position
    }
	
	void OnBecameInvisible()  // called when bullet goes off screen
    {
        Destroy(gameObject);  // delete bullet
    }

    void OnCollisionEnter2D(Collision2D coll)  // when bullet hits something
    {
        coll.gameObject.SendMessage("HitByWeapon", damage); // tell item that was collided that we want to deal damage
        Destroy(gameObject);  // delete bullet
    }
}
