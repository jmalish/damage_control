﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    // ship variables
    public float health = 100;
    public float accelSpeed = 40;
    public float turnSpeed = 10;
    public GameObject bullet;

    // UI Variables
    public Text scoreboard_health;
    public Text scoreboard_damage;
    public Image healthbar;
    float healthBarScaleY, attackTime;
    float distanceToAttack = 25;
    public float attackRepeatTime = .25f;


    void Start()
    {
        healthBarScaleY = healthbar.rectTransform.transform.localScale.y;
    }

    void Update()
    {
        // Movement
        float accel = Input.GetAxis("Vertical");
        if (accel > 0)
        {
            GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * accelSpeed * accel);
        }
        else if (accel < 0)
        {
            GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * accelSpeed * accel/2);
        }

        float turn = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().transform.Rotate(Vector3.back * turn * turnSpeed);  // if we want to turn ship with a and d versus point towards mouse

        // Weapons
        if ((Input.GetButton("Fire1")) && (Time.time > attackTime))
        {
            FireWeapon();
            attackTime = Time.time + attackRepeatTime;  // reset attack timer
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        string collTag = coll.gameObject.tag.ToLower();

        if (collTag.Contains("health") || collTag.Contains("medic"))
        {
            HitByWeapon(-.5f);
        }
        else
        {
            HitByWeapon(.2f);
        }
        
    }

    void HitByWeapon(float damage)
    {
        health -= damage;  // health equals health minus damage received

        if (health <= 0)
        {
            Destroy(gameObject);  // if health is less than or equal to 0, it's dead
        } 
        else if (health >= 100)
        {
            health = 100;
        }

        //scoreboard_health.text = "Health: " + Mathf.Round(health).ToString();
        scoreboard_health.text = "Health: " + Mathf.Round(health).ToString();
        scoreboard_damage.text = "Damage: " + Mathf.Round(100 - health).ToString();

        healthbar.rectTransform.localScale = new Vector3(health / 100, healthBarScaleY);
    }

    void FireWeapon()
    {
        Vector3 localOffset = new Vector3(0, 2.25f, 0);
        Vector3 worldOffset = transform.rotation * localOffset;
        Vector3 spawnPos = transform.position + worldOffset;

        if (health >= 75)  // if health is between 75 and 100
        {
            Instantiate(bullet, spawnPos, transform.rotation);  // spawn bullet
        }
        else if ((health >= 50) && (health < 75)) // if health is between 50 and 75
        {

        }
        else if ((health >= 25) && (health < 50)) // if health is between 25 and 50
        {

        }
        else if ((health >= 00) && (health < 25)) // if health is between 00 and 25
        {

        }
    }
}