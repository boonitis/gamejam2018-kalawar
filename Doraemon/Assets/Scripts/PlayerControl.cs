using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private float x, y; //player positionn
    private int maxLives; //player max lives
    private int lives; //current player Lives
    private float speed; //player speed modifier
    private int weaponDamage;
    private float weaponFireRate; //player fire rate
    private float weaponFireReady; //check whether player is ready to fire
    private bool isImmune = false; //Is player immune to damage?

    public GameObject GameManagerGO;//Reference to GameManager
    public GameObject PlayerBulletGO;
    public GameObject bulletPosition1;
    public GameObject bulletPosition2;
    public GameObject DamagedEffectGO;
    public GameObject ExplosionGO;
    public Animator animator;

    public static float ScoreMultiplier;
    public Text LiveUIText;

    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            lives = value;
            LiveUIText.text = lives.ToString()+"";
        }
    }
    public float WeaponFireRate
    {
        get
        {
            return weaponFireRate;
        }

        set
        {
            weaponFireRate = value;
        }
    }
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }
    public int MaxLives
    {
        get
        {
            return maxLives;
        }

        set
        {
            maxLives = value;
        }
    }
    public int WeaponDamage
    {
        get
        {
            return weaponDamage;
        }

        set
        {
            weaponDamage = value;
        }
    }

    public void Init()
    {
        //set default status
        ScoreMultiplier = 1;
        WeaponDamage = 1;
        MaxLives = 10;
        Lives = MaxLives;
        Speed = 2;
        WeaponFireRate = 0.5f;
        GetComponent<Renderer>().material.color = Color.white;
        animator = GetComponent<Animator>();

        //update the Lives UI text
        LiveUIText.text = lives.ToString();
        
        //reset player position
        transform.position = new Vector2(0, 0);

        //set this player game object to active
        gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        //Debug log 
        Debug.LogFormat("ScoreMultiplier/Damage/Speed/Firerate/isImmune : {0}/{1}/{2}/{3}/{4} ", ScoreMultiplier, WeaponDamage, Speed, WeaponFireRate,isImmune);

        //Set Copter Speed
        animator.SetFloat("CopterSpeed",Speed);

        x = Input.GetAxisRaw("Horizontal"); // The value will be -1, 0, 1 : Left, No, Right
        y = Input.GetAxisRaw("Vertical"); // The value will be -1, 0, 1 : Down, No, Up
        Vector2 direction = new Vector2(x, y).normalized;

        Move(direction);
        Shoot();
    }

    void Move(Vector2 direction)
    {
        //Find the screen limit for player movement
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        max.x = max.x - 0.155f;
        min.x = min.x + 0.155f;

        max.y = max.y - 0.285f;
        min.y = min.y + 0.285f;

        //Get the player object position
        Vector2 pos = transform.position;

        //Move in the direction detect from axis movement
        pos += direction * Speed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    void Shoot()
    {
        //press spacebar to fire a bullet
        //if godmode is enabled player can shoot continuosly holding spacebar
        if (Input.GetButton("Fire1") && Time.time > weaponFireReady)
        {
            //Set weapon fire rate
            weaponFireReady = Time.time + WeaponFireRate;

            //play the firing sound effect
            GetComponent<AudioSource>().Play();

            //instantiate the first bullet
            GameObject bullet01 = (GameObject)Instantiate(PlayerBulletGO);
            bullet01.GetComponent<PlayerBullet>().Damage = WeaponDamage;
            bullet01.transform.position = bulletPosition1.transform.position; //set the bullet initial position

            //instantiate the second bullet
            GameObject bullet02 = (GameObject)Instantiate(PlayerBulletGO);
            bullet02.GetComponent<PlayerBullet>().Damage = WeaponDamage;
            bullet02.transform.position = bulletPosition2.transform.position; //set the bullet initial position
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Detect collision of the player character with an enemy character, or with an enemy bullet
        if (((col.tag == "EnemyCharacterTag") || (col.tag == "EnemyBulletTag")) && !isImmune)
        {
            //Reset score multiplier on damaged
            ScoreMultiplier = 1;

            StartCoroutine("FlashCharacterOnDamage");//Blink unit on damaged
            PlayExplosion();//Create damaged or explosion effect
            Lives--;//player lose one Lives

            LiveUIText.text = Lives.ToString();//Update the UI text Lives

            if (Lives <= 0)//If player is dead
            {
                StopAllCoroutines();
                //Change game manager state to game over state
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);

                //hide the player's character
                gameObject.SetActive(false);
            }
        }
    }

    //Function to instantiate an explosion
    void PlayExplosion()
    {
        GameObject explosion;

        if (Lives <= 0)
            explosion = (GameObject)Instantiate(ExplosionGO);
        else
            explosion = (GameObject)Instantiate(DamagedEffectGO);

        //set the explosion positon to this object position
        explosion.transform.position = transform.position;
    }

    //Function to activate collectable effect
    public void PlayerCollectableEffect(Collectable.collectableType type, float effectMultiplier)
    {
        switch (type)
        {
            case Collectable.collectableType.liveUp:
                if (Lives < MaxLives)
                    Lives += (int)(1 * effectMultiplier);
                break;
            case Collectable.collectableType.damageUp:
                if (WeaponDamage < 15)
                    WeaponDamage += (int)(1 * effectMultiplier);
                break;
            case Collectable.collectableType.speedUp:
                if (Speed < 7f)
                    Speed += (0.5f * effectMultiplier);
                break;
            case Collectable.collectableType.poison:
                Lives--;//player lose one Lives
                StartCoroutine("FlashCharacterOnDamage");
                WeaponDamage = 1;
                Speed = 2;
                WeaponFireRate = 0.5f;
                ScoreMultiplier = 1;
                break;
            case Collectable.collectableType.firerateUp:
                WeaponFireRate -= 0.05f * effectMultiplier;
                break;
            case Collectable.collectableType.fullLife:
                Lives = MaxLives;
                break;
            case Collectable.collectableType.coin:
                ScoreMultiplier += 0.5f;
                break;
            case Collectable.collectableType.bigAndImmune:
                StartCoroutine("BiggerItemEffect");
                break;
            case Collectable.collectableType.giantSing:
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyCharacterTag"))
                {
                    enemy.GetComponent<EnemyControl>().PlayExplosion();
                    enemy.GetComponent<EnemyControl>().DropCollectable();
                    Destroy(enemy);
                }
                foreach (GameObject enemyBullet in GameObject.FindGameObjectsWithTag("EnemyBulletTag"))
                {
                    Destroy(enemyBullet);
                }
                break;
            case Collectable.collectableType.teleportDoor:
                break;
        }
    }

    //Flash Character on Damage
    IEnumerator FlashCharacterOnDamage()
    {
        isImmune = true;
        for (int i = 0; i < 2; i++)
        {
            GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(.1f);
            GetComponent<Renderer>().material.color = Color.white;
            yield return new WaitForSeconds(.1f);
        }
        isImmune = false;
    }

    //Make Character Bigger For A Time Period
    IEnumerator BiggerItemEffect()
    {
        StopCoroutine("FlashCharacterOnDamage");
        float scaleValue = 0.001f;
        isImmune = true;
        for (int i = 0; i < 50; i++)
        {
            transform.localScale += new Vector3(scaleValue, scaleValue, scaleValue);
            yield return new WaitForSeconds(.01f);
        }
        for (int i = 0; i < 5; i++)
        {
            GetComponent<Renderer>().material.color = Color.green;
            yield return new WaitForSeconds(.5f);
            GetComponent<Renderer>().material.color = Color.white;
            yield return new WaitForSeconds(.5f);
        }
        for (int i = 0; i < 50; i++)
        {
            transform.localScale -= new Vector3(scaleValue, scaleValue, scaleValue);
            yield return new WaitForSeconds(.01f);
        }
        isImmune = false;
    }
}
