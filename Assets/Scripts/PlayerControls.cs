using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public int maxHealth;
    public int maxHealthModifier;
    public int health;
    public int healthModifier;
    public float moveSpeed;
    public GameObject target;
    public float shootSpeed;
    public GameObject bullet;
    public int bulletDamage = 1;
    public float bulletSpeed;
    public GameObject damageAnim;
    float reloadTime;
    GameObject turret;
    GameManager gameManager;
    public bool nextLevel;
    bool transition;
    Vector3 moveTo;

    // Start is called before the first frame update
    void Start()
    {
        turret = transform.GetChild(1).gameObject;
        reloadTime = shootSpeed;
        gameManager = GameObject.Find("Background").GetComponent<GameManager>();
    }

    public void UpdateStats(int in_maxHealth, int in_health, int in_weaponDamage, float fl_bulletSpeed, float fl_rateOfFire, float fl_shipSpeed)
    {
        maxHealthModifier = in_maxHealth;
        healthModifier = in_health;
        bulletDamage = in_weaponDamage;
        bulletSpeed = fl_bulletSpeed;
        shootSpeed = fl_rateOfFire;
        moveSpeed = fl_shipSpeed;

        health += healthModifier;
        maxHealth += maxHealthModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.playerAlive && !nextLevel)
        {
            if (Input.GetMouseButton(0))
            {
                MovePlayer();
            }

            for (int i = 0; i < gameManager.enemies.Length; i++)
            {
                if (target == null) target = gameManager.enemies[i].gameObject;
                else if (Vector3.Distance(transform.position, gameManager.enemies[i].transform.position) <
                         Vector3.Distance(transform.position, target.transform.position) && !gameManager.enemies[i].GetComponent<EnemyMovement>().enemyDead)
                    target = gameManager.enemies[i].gameObject;
                else if (gameManager.enemies.Length == 1 && gameManager.enemies[i].GetComponent<EnemyMovement>().enemyDead) target = null;
            }

            if (target != null) AttackTarget();
        }
        if (gameManager.levelEnd && !transition) StartCoroutine(NextLevel());

        if (nextLevel)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, 100f * Time.deltaTime);
        }
    }

    void MovePlayer()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 20f) || Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit, 20f))
        {
            moveTo = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.position = Vector3.MoveTowards(transform.position, moveTo, (moveSpeed + 9) * Time.deltaTime);
            
        }

        if (!nextLevel)
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -13, 13), transform.position.y, Mathf.Clamp(transform.position.z, -6, 6));
    }

    void AttackTarget()
    {
        turret.transform.LookAt(target.transform.position + (Vector3.back * 2));

        reloadTime += Time.deltaTime;

        if (reloadTime >= shootSpeed)
        {
            reloadTime = 0;
            GameObject newBullet = Instantiate(bullet, transform.position + transform.GetChild(1).TransformDirection(Vector3.forward), transform.GetChild(1).rotation);
            newBullet.GetComponent<Bullet>().bulletDamage = bulletDamage;
            newBullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed + 9;
        }
    }

    public void TakeDamage(Vector3 hitPosition, int damage)
    {
        if (!nextLevel)
        {
            Instantiate(damageAnim, hitPosition, transform.GetChild(0).rotation, transform);
            health -= damage;
        }

        if (health <= 0 && gameManager.playerAlive)
        {
            StartCoroutine(PlayerDeath());
        }
    }

    IEnumerator PlayerDeath()
    {
        gameManager.playerAlive = false;
        GameObject explosions = Instantiate(gameManager.explosions[0], transform.position, transform.GetChild(0).rotation);
        explosions.transform.localScale = new Vector3(4, 4, 1);
        gameManager.ResetProgress();
        yield return new WaitForSeconds(.8f);
        Destroy(gameObject);
    }

    IEnumerator NextLevel()
    {
        transition = true;
        yield return new WaitForSeconds(5f);
        transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        nextLevel = true;
        moveTo = new Vector3(transform.position.x, transform.position.y, transform.position.z + 20);
    }
}
