using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    // ship variables
    public float health = 100;
    public float accelSpeed = 40;
    public float turnSpeed = 10;
    public GameObject attackStage1;
    public GameObject attackStage2;
    public GameObject attackStage3;
    public GameObject attackStage4;

    // UI Variables
    public Text scoreboard_health;
    public Text scoreboard_damage;
    public Image healthbar;
    float healthBarScaleY, attackTime;
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
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        string collTag = coll.gameObject.tag.ToLower();

        if (collTag.Contains("health") || collTag.Contains("medic"))
        {
            HitByWeapon(-.5f);
        }
        else if (collTag.Contains("dead")) { } // do nothing
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
        LineRenderer laser = GetComponentInChildren<LineRenderer>();

        if (health >= 75)  // if health is between 75 and 100
        {
            laser.enabled = false;
            Vector3 localOffset = new Vector3(0, 2.25f, 0);
            Vector3 worldOffset = transform.rotation * localOffset;
            Vector3 spawnPos = transform.position + worldOffset;

            Instantiate(attackStage1, spawnPos, transform.rotation);  // spawn bullet

            attackTime = Time.time + attackRepeatTime;  // reset attack timer
        }
        else if ((health >= 50) && (health < 75)) // if health is between 50 and 75
        {
            laser.enabled = false;

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
        }
        else if ((health >= 25) && (health < 50)) // if health is between 25 and 50
        {
            laser.enabled = true;
        }
        else if ((health >= 00) && (health < 25)) // if health is between 00 and 25
        {
            laser.enabled = false;
        }
    }
}