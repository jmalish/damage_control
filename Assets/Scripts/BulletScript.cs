using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public int speed = 50;  // how fast the bullet is
    public int damage = 1;  // how much damage the bullet deals
    float bulletLife = 2;

	// Use this for initialization
	void Update ()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed);  // update bullets position
        Destroy(gameObject, bulletLife);  // delete bullet after time of life
    }

    void OnCollisionEnter2D(Collision2D coll)  // when bullet hits something
    {
        try
        {
            coll.gameObject.SendMessage("HitByWeapon", damage); // tell item that was collided that we want to deal damage
        } catch { }

        var collTag = coll.collider.tag.ToLower();
        if (!(collTag.Contains("projectile") || collTag.Contains("dead")))
        {   
            Destroy(gameObject);  // delete bullet
        }
    }
}
