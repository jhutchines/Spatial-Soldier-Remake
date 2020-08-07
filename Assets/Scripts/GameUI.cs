using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject damageBar;
    PlayerControls player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(player.health * 10, 10);
        healthBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(((player.health * 10) / 2) + 15, 10, 0);

        damageBar.GetComponent<RectTransform>().sizeDelta = new Vector2(player.maxHealth * 10, 10);
        damageBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(((player.maxHealth * 10) / 2) + 15, 10, 0);
    }
}
