using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playerAlive = true;

    public bool fullVersion;

    public enum BulletType {Player, Enemy}

    public GameObject[] explosions;
    public GameObject[] enemySmall;
    public GameObject[] enemyMedium;
    public GameObject[] enemyLarge;

    public Sprite[] backgrounds;

    public EnemyMovement[] enemies;

    public bool levelEnd;
    public bool levelStarted;

    public int level;
    public int money;
    public int maxHealth;
    public int health;
    public int weaponDamage;
    public float bulletSpeed;
    public float rateOfFire;
    public float shipSpeed;

    private float time;
    PlayerControls player;

    public GameObject gameOver;


    // Start is called before the first frame update
    void Start()
    {
        enemies = new EnemyMovement[0];
        player = GameObject.Find("Player").GetComponent<PlayerControls>();
        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            level = 1;
            money = 0;
            maxHealth = 1;
            health = 10;
            weaponDamage = 1;
            bulletSpeed = 1;
            rateOfFire = 1;
            shipSpeed = 1;
            PlayerPrefs.SetInt("FirstTime", 1);
        }
        else
        {
            level = PlayerPrefs.GetInt("Level");
            money = PlayerPrefs.GetInt("Money");
            maxHealth = PlayerPrefs.GetInt("MaxHealth");
            health = PlayerPrefs.GetInt("Health");
            weaponDamage = PlayerPrefs.GetInt("WeaponDamage");
            bulletSpeed = PlayerPrefs.GetFloat("BulletSpeed");
            rateOfFire = PlayerPrefs.GetFloat("RateOfFire");
            shipSpeed = PlayerPrefs.GetFloat("ShipSpeed");
        }
        player.UpdateStats(maxHealth, health, weaponDamage, bulletSpeed, rateOfFire, shipSpeed);

        int rand = Random.Range(0, backgrounds.Length);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = backgrounds[rand];
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemies = FindObjectsOfType<EnemyMovement>();
        time += Time.deltaTime;
        if (time >= (49f + level))
        {
            levelEnd = true;
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            ResetProgress();
            Debug.Log("Progress Reset");
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void NextLevel()
    {
        level++;
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetInt("MaxHealth", 1);
        PlayerPrefs.SetInt("Health", 10);
        PlayerPrefs.SetInt("WeaponDamage", 1);
        PlayerPrefs.SetFloat("BulletSpeed", 1);
        PlayerPrefs.SetFloat("RateOfFire", 1);
        PlayerPrefs.SetFloat("ShipSpeed", 1);
    }

    public void UpdateProgress()
    {
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("MaxHealth", player.maxHealthModifier);
        PlayerPrefs.SetInt("Health", player.health);
        PlayerPrefs.SetInt("WeaponDamage", player.bulletDamage);
        PlayerPrefs.SetFloat("BulletSpeed", player.bulletSpeed);
        PlayerPrefs.SetFloat("RateOfFire", player.shootSpeed);
        PlayerPrefs.SetFloat("ShipSpeed", player.moveSpeed);
    }
}
