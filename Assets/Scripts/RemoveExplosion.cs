using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveExplosion : MonoBehaviour
{
    public AudioClip[] explosions;
    public Vector3 moveTowards;
    public float speed;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (explosions.Length > 0)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = explosions[Random.Range(0, explosions.Length)];
            audioSource.Play();
        }
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
