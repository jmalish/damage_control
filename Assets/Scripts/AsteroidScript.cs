using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour {
    public float health = 5;
    public GameObject healthPack, destroyedAsteroid;
    GameObject player;
    float distanceFromPlayer;
    public float damage = 1;
    bool destroyed = false;
	
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

	void FixedUpdate () {
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        
        if (distanceFromPlayer > 40)
        {
            Destroy(gameObject);  // enemy is too far away, despawn them
        }
    }

    void OnCollisionEnter2D(Collision2D coll)  // when bullet hits something
    {
        coll.gameObject.SendMessage("TakeDamage", damage); // tell item that was collided that we want to deal damage
        TakeDamage(1);
    }

    void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 & !destroyed)
        {
            Destroy(gameObject);
            Instantiate(destroyedAsteroid, gameObject.transform.position, gameObject.transform.rotation);
            Instantiate(healthPack, gameObject.transform.position + new Vector3(0, -1.5f, 0), gameObject.transform.rotation);
            destroyed = true;
        }
    }
}
