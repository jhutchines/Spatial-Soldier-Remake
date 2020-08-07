using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public bool debugMode;

    public int maxHealth;
    public int maxHealthModifier;
    public int health;
    public float moveSpeed;
    public GameObject target;
    public float shootSpeed;
    public GameObject bullet;
    public int bulletDamage = 1;
    public float bulletSpeed;
    public GameObject damageAnim;
    public GameObject joystick;
    float reloadTime;
    GameObject turret;
    GameManager gameManager;
    PersistentData persistentData;
    public bool nextLevel;
    public bool levelStart = true;
    bool transition;
    public Vector3 moveTo;
    public Vector3 moveToEnd;
    public bool touchAllowed;

    public AudioClip chargeSound;
    public AudioClip warpSound;
    public AudioClip startSound;
    AudioSource audioSource;

    public GameManager.PickUpType pickUpType;
    public float pickupTime;
    float laserAttackTime;

    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        turret = transform.GetChild(1).gameObject;
        reloadTime = shootSpeed;
        gameManager = GameObject.Find("Background").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        if (!debugMode)
        {
            persistentData = GameObject.Find("PersistentData").GetComponent<PersistentData>();
            if (persistentData.controllerType == PersistentData.ControllerType.Touchscreen)
            {
                touchAllowed = true;
                GetComponent<Joystick>().enabled = false;
                joystick.SetActive(false);
            }
        }
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(StartLevel());
    }

    public void UpdateStats(int in_maxHealth, int in_health, int in_weaponDamage, float fl_bulletSpeed, float fl_rateOfFire, float fl_shipSpeed)
    {
        maxHealthModifier = in_maxHealth;
        health = in_health;
        bulletDamage = in_weaponDamage;
        bulletSpeed = fl_bulletSpeed;
        shootSpeed = fl_rateOfFire;
        moveSpeed = fl_shipSpeed;
        maxHealth += maxHealthModifier;
    }

    // Update is called once per frame
    void Update()
    {
        // -- DEBUG INPUT --
        // Full Health
        if (Input.GetKeyDown(KeyCode.X))
        {
            health = maxHealth;
            Debug.Log("Full health");
        }

        if (gameManager.playerAlive && !nextLevel && !levelStart)
        {
            if (((Input.GetMouseButton(0) || Input.touchCount > 0) && touchAllowed) || GetComponent<Joystick>().touchStart)
            {
                MovePlayer();
            }

            if (target != null)
            {
                if (target.GetComponent<EnemyMovement>().enemyDead) target = null;
            }

            for (int i = 0; i < gameManager.enemies.Length; i++)
            {
                if (gameManager.enemies.Length >= 1)
                {
                    if (target == null && gameManager.enemies[i] != null) target = gameManager.enemies[i].gameObject;
                    if (gameManager.enemies[i] != null)
                    {
                        if (Vector3.Distance(transform.position, gameManager.enemies[i].transform.position) <
                            Vector3.Distance(transform.position, target.transform.position) && !gameManager.enemies[i].enemyDead &&
                            gameManager.enemies[i].transform.position.z <= 10) 
                        {
                                target = gameManager.enemies[i].gameObject;
                        }
                    }
                }
                else if (gameManager.enemies[0].enemyDead) target = null;
            }

            if (target != null) AttackTarget();
            else
            {
                lineRenderer.positionCount = 0;
                laserAttackTime = 0;
            }
        }
        if (gameManager.levelEnd && !transition) StartCoroutine(NextLevel());

        if (nextLevel)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToEnd, 100f * Time.deltaTime);
        }

        if (levelStart && moveTo != new Vector3(0, 0, 0))
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, 60f * Time.deltaTime);
        }

        if (pickUpType != GameManager.PickUpType.None) PickUpTime();
    }

    void MovePlayer()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (touchAllowed)
        {
            if (Physics.Raycast(ray, out hit, 20f) || Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit, 20f))
            {
                moveTo = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            }
        }
        transform.position = Vector3.MoveTowards(transform.position, moveTo, (moveSpeed + 9) * Time.deltaTime);

        if (!nextLevel || !levelStart)
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -13, 13), transform.position.y, Mathf.Clamp(transform.position.z, -6, 6));
    }

    void AttackTarget()
    {

        reloadTime += Time.deltaTime;

        if (target.transform.position.z <= 10)
        {
            Vector3 targetPos = target.transform.position + (Vector3.back * 2);
            turret.transform.LookAt(targetPos);
            if (reloadTime >= (1.05f - (0.05f * shootSpeed)))
            {
                if (pickUpType == GameManager.PickUpType.LaserBeam)
                {
                    turret.transform.LookAt(target.transform.position);
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, new Vector3(transform.position.x, 1, transform.position.z));
                    lineRenderer.SetPosition(1, new Vector3(target.transform.position.x, 1, target.transform.position.z));
                    laserAttackTime += Time.deltaTime;
                    if (laserAttackTime >= .5f)
                    {
                        target.GetComponent<EnemyMovement>().TakeDamage(target.transform.position, bulletDamage);
                        laserAttackTime = 0;
                    }
                }

                else
                {
                    laserAttackTime = 0;
                    turret.transform.LookAt(targetPos);
                    lineRenderer.positionCount = 0;
                    reloadTime = 0;
                    GameObject newBullet = Instantiate(bullet, transform.position + transform.GetChild(1).TransformDirection(Vector3.forward), transform.GetChild(1).rotation);
                    newBullet.GetComponent<Bullet>().bulletDamage = bulletDamage;
                    newBullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed + 9;

                    if (pickUpType == GameManager.PickUpType.TripleShot)
                    {
                        GameObject newBullet1 = Instantiate(bullet, transform.position + transform.GetChild(1).TransformDirection(Vector3.forward + (Vector3.left / 2)), transform.GetChild(1).rotation);
                        newBullet1.GetComponent<Bullet>().bulletDamage = bulletDamage;
                        newBullet1.GetComponent<Bullet>().bulletSpeed = bulletSpeed + 9;

                        GameObject newBullet2 = Instantiate(bullet, transform.position + transform.GetChild(1).TransformDirection(Vector3.forward + Vector3.right / 2), transform.GetChild(1).rotation);
                        newBullet2.GetComponent<Bullet>().bulletDamage = bulletDamage;
                        newBullet2.GetComponent<Bullet>().bulletSpeed = bulletSpeed + 9;
                    }

                    if (pickUpType == GameManager.PickUpType.BigShot)
                    {
                        newBullet.transform.localScale = new Vector3(2.5f, 1, 2.5f);
                        newBullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed + 10;
                    }

                }

            }
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
        gameManager.gameOver.SetActive(true);
        Destroy(gameObject);
    }

    IEnumerator NextLevel()
    {
        transition = true;
        yield return new WaitForSeconds(5f);
        transform.GetChild(2).gameObject.SetActive(true);
        audioSource.clip = chargeSound;
        audioSource.volume = 0.15f;
        audioSource.loop = true;
        audioSource.Play();
        yield return new WaitForSeconds(5f);
        nextLevel = true;
        audioSource.loop = false;
        audioSource.clip = warpSound;
        audioSource.volume = 0.5f;
        audioSource.Play();
        moveToEnd = new Vector3(transform.position.x, transform.position.y, transform.position.z + 20);
    }

    IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(1f);
        audioSource.clip = startSound;
        audioSource.volume = 0.25f;
        audioSource.Play();
        moveTo = new Vector3(transform.position.x, transform.position.y, -1);
        yield return new WaitForSeconds(1f);
        levelStart = false;
    }

    void PickUpTime()
    {
        pickupTime += Time.deltaTime;
        if (pickupTime >= 10)
        {
            pickUpType = GameManager.PickUpType.None;
            pickupTime = 0;
        }
    }
}
