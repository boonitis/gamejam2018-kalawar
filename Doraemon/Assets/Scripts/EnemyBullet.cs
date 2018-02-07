using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float speed; //the enemy bullet speed
    Vector2 _direction; //the direction of the bullet
    bool isReady; //to know when the bullet direction is set

    //set default values in Awake function
    private void Awake()
    {
        isReady = false;
    }

    // Use this for initialization
    void Start()
    {

    }


    //function to set the bullet direction
    public void SetDirection(Vector2 direction)
    {
        //set the direction normalized, to get an unit vector
        _direction = direction.normalized;

        isReady = true;//set flag to true
    }


    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            //get the bullet's current position
            Vector2 position = transform.position;

            //compute the bullet's new position
            position += _direction * speed * Time.deltaTime;

            //update the bullet's position
            transform.position = position;

            //Next we need to remove the bullet from our game
            //if the bullet goes outside the screen

            //this is the bottom-left point of the screen
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

            //this is the top-right point of the screen
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            //if the bullet goes outside the screen then destroy it
            if ((transform.position.x < min.x) || (transform.position.x > max.x) || (transform.position.y < min.y) || (transform.position.y > max.y))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Detect collision with player character
        if (col.tag == "PlayerCharacterTag")
        {
            Destroy(gameObject); //Destroy this character
        }
    }
}
