using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float shootFrom = 10;
    public float shootTime;
    float reloadTime;
    GameObject player;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        reloadTime = shootTime;
        gameManager = GameObject.Find("Background").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= shootFrom && transform.position.z > -shootFrom && gameManager.playerAlive && 
            !GetComponentInParent<EnemyMovement>().enemyDead && !player.GetComponent<PlayerControls>().nextLevel)
        {
            transform.LookAt(player.transform);
            reloadTime += Time.deltaTime;
            if (reloadTime >= Random.Range(0.85f, shootTime))
            {
                reloadTime = 0;
                Instantiate(GetComponentInParent<EnemyMovement>().bullet, transform.position + transform.TransformDirection(Vector3.forward), transform.rotation);
            }
        }
    }
}
