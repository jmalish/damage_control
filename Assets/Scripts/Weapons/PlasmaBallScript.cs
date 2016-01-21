using UnityEngine;
using System.Collections;

public class PlasmaBallScript : MonoBehaviour {

    public int speed = 200;  // how fast the bullet is
    public int damage = 20;  // how much damage the bullet deals
    public float initialSize = .05f;
    public float growthRate = .001f;
    float bulletLife = 10;   // life in seconds

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed);  // update bullets position
        Destroy(gameObject, bulletLife);  // delete bullet after time of life

        gameObject.transform.localScale += new Vector3(growthRate, growthRate, 0);
    }

    void OnCollisionEnter2D(Collision2D coll)  // when bullet hits something
    {
        try
        {
            coll.gameObject.SendMessage("TakeDamage", damage); // tell item that was collided that we want to deal damage
        }
        catch { }

        var collTag = coll.collider.tag.ToLower();
        if (!(collTag.Contains("projectile") || collTag.Contains("dead")))
        {
            Destroy(gameObject);
        }
    }
}
