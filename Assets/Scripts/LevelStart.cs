using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    GameManager gameManager;
    GameObject panel;
    public GameObject levelText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Background").GetComponent<GameManager>();
        panel = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!panel.activeSelf)
        {
            if (!gameManager.levelStarted) levelText.SetActive(true);
            gameManager.levelStarted = true;
        }
    }
}
