using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float health = 100;
    public float accelSpeed = 40;
    public float turnSpeed = 10;
    public GameObject bullet;

    void FixedUpdate()
    {
    //    var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Quaternion rot = Quaternion.LookRotation(transform.position - mousepos, Vector3.forward);
        //transform.rotation = rot;
        //transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

        float accel = Input.GetAxis("Vertical");
        GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * accelSpeed * accel);

        float turn = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().transform.Rotate(Vector3.back * turn * turnSpeed);  // if we want to turn ship with a and d versus point towards mouse
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 localOffset = new Vector3(0, 2, 0);
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

    void HitByWeapon(int damage)
    {
        health -= damage;  // health equals health minus damage received

        if (health <= 0)
        {
            Destroy(gameObject);  // if health is less than or equal to 0, it's dead
        }
    }
}