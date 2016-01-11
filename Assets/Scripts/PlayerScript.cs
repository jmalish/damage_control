using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    // ship variables
    public float health = 100;
    public float accelSpeed = 40;
    public float turnSpeed = 10;
    public GameObject bullet;

    // UI Variables
    public Text scoreboard_health;
    public Text scoreboard_damage;
    public RectTransform healthBar;
    private float healthBarLocationY, healthBarLocationX;


    void Start()
    {
        healthBarLocationY = healthBar.position.y;
        healthBarLocationX = healthBar.position.x;
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

    void OnCollisionStay2D(Collision2D coll)
    {
        HitByWeapon(.2f);
    }

    void HitByWeapon(float damage)
    {
        health -= damage;  // health equals health minus damage received

        if (health <= 0)
        {
            Destroy(gameObject);  // if health is less than or equal to 0, it's dead
        }

        scoreboard_health.text = "Health: " + Mathf.Round(health).ToString();
        scoreboard_damage.text = "Damage: " + Mathf.Round(100 - health).ToString();

        healthBar.position = new Vector3(healthBarLocationX - health, healthBarLocationY);
    }
}