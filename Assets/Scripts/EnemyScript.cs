using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    public float speed;
    public float health;
    public Transform player;
    public Text scoreboard;

    void FixedUpdate()
    {
        float z = Mathf.Atan2((player.transform.position.y - transform.position.y),
            player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;

        transform.eulerAngles = new Vector3(0, 0, z);

        GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed);

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
