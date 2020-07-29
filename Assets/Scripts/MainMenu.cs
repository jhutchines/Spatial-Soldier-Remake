using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    PersistentData persistentData;
    public GameObject fadePanel;
    public GameObject fadeReverse;
    public Text controllerType;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowMenu());
        persistentData = GameObject.Find("PersistentData").GetComponent<PersistentData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (persistentData.controllerType == PersistentData.ControllerType.Touchscreen)
        {
            controllerType.text = "Touchscreen Mode";
        }
        else
        {
            controllerType.text = "Analog Mode";
        }

        if (fadeReverse.activeSelf)
        {
            Camera.main.GetComponent<AudioSource>().volume -= 0.0025f;
        }
    }

    IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds(3.5f);
        for (int i = 1; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(.2f);
        }
    }

    public void StartGame()
    {
        fadeReverse.SetActive(true);
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }

    public void ChangeControls()
    {
        if (persistentData.controllerType == PersistentData.ControllerType.Touchscreen)
        {
            persistentData.controllerType = PersistentData.ControllerType.Analog;
        }
        else
        {
            persistentData.controllerType = PersistentData.ControllerType.Touchscreen;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
