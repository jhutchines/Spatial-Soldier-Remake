using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameOverMenu());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GameOverMenu()
    {
        yield return new WaitForSeconds(2);
        for (int i = 1; i < transform.childCount; i++)
        {
            yield return new WaitForSeconds(.2f);
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
