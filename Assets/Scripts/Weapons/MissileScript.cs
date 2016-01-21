using UnityEngine;
using System.Collections;

public class MissileScript : MonoBehaviour {
    
    public GameObject player, explosion;
    public float speed = 25;
    public float damage = 10;
    float distanceFromPlayer, spawnTime;
    bool tracking = true;

    void Start()
    {
        spawnTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (Time.time - spawnTime < .75f)
        {
            GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed);  // move forwards
        }
        else if (distanceFromPlayer > 40)
        {
            Destroy(gameObject);  // missile is too far away, despawn them
        }
        else if (distanceFromPlayer > 5 && tracking)
        {
            float zPos = Mathf.Atan2((player.transform.position.y - transform.position.y), player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, zPos);  // turn towards player

            GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed);  // move towards player
        }
        else
        {
            tracking = false;

            GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed);  // move forwards
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Vector3 localOffset = new Vector3(0, .5f, 0);
        Vector3 worldOffset = transform.rotation * localOffset;
        Vector3 spawnPos = transform.position + worldOffset;

        Debug.Log("missile hit");
        coll.gameObject.SendMessage("TakeDamage", damage);
        Destroy(gameObject);
        GameObject createdExplosion = (GameObject)Instantiate(explosion, spawnPos, gameObject.transform.rotation);

        Destroy(createdExplosion, .25f);
    }
}
