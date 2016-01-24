using UnityEngine;
using System.Collections;

public class Enemy_LargeScript : MonoBehaviour {

    public float speed = 10;
    public float health = 10;
    public int value = 100;
    public GameObject weapon, destroyedShip;
    GameObject player;

    float distanceFromPlayer, attackTime, flyAwayTime;
    float distanceToAttack = 20;
    float attackRepeatTime = 1;
    bool flyAway = false;
    bool fireLeft = true; // used to tell enemy which missile to shoot next
    public bool fleeing = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        #region movement
        if (!fleeing)
        {
            if (flyAway)
            {
                if (Time.time - flyAwayTime > 4)
                {
                    flyAway = false;
                    attackTime = Time.time + .3f;  // reset attack timer
                }
                else
                {
                    float zPos = Mathf.Atan2((player.transform.position.y - transform.position.y), player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
                    transform.eulerAngles = new Vector3(0, 0, -zPos);  // turn away from player

                    GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed);  // move away from player
                }
            }
            else if (distanceFromPlayer > 50)
            {
                Destroy(gameObject);  // enemy is too far away, despawn them
            }
            else if (distanceFromPlayer > 10)
            {
                float zPos = Mathf.Atan2((player.transform.position.y - transform.position.y), player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
                transform.eulerAngles = new Vector3(0, 0, zPos);  // turn towards player

                GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed);  // move towards player
            }
            else if (distanceFromPlayer <= 10)
            {
                flyAway = true;
                flyAwayTime = Time.time;
            }
        }
        else
        {
            flyAway = true;

            float zPos = Mathf.Atan2((player.transform.position.y - transform.position.y), player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, -zPos);  // turn towards player

            GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed);  // move towards player

            if (distanceFromPlayer > 40)
            {
                Destroy(gameObject);  // enemy is too far away, despawn them
            }
        }
        #endregion movement

        // make sure enemy is close enough, and keep it from shooting a billion bullets at once
        if ((distanceFromPlayer < distanceToAttack) && (Time.time > attackTime) && (!flyAway))
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
            Destroy(gameObject);  // if health is less than or equal to 0, it's dead
            Instantiate(destroyedShip, gameObject.transform.position, gameObject.transform.rotation);  // spawn
            ScoreManager.score += value;
        }
    }

    void FireWeapon()
    {
        if (fireLeft)
        {
            Vector3 localOffset = new Vector3(-2, 2, 0);
            Vector3 worldOffset = transform.rotation * localOffset;
            Vector3 spawnPos = transform.position + worldOffset;

            Instantiate(weapon, spawnPos, transform.rotation);  // spawn bullet
            fireLeft = false;
        }
        else
        {
            Vector3 localOffset = new Vector3(2, 2, 0);
            Vector3 worldOffset = transform.rotation * localOffset;
            Vector3 spawnPos = transform.position + worldOffset;

            Instantiate(weapon, spawnPos, transform.rotation);  // spawn bullet
            fireLeft = true;
        }
    }

    void Flee()
    {
        // speed = speed / 2;
        fleeing = true;
    }
}
