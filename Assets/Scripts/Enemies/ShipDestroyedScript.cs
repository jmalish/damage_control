using UnityEngine;
using System.Collections;

public class ShipDestroyedScript : MonoBehaviour {
    float newAlphaValue = 1.0f;

    void Start()
    {

    }
   
    void FixedUpdate()
    {
        Component[] rigidBodies = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D rb in rigidBodies)
        {
            rb.AddForce(transform.up * .5f);  // update position
        }

        if (newAlphaValue > 0)
        {
            newAlphaValue -= .015f;

            Component[] renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in renderers)
            {
                sr.color = new Color(1,1,1,newAlphaValue);  // fades enemy out after death
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
