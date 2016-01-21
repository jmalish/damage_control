using UnityEngine;
using System.Collections;

public class AsteroidDestroyedScript : MonoBehaviour {
    float newAlphaValue = 1.0f;

    void FixedUpdate()
    {
        if (newAlphaValue > 0)
        {
            newAlphaValue -= .012f;

            Component[] renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in renderers)
            {
                sr.color = new Color(1, 1, 1, newAlphaValue);  // fades enemy out after death
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
