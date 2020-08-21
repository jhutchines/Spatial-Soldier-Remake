using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public GameManager.PickUpType pickUpType;
    Vector3 moveTowards;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        moveTowards = new Vector3(transform.position.x, transform.position.y, -15);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveTowards, moveSpeed * Time.deltaTime);
        if (transform.position == moveTowards) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControls>() != null)
        {
            if (pickUpType != GameManager.PickUpType.Repair)
            {
                other.GetComponent<PlayerControls>().pickUpType = pickUpType;
                other.GetComponent<PlayerControls>().pickupTime = 0;
            }
            else
            {
                if (other.GetComponent<PlayerControls>().health <= other.GetComponent<PlayerControls>().maxHealth - 3)
                {
                    other.GetComponent<PlayerControls>().health += 3;
                }
                else other.GetComponent<PlayerControls>().health = other.GetComponent<PlayerControls>().maxHealth;
            }
            other.GetComponent<PlayerControls>().PickupAudio(pickUpType);
            Destroy(gameObject);
        }
    }
}
