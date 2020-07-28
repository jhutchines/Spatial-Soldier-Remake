using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveExplosion : MonoBehaviour
{
    public Vector3 moveTowards;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animator>().GetBool("Finished"))
        {
            Destroy(gameObject);
        }
        if (moveTowards != new Vector3(0, 0, 0))
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTowards, speed * Time.deltaTime);
        }
    }
}
