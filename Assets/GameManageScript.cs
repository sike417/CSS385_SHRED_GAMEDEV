using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManageScript : MonoBehaviour {

    public Button LevelOneButton;
    public Button LevelTwoButton;

    // Use this for initialization
    void Start () {
        LevelOneButton.onClick.AddListener(() =>
        {
//            Debug.Log("reached Here");
            SceneManager.LoadScene("Sample_Terrain");
        });
        LevelTwoButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("level2");
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
