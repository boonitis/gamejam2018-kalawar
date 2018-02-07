using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    GameObject scoreUITextGO;

    public GameObject DamagedEffectGO;
    public GameObject ExplosionGO;
    public GameObject[] CollectablesGO; //list of collectables drop from this unit, null equal no drop! 

    public float speed; //unit movement speed
    public int scorePoints; //unit score points

    public int MaxLives; //maximum unit Lives
    int Lives; //current unit Lives

    // Use this for initialization
    void Start()
    {
        Lives = MaxLives;
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }

    // Update is called once per frame
    void Update()
    {
        //Get the enemy current position
        Vector2 position = transform.position;

        //Compute the enemy position
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);

        //Update the enemy position
        transform.position = position;

        //this is the bottom-left point of screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        //if the enemy went outside the screen on the bottom, then destroy the enemy
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Detect collision of the enemy character with player character, or with player bullet
        if ((col.tag == "PlayerCharacterTag") || (col.tag == "PlayerBulletTag"))
        {
            //If collide with player, deal player on hit damage; if not, receive 'x' damage
            Lives -= col.tag == "PlayerCharacterTag" ? 10 : col.GetComponent<PlayerBullet>().Damage;

            StartCoroutine("Flasher");//Blink unit on damaged
            PlayExplosion();//Create damaged or explosion effect

            if (Lives <= 0)//If unit is dead
            {
                //Add 'n' points multiply with ScoreMultiplier to the GameScore
                scoreUITextGO.GetComponent<GameScore>().Score += (int)(scorePoints * PlayerControl.ScoreMultiplier);

                //Increase score multiplier calculate from enemy max Lives
                PlayerControl.ScoreMultiplier += scorePoints * 0.001f;

                //Drop collectable if available
                if (CollectablesGO.Length != 0)
                    DropCollectable();
                //Destroy this character
                Destroy(gameObject);
            }
        }
    }

    public void PlayExplosion()
    {
        GameObject explosion;

        if (Lives <= 0)
            explosion = (GameObject)Instantiate(ExplosionGO);
        else
            explosion = (GameObject)Instantiate(DamagedEffectGO);

        //set the explosion positon to this object position
        explosion.transform.position = transform.position;
    }

    // Functions to be used as Coroutines MUST return an IEnumerator
    IEnumerator Flasher()
    {
        for (int i = 0; i < 2; i++)
        {
            GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(.1f);
            GetComponent<Renderer>().material.color = Color.white;
            yield return new WaitForSeconds(.1f);
        }
    }

    //Function to generate collectable at this unit position
    public void DropCollectable()
    {
        GameObject collectable;

        //Randomly pick one collectable index from the list, or not
        int collectableIndex = Random.Range(0, CollectablesGO.Length);

        if (CollectablesGO[collectableIndex] != null)
        {
            //instatiate collectable
            collectable = (GameObject)Instantiate(CollectablesGO[collectableIndex]);
            //set the collectable positon to this object position
            collectable.transform.position = transform.position;
        }

    }



}
