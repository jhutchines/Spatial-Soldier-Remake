using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float startingHealth;
    public float health;
    public GameObject damageAnim;

    public float shipSpeed;

    public float disappearOffset;

    public GameObject bullet;

    public Vector3 explosionSize;

    Vector3 moveTowards;
    GameManager gameManager;
    public bool enemyDead;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Background").GetComponent<GameManager>();
        moveTowards = new Vector3(transform.position.x, transform.position.y, -15 + disappearOffset);
        health = startingHealth + gameManager.level;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveTowards, shipSpeed * Time.deltaTime);
        if (transform.position == moveTowards && !enemyDead) Destroy(gameObject);
    }

    public void TakeDamage(Vector3 hitPosition, float damage)
    {
        Instantiate(damageAnim, hitPosition, transform.GetChild(0).rotation, transform);
        health -= damage;
        if (health <= 0 && !enemyDead) StartCoroutine(EnemyDeath());
    }

    IEnumerator EnemyDeath()
    {
        enemyDead = true;
        GameObject explosions = Instantiate(gameManager.explosions[1], transform.position, transform.GetChild(0).rotation);
        explosions.transform.localScale = explosionSize;
        explosions.GetComponent<RemoveExplosion>().moveTowards = moveTowards;
        explosions.GetComponent<RemoveExplosion>().speed = shipSpeed;
        yield return new WaitForSeconds(.8f);
        Destroy(gameObject);
    }
}
