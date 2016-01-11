using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    public float speed;
    public float health;
    public Transform player;
    float distanceFromPlayer;

    void FixedUpdate()
    {
        float z = Mathf.Atan2((player.transform.position.y - transform.position.y),
            player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;

        transform.eulerAngles = new Vector3(0, 0, z);

        GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed);

        distanceFromPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceFromPlayer > 40)
        {
            Destroy(gameObject);  // enemy is too far away, despawn them
        }

    }

    void HitByWeapon(int damage)
    {
        health -= damage;  // health equals health minus damage received 

        if (health <= 0)
        {
            Destroy(gameObject);  // if health is less than or equal to 0, it's dead
        }
    }
}