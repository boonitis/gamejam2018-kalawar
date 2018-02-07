using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{

    public GameObject EnemyBulletGO; //this is our enemy bullet prefab

    public float initFireInSecond; //First time firing in second
    public float fireRateInSecond; //Fire rate in second

    public bool isReloadType; //Reload-Type enemy reloads after number of bullets are fired
    public int maxBullets; //Number of bullets unit can fire before reload
    int currentBullets; //Current bullets
    public float reloadTime; //Delay time between each reload

    // Use this for initialization
    void Start()
    {
        ReloadEnemyBullet(); //Set init currentBullets to maxBullets and start firing

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FireEnemyBullet()
    {

        GameObject playerCharacter = GameObject.Find("PlayerGO");

        if (playerCharacter != null && (!isReloadType || currentBullets != 0)) //if player is not dead and is not ReloadType or is not out of ammo
        {

            GetComponent<AudioSource>().Play();

            //instantiate an enemy bullet
            GameObject bullet = (GameObject)Instantiate(EnemyBulletGO);

            //set the bullet's initial position
            bullet.transform.position = transform.position;

            //compute the bullet's direction towards the player's ship
            Vector2 direction = playerCharacter.transform.position - bullet.transform.position;

            //set the bullet's direction
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);

            if (isReloadType)
                currentBullets--;

        }
        else if (currentBullets == 0)
        {
            Invoke("ReloadEnemyBullet", reloadTime);
            CancelInvoke("FireEnemyBullet");
        }

    }

    //Function to reload enemy bullet
    void ReloadEnemyBullet()
    {
        currentBullets = maxBullets;
        InvokeRepeating("FireEnemyBullet", initFireInSecond, fireRateInSecond);

    }
}
