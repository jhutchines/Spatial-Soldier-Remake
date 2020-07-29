using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopUI : MonoBehaviour
{
    PlayerControls player;
    GameManager gameManager;
    bool shopOpen;
    public GameObject levelComplete;
    public GameObject nextLevelButtons;
    public GameObject repairOneButton;
    public GameObject repairAllButton;
    public GameObject watchAdButton;
    public GameObject adButtonMoveTo;

    public Text nextLevel;

    public Text money;

    public Text maxHealthLevel;
    public Text weaponDamageLevel;
    public Text projectileSpeedLevel;
    public Text rateOfFireLevel;
    public Text shipSpeedLevel;

    public Text maxHealthCost;
    public Text weaponDamageCost;
    public Text projectileSpeedCost;
    public Text rateOfFireCost;
    public Text shipSpeedCost;
    public Text fullRepairCost;
    public Text repairCost;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControls>();
        gameManager = GameObject.Find("Background").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nextLevel.gameObject.activeSelf)
        {
            nextLevel.text = "Level " + gameManager.level;
        }
        if (player.nextLevel)
        {
            if (!shopOpen) StartCoroutine(ShowShop());
            UpdateShop();
        }
    }

    IEnumerator ShowShop()
    {
        shopOpen = true;
        yield return new WaitForSeconds(2f);
        levelComplete.SetActive(true);
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject == repairOneButton)
            {
                if (player.health == player.maxHealth) continue;
            }

            if (transform.GetChild(i).gameObject == repairAllButton)
            {
                if (player.maxHealth - player.health <= 1) continue;
            }
            transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(.2f);
        }
        watchAdButton.transform.position = adButtonMoveTo.transform.position;
    }

    void UpdateShop()
    {
        money.text = gameManager.money.ToString();

        maxHealthLevel.text = player.maxHealthModifier.ToString();
        weaponDamageLevel.text = player.bulletDamage.ToString();
        projectileSpeedLevel.text = player.bulletSpeed.ToString();
        rateOfFireLevel.text = player.shootSpeed.ToString();
        shipSpeedLevel.text = player.moveSpeed.ToString();

        maxHealthCost.text = (player.maxHealthModifier * 10).ToString();
        weaponDamageCost.text = (player.bulletDamage * 10).ToString();
        projectileSpeedCost.text = (player.bulletSpeed * 10).ToString();
        rateOfFireCost.text = (player.shootSpeed * 10).ToString();
        shipSpeedCost.text = (player.moveSpeed * 10).ToString();

        fullRepairCost.text = "Repair all \n Costs  " + ((player.maxHealth - player.health) * 5).ToString();
        if (player.maxHealth - player.health <= 1) fullRepairCost.transform.parent.gameObject.SetActive(false);
        if (player.health == player.maxHealth) repairCost.transform.parent.gameObject.SetActive(false);
    }

    public void DoneButton()
    {
        nextLevelButtons.SetActive(true);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        watchAdButton.SetActive(false);
        watchAdButton.GetComponent<RewardedAdsButton>().GameEnd();
        audioSource.Play();
    }

    public void NextLevel()
    {
        gameManager.NextLevel();
        gameManager.UpdateProgress();
        SceneManager.LoadScene(1);
    }

    public void SaveAndQuit()
    {
        gameManager.NextLevel();
        gameManager.UpdateProgress();
        SceneManager.LoadScene(0);
    }

    bool CheckCanAfford(float cost)
    {
        if (gameManager.money - cost >= 0)
        {
            gameManager.money -= Mathf.RoundToInt(cost);
            return true;
        }
        else return false;
    }

    public void UpdateMaxHealth()
    {
        if (CheckCanAfford(gameManager.maxHealth * 10))
        {
            player.maxHealthModifier++;
            player.maxHealth++;
            player.health++;
            audioSource.Play();
        }
    }

    public void UpdateWeaponDamage()
    {
        if (CheckCanAfford(player.bulletDamage * 10))
        {
            player.bulletDamage++;
            audioSource.Play();
        }
    }

    public void UpdateProjectileSpeed()
    {
        if (CheckCanAfford(player.bulletSpeed * 10))
        {
            player.bulletSpeed++;
            audioSource.Play();
        }
    }

    public void UpdateRateOfFire()
    {
        if (CheckCanAfford(player.shootSpeed * 10))
        {
            player.shootSpeed++;
            audioSource.Play();
        }
    }

    public void UpdateShipSpeed()
    {
        if (CheckCanAfford(player.moveSpeed * 10))
        {
            player.moveSpeed++;
            audioSource.Play();
        }
    }

    public void SingleRepair()
    {
        if (CheckCanAfford(5))
        {
            player.health++;
            audioSource.Play();
        }
    }

    public void AllRepair()
    {
        if (CheckCanAfford((player.maxHealth - player.health) * 5))
        {
            player.health = player.maxHealth;
            audioSource.Play();
        }
    }

    public void WatchedAd()
    {
        gameManager.money *= 2;
        watchAdButton.SetActive(false);
    }
}
