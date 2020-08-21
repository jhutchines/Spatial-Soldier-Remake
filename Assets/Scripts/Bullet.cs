using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public int bulletDamage = 1;
    float destroyTime;
    public GameManager.BulletType bulletType;
    GameManager gameManager;
    bool alreadyHit;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Background").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.TransformDirection(Vector3.forward * bulletSpeed * Time.deltaTime);
        destroyTime += Time.deltaTime;
        if (destroyTime >= 10) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bulletType == GameManager.BulletType.Player)
        {
            if (other.GetComponent<EnemyMovement>() != null && !alreadyHit)
            {
                alreadyHit = true;
                other.GetComponent<EnemyMovement>().TakeDamage(transform.position, bulletDamage);
                gameManager.AddMoney(1);
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.GetComponent<PlayerControls>() != null && !alreadyHit)
            {
                alreadyHit = true;
                other.GetComponent<PlayerControls>().TakeDamage(transform.position, bulletDamage);
                Destroy(gameObject);
            }
        }
    }
}
