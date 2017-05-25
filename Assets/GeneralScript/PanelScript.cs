using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelScript : MonoBehaviour
{
    private GameObject gm;
    private GlobalBehavior gb;
    public Button RetryButton;
    public Button NextLevelButton;
   

    // Use this for initialization
    void Start()
    {
        RetryButton.onClick.AddListener(Retry);
        if (NextLevelButton != null)
        {
            NextLevelButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("level2");
                SceneManager.UnloadSceneAsync("Sample_Terrain");
            });
        }
        gm = GameObject.Find("Game Manager");
        gb = gm.GetComponent<GlobalBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void Retry()
    {
        gb.RetryLevel();
    }
}

