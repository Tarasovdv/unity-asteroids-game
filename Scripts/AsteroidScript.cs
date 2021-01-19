using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{

public float maxThrust;
public float maxTorque;

public float screenTop;
public float screenBottom;
public float screenLeft;
public float screenRight;
public int asteroidSize;
public GameObject asteroidMedium;
public GameObject asteroidSmall;
public GameObject player;

public GameManager gm;

public int points;

public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 thrust = new Vector2(Random.Range(-maxThrust,maxThrust),Random.Range(-maxThrust,maxThrust));
        float torque = Random.Range(-maxTorque,maxTorque);
        
        rb.AddForce(thrust);
        rb.AddTorque(torque);

        player = GameObject.FindWithTag("Player");

        gm = GameObject.FindObjectOfType<GameManager>();


    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = transform.position;
        if(transform.position.y > screenTop){
            newPos.y = screenBottom;
        }
        if(transform.position.y < screenBottom){
            newPos.y = screenTop;
        }
        if(transform.position.x > screenRight){
            newPos.x = screenLeft;
        }
        if(transform.position.x < screenLeft){
            newPos.x = screenRight;
        }

        transform.position = newPos;
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet"))
        {

           //Debug.Log("Hit by" + other.name);
            Destroy (other.gameObject); //destroy the bullet
            if (asteroidSize == 3)
            {
                Instantiate(asteroidMedium,transform.position,transform.rotation);
                Instantiate(asteroidMedium,transform.position,transform.rotation);

                gm.UpdateNumberOfAsteroids(1);
               
            }
            else if (asteroidSize == 2)
            {
                Instantiate(asteroidSmall,transform.position,transform.rotation);
                Instantiate(asteroidSmall,transform.position,transform.rotation);

                gm.UpdateNumberOfAsteroids(1);
                
            }
            else if (asteroidSize == 1)
            {
                gm.UpdateNumberOfAsteroids(-1);
                
            }

            player.SendMessage("ScorePoints",points);

             Destroy(gameObject);
  
        }
       
    }
}
