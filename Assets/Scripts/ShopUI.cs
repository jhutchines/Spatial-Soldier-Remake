using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    PlayerControls player;
    GameManager gameManager;
    bool shopOpen;
    public GameObject levelComplete;

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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControls>();
        gameManager = GameObject.Find("Background").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
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
            transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(.2f);
        }
    }

    void UpdateShop()
    {
        money.text = gameManager.money.ToString();

        maxHealthLevel.text = gameManager.maxHealth.ToString();
        weaponDamageLevel.text = player.bulletDamage.ToString();
        projectileSpeedLevel.text = player.bulletSpeed.ToString();
        rateOfFireLevel.text = player.shootSpeed.ToString();
        shipSpeedLevel.text = player.moveSpeed.ToString();

        maxHealthCost.text = (gameManager.maxHealth * 10).ToString();
        weaponDamageCost.text = (player.bulletDamage * 10).ToString();
        projectileSpeedCost.text = (player.bulletSpeed * 10).ToString();
        rateOfFireCost.text = (player.shootSpeed * 10).ToString();
        shipSpeedCost.text = (player.moveSpeed * 10).ToString();

        fullRepairCost.text = "Repair all \n Costs  " + ((player.maxHealth - player.health) * 5).ToString();
    }
}
