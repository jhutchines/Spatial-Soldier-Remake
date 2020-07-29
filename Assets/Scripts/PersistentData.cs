using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public enum ControllerType {Touchscreen, Analog}
    public ControllerType controllerType;

    private void Awake()
    { 
        PersistentData[] data = FindObjectsOfType<PersistentData>();
        if (data.Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
