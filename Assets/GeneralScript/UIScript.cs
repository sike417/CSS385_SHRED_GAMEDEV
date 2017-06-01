using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour {
    
    // Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
      
	}

    public void levelOneClick()
    {
        SceneManager.LoadScene("Level1");
    }

    public void levelTwoClick()
    {
        SceneManager.LoadScene("Level2");
    }

    public void levelSelectClick()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void creditClick()
    {
        SceneManager.LoadScene("Credits");
    }

    public void menuClick()
    {
        SceneManager.LoadScene("Menu");
    }

    public void quitClick()
    {
        Application.Quit();
    }
}
