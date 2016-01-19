﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    // ship variables
    public float health = 100;
    public float accelSpeed = 40;
    public float turnSpeed = 10;
    public float attackRepeatTime = .4f;
    public GameObject attackStage1, attackStage2, attackStage4;

    // UI Variables
    public Text scoreboardHealth, scoreboard_score;
    public Image healthbar;
    float healthBarScaleY, attackTime;
    
    // other variables
    LineRenderer laser;
    bool exploding = false;
    float colorValue = 1;


    void Start()
    {
        healthBarScaleY = healthbar.rectTransform.transform.localScale.y;
        laser = gameObject.GetComponent<LineRenderer>();
        //Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        //laser.material = whiteDiffuseMat;

    }

    void FixedUpdate()
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
        if (health <= 5)
        {
            if (colorValue > 0)
            {
                colorValue -= .005f;
                GetComponent<SpriteRenderer>().color = new Color(1, colorValue, colorValue, 1);  // fades ship to red
            }
        }
        else if ((Input.GetButton("Fire1")) && (Time.time > attackTime))
        {
            FireWeapon();
        } else
        {
            laser.enabled = false;
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        string collTag = coll.gameObject.tag.ToLower();

        if (collTag.Contains("dead") || collTag.Contains("health")) { } // do nothing
        else
        {
            TakeDamage(.2f);
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage;  // health equals health minus damage received

        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);  // if health is less than or equal to 0, it's dead
        } 
        else if (health >= 100)
        {
            health = 100;
        }

        //scoreboard_health.text = "Health: " + Mathf.Round(health).ToString();
        scoreboardHealth.text = "Health: " + Mathf.Round(health).ToString();

        healthbar.rectTransform.localScale = new Vector3(health / 100, healthBarScaleY);
    }

    void FireWeapon()
    {
        if (health >= 95)
        {
            laser.SetColors(new Color(1,1,1,.33f), new Color(1,1,1,0));

            laser.SetWidth(.01f, 5);
            //line.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.time);  // makes laser "rotate", gives it movement

            laser.enabled = true;

            Vector3 localOffset = new Vector3(0, 1.5f, 0);          // offset laser spawn
            Vector3 worldOffset = transform.rotation * localOffset; // ^
            Vector3 spawnPos = transform.position + worldOffset;    // ^

            RaycastHit2D hit = Physics2D.Raycast(spawnPos, transform.up, 10);  // create raycast

            laser.SetPosition(0, spawnPos);  // start of laser
            laser.SetPosition(1, spawnPos + (transform.up * 3));  // end of laser, since nothing is hit, just make it long enough to go offscreen

            if (hit)
            {
                try { hit.rigidbody.AddForceAtPosition(transform.up * 5, hit.point); } catch { } // push object that was hit
            }
        }
        else if ((health >= 75) && (health < 95))
        {
            #region single shot
            Vector3 localOffset = new Vector3(0, 2.25f, 0);
            Vector3 worldOffset = transform.rotation * localOffset;
            Vector3 spawnPos = transform.position + worldOffset;

            Instantiate(attackStage1, spawnPos, transform.rotation);  // spawn bullet

            attackTime = Time.time + attackRepeatTime;  // reset attack timer

            TakeDamage(-.5f);
            #endregion
        }
        else if ((health >= 50) && (health < 75)) // if health is between 50 and 75
        {
            #region Multi shot
            // multi shot
            Vector3 localOffset = new Vector3(-1.25f, .75f, 0);
            Vector3 worldOffset = transform.rotation * localOffset;
            Vector3 spawnPos = transform.position + worldOffset;

            Instantiate(attackStage2, spawnPos, transform.rotation);  // spawn left bullet

            localOffset = new Vector3(0, 2.25f, 0);
            worldOffset = transform.rotation * localOffset;
            spawnPos = transform.position + worldOffset;

            Instantiate(attackStage2, spawnPos, transform.rotation);  // spawn center bullet

            localOffset = new Vector3(1.25f, .75f, 0);
            worldOffset = transform.rotation * localOffset;
            spawnPos = transform.position + worldOffset;

            Instantiate(attackStage2, spawnPos, transform.rotation);  // spawn right bullet
            // multi shot

            attackTime = Time.time + attackRepeatTime;  // reset attack timer

            TakeDamage(-1.5f);
            #endregion
        }
        else if ((health >= 25) && (health < 50)) // if health is between 25 and 50
        {
            #region laser beam
            laser.SetColors(new Color(0, .25f, 1, 1), new Color(1, 1, 1, 1));
            laser.SetWidth(.15f, .15f);

            laser.enabled = true;
            
            Vector3 localOffset = new Vector3(0, 1.5f, 0);          // offset laser spawn
            Vector3 worldOffset = transform.rotation * localOffset; // ^
            Vector3 spawnPos = transform.position + worldOffset;    // ^

            RaycastHit2D hit = Physics2D.Raycast(spawnPos, transform.up, 50);  // create raycast

            if (hit)
            {
                laser.SetPosition(0, spawnPos);  // start of laser
                laser.SetPosition(1, hit.point);  // end of laser, ends where the raycast hits something

                try
                {
                    hit.collider.SendMessage("TakeDamage", 1, SendMessageOptions.RequireReceiver);  // let object know it's been hit, deal x damage
                    hit.rigidbody.AddForceAtPosition(transform.up * 5, hit.point);  // push object that was hit
                }
                catch { }
            }
            else
            {
                laser.SetPosition(0, spawnPos);  // start of laser
                laser.SetPosition(1, spawnPos + (transform.up * 50));  // end of laser, since nothing is hit, just make it long enough to go offscreen
            }

            TakeDamage(-.05f);
            #endregion
        }
        else if ((health >= 05) && (health < 25)) // if health is between 00 and 25
        {
            attackRepeatTime = 1;

            Vector3 localOffset = new Vector3(0, 2f, 0);
            Vector3 worldOffset = transform.rotation * localOffset;
            Vector3 spawnPos = transform.position + worldOffset;

            Instantiate(attackStage4, spawnPos, transform.rotation);  // spawn bullet

            attackTime = Time.time + attackRepeatTime;  // reset attack timer

            TakeDamage(-5);
        }
        else if ((health >= 00) && (health < 5)) // if health is between 00 and 25
        {
            // TODO: explosion
        }
    }
}