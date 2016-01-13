using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy_SmallScript : MonoBehaviour {

    public float speed = 20;
    public float health = 4;
    public Transform player;
    public GameObject weapon;

    float distanceFromPlayer, attackTime;
    float distanceToAttack = 25;
    float attackRepeatTime = .5f;
    
    void FixedUpdate()
    {
        float zPos = Mathf.Atan2((player.transform.position.y - transform.position.y),
            player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;

        transform.eulerAngles = new Vector3(0, 0, zPos);

        GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed);

        distanceFromPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceFromPlayer > 40)
        {
            Destroy(gameObject);  // enemy is too far away, despawn them
        }
        else if ((distanceFromPlayer < distanceToAttack) && (Time.time > attackTime))  // make sure enemy is close enough, and keep it from shooting a billion bullets at once
        {
            FireWeapon();
            attackTime = Time.time + attackRepeatTime;  // reset attack timer
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

    void FireWeapon()
    {
        Debug.Log("Firing");
        Vector3 localOffset = new Vector3(0, 2, 0);
        Vector3 worldOffset = transform.rotation * localOffset;
        Vector3 spawnPos = transform.position + worldOffset;

        Instantiate(weapon, spawnPos, transform.rotation);  // spawn bullet
    }
}