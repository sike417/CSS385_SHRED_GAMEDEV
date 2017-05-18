using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Button btn;
	// Use this for initialization
	void Start () {
		btn.onClick.AddListener(() =>
		{
            Debug.Log("reached Here");
		    SceneManager.LoadScene("Sample_Terrain");
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
