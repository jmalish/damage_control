﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    // ship variables
    public static float health = 100;
    public float accelSpeed = 40;
    public float turnSpeed = 10;
    public float attackRepeatTime = .4f;
    public GameObject attackStage1, attackStage2, attackStage4, explosion;

    // UI Variables
    public Text scoreboardHealth, explosionImminent;
    public Image healthbar, explosionCircle;
    float healthBarScaleY, attackTime;
    public static bool gameOver = false;
    public GameObject storyScript;

    // other variables
    LineRenderer laser;
    bool exploding = false;
    public static bool tutorial = true;
    public static bool canShoot = true;
    float distanceFromStart;
    bool warned = false;
    bool selfDestructing = false;
    bool weaponsExplained = false;


    void Start()
    {
        Time.timeScale = 1;
        healthBarScaleY = healthbar.rectTransform.transform.localScale.y;
        laser = gameObject.GetComponent<LineRenderer>();

        explosionImminent.enabled = false;
        explosionCircle.enabled = false;

        StoryManagerScript.storyStage = 1;
    }

    void FixedUpdate()
    {
        #region Movement
        float accel = Input.GetAxis("Vertical");
        if (accel > 0)
        {
            GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * accelSpeed * accel);
        }
        else if (accel < 0)
        {
            GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * accelSpeed * accel / 2);
        }

        float turn = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().transform.Rotate(Vector3.back * turn * turnSpeed);
        #endregion movement

        #region Weapons
        if (health < 0)
        {
            StartCoroutine("EnableExplosionCircle");
            StartCoroutine("ExplosionCircleIncreaseAlpha");
        }
        else if (health <= 5)
        {
            if (!exploding)
            {
                StartCoroutine("NuclearExplosion");
                StartCoroutine("FlashImminentText");
            }
        }
        else if ((Input.GetButton("Fire1")) && (Time.time > attackTime))
        {
            FireWeapon();
        }
        else
        {
            laser.enabled = false;
        }

        if (health > 5 && exploding)
        {
            StopCoroutine("NuclearExplosion");
            StopCoroutine("FlashImminentText");
            exploding = false;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);  // fades ship back to regular color
            explosionImminent.enabled = false;
        }
        #endregion weapons

        #region dialogue/story
        if (ScoreManager.score > 4 && tutorial)
        {
            StoryManagerScript.storyStage = 5;
            tutorial = false; // tutorial is complete
            SpawnManagerScript.canSpawn = true;
        }
        else if (health < 95 && tutorial && !gameOver)
        {
            StoryManagerScript.storyStage = 2;
        }
        else if (health < 95 && !weaponsExplained)
        {
            StoryManagerScript.storyStage = 6;
            weaponsExplained = true;
        }
        

        distanceFromStart = Vector3.Distance(new Vector3(0,0), transform.position);
        if (distanceFromStart > 35 && distanceFromStart < 50 && tutorial)
        {
            if (!warned)
            {
                // player not done with tutorial yet, yell at them
                StoryManagerScript.storyStage = 3;
                warned = true;
            }
        }
        else if (distanceFromStart > 50 && tutorial && !selfDestructing)
        {
            // that jerk is stealing our ship, self destruct
            StartCoroutine(SelfDestruct());
        }
        #endregion dialogue/story
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
            gameOver = true;
            health = 0;
            ForceEnemiesToFlee();
            StartCoroutine(EnableExplosionCircle());
            StartCoroutine(ExplosionCircleIncreaseAlpha());

            StoryManagerScript.storyStage = 7;
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
        if (!canShoot)
        {
            return;
        }

        if (health >= 95)
        {
            #region force field
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
                hit.rigidbody.AddForceAtPosition(transform.up * 5, hit.point); // push object that was hit
            }
            #endregion
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
        else if ((health > 0) && (health < 25)) // if health is between 00 and 25
        {
            #region plasma ball
            attackRepeatTime = 1;

            Vector3 localOffset = new Vector3(0, 3f, 0);
            Vector3 worldOffset = transform.rotation * localOffset;
            Vector3 spawnPos = transform.position + worldOffset;

            Instantiate(attackStage4, spawnPos, transform.rotation);  // spawn bullet

            attackTime = Time.time + attackRepeatTime;  // reset attack timer

            TakeDamage(-5);
            #endregion
        }
    }

    IEnumerator FlashImminentText()
    {
        float alphaValue = 0;
        bool rising = true;

        explosionImminent.enabled = true;

        while (true)
        {
            while (rising)
            {
                alphaValue += .015f;
                explosionImminent.color = new Color(1, 0, 0, alphaValue);
                yield return new WaitForSeconds(.005f);

                if (alphaValue >= 1)
                {
                    rising = false;
                }
            }

            while (alphaValue > 0)
            {
                alphaValue -= .015f;
                explosionImminent.color = new Color(1, 0, 0, alphaValue);
                yield return new WaitForSeconds(.005f);

                if (alphaValue <= 0)
                {
                    rising = true;
                }
            }
        }
    }

    IEnumerator NuclearExplosion()
    {
        exploding = true;
        for (float colorValue = 1; colorValue > 0; colorValue -= .025f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, colorValue, colorValue, 1);  // fades ship to red
            yield return new WaitForSeconds(.05f);
        }
    }

    IEnumerator EnableExplosionCircle()
    {
        explosionCircle.enabled = true;

        for (float size = .1f; size < 15; size += .125f)
        {
            explosionCircle.transform.localScale = new Vector3(size, size);
            yield return new WaitForSeconds(.02f);
        }
    }

    IEnumerator ExplosionCircleIncreaseAlpha()
    {
        for (float colorValue = 0; colorValue < 1; colorValue += .035f)
        {
            explosionCircle.GetComponent<Image>().color = new Color(1, 1, 1, colorValue);  // brings circle to solid color
            yield return new WaitForSeconds(.05f);
        }

        yield return new WaitForSeconds(.75f);


        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in allEnemies)
        {
            enemy.SendMessage("TakeDamage", 999);  // kill all enemies still alive
        }

        gameOver = true;
}

    IEnumerator SelfDestruct()
    {
        selfDestructing = true;
        StoryManagerScript.storyStage = 4;

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
        GameObject createdExplosion = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
        Destroy(createdExplosion, .25f);
    }

    void ForceEnemiesToFlee()  // tells enemies that we're about to explode, they should probably run
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in allEnemies)
        {
            enemy.SendMessage("Flee");
        }

        Time.timeScale = .5f;
    }
}