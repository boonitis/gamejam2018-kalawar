using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    GameObject playerCharacter;

    public GameObject CollectEffectGO;
    public float effectMultiplier;

    public float speed; //unit movement speed

    public enum collectableType
    {
        liveUp,
        damageUp,
        speedUp,
        coin,
        firerateUp,
        poison,
        fullLife,
        giantSing,
        bigAndImmune,
        teleportDoor


    }

    public collectableType type;


    // Use this for initialization
    void Start()
    {
        playerCharacter = GameObject.Find("PlayerGO");
    }

    // Update is called once per frame
    void Update()
    {

        //Get the unit current position
        Vector2 position = transform.position;

        //Compute the unit position
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);

        //Update the unit position
        transform.position = position;

        //this is the bottom-left point of screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        //if the unit went outside the screen on the bottom, then destroy the unit
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Detect collision of the enemy character with player character, or with player bullet
        if (col.tag == "PlayerCharacterTag")
        {
            //PlayEffectOnCollect
            PlayOnCollectEffect(col.gameObject);

            //Activate collectable effect
            CollectableEffect(type,effectMultiplier);

            //Destroy this character
            Destroy(gameObject);

        }
    }

    public void PlayOnCollectEffect(GameObject playerGO)
    {

        GameObject effect = (GameObject)Instantiate(CollectEffectGO);

        //set the explosion positon to this object position
        effect.transform.position = playerGO.transform.position;
    }

    void CollectableEffect(collectableType type, float effectMultiplier)
    {
        playerCharacter.GetComponent<PlayerControl>().PlayerCollectableEffect(type, effectMultiplier);
    }
}
