using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    public enum BulletType
    {
        normal,
        missile,
        mine
    }

    public GameObject ExplosionGO;

    public BulletType type; //Bullet type
    public float speed;
    private int damage; //Bullet on hit damage
    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;

        position = new Vector2(position.x, position.y + speed * Time.deltaTime);

        transform.position = position;

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        if (transform.position.y > max.y)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Detect collision with enemy character
        if (col.tag == "EnemyCharacterTag")
        {
            if (type == BulletType.missile)
                PlayExplosion();

            Destroy(gameObject); //Destroy this character
        }
    }

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExplosionGO);
        //set the explosion positon to this object position
        explosion.transform.position = transform.position;
    }
}
