﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy_SmallScript : MonoBehaviour {

    public float speed = 20;
    public float health = 4;
    public GameObject weapon, destroyedShip, player;

    float distanceFromPlayer, attackTime;
    float distanceToAttack = 25;
    float attackRepeatTime = .5f;

    void FixedUpdate()
    {
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        
        #region movement
        if (distanceFromPlayer > 40)
        {
            Destroy(gameObject);  // enemy is too far away, despawn them
        }
        else if (distanceFromPlayer > 10) { 
            float zPos = Mathf.Atan2((player.transform.position.y - transform.position.y), player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, zPos);  // turn towards player

            GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed);  // move towards player
        }
        else if ((distanceFromPlayer > 0) && (distanceFromPlayer <= 7))
        {
            float zPos = Mathf.Atan2((player.transform.position.y - transform.position.y), player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, zPos);  // turn towards player

            GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed * -.5f);  // move towards player
        }
        else
        {
            float zPos = Mathf.Atan2((player.transform.position.y - transform.position.y), player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, zPos);  // turn towards player
        }
        #endregion movement

        // make sure enemy is close enough, and keep it from shooting a billion bullets at once
        if ((distanceFromPlayer < distanceToAttack) && (Time.time > attackTime))
        {
            FireWeapon();
            attackTime = Time.time + attackRepeatTime;  // reset attack timer
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;  // health equals health minus damage received 

        if (health <= 0)
        {
//            var offsetPos = new Vector3(transform.position.x + .21f, transform.position.y + .29f);
            Instantiate(destroyedShip, gameObject.transform.position, gameObject.transform.rotation);  // spawn 
            Destroy(gameObject);  // if health is less than or equal to 0, it's dead
        }
    }

    void FireWeapon()
    {
        Vector3 localOffset = new Vector3(0, 2, 0);
        Vector3 worldOffset = transform.rotation * localOffset;
        Vector3 spawnPos = transform.position + worldOffset;

        Instantiate(weapon, spawnPos, transform.rotation);  // spawn bullet
    }
}