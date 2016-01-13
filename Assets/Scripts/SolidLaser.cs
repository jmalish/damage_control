using UnityEngine;
using System.Collections;

public class SolidLaser : MonoBehaviour {

    LineRenderer line;

	void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }
    }

    IEnumerator FireLaser()
    {
        line.enabled = true;
        
        while(Input.GetButton("Fire1"))
        {
            //line.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.time);  // makes laser "rotate", gives it movement

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 1000);


            if (hit)
            {
                line.SetPosition(0, transform.position);
                line.SetPosition(1, hit.point);
                
                try
                {
                    hit.collider.SendMessage("HitByWeapon", 1);
                    hit.rigidbody.AddForceAtPosition(transform.forward * 5, hit.point);
                }
                catch
                {
                    // do nothing
                }
                
            }
            else
            {
                line.SetPosition(0, transform.position);
                line.SetPosition(1, transform.position + (transform.forward * 1000));
            }

            yield return null;
        }

        line.enabled = false;
    }
}
