using UnityEngine;
using System.Collections;

public class PlayerMobility : MonoBehaviour {

    public float accelSpeed;
    //public float turnSpeed;
    public GameObject bullet;

    void FixedUpdate()
    {
        var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion rot = Quaternion.LookRotation(transform.position - mousepos, Vector3.forward);

        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

        float accel = Input.GetAxis("Vertical");
        GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * accelSpeed * accel);

        //float turn = Input.GetAxis("Horizontal"); 
        //GetComponent<Rigidbody2D>().transform.Rotate(Vector3.back * turn * turnSpeed);  // if we want to turn ship with a and d versus point towards mouse

        if (Input.GetKeyDown("space"))
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }
}