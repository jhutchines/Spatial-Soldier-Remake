using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public Vector2 resetPosition;
    public Vector2 moveTo;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (new Vector2(transform.position.x, transform.position.z) != moveTo)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveTo.x, transform.position.y, moveTo.y), moveSpeed * Time.deltaTime);
        }
        else
        {
            if (resetPosition != new Vector2(0, 0))
            {
                transform.position = new Vector3(resetPosition.x, transform.position.y, resetPosition.y);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
