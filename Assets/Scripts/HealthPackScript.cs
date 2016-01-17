using UnityEngine;
using System.Collections;

public class HealthPackScript : MonoBehaviour {
    public float healthGiven = 10;

    void OnCollisionEnter2D(Collision2D coll)  // when bullet hits something
    {
        var collTage = coll.gameObject.tag.ToLower();
        if (collTage.Contains("player") && !collTage.Contains("projectile"))
        {
            coll.gameObject.SendMessage("TakeDamage", -healthGiven); // tell item that was collided that we want to deal damage
            Destroy(gameObject);
        }
    }
}
