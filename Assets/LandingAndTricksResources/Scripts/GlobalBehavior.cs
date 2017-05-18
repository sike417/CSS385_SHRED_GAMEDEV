using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalBehavior : MonoBehaviour
{

    public Transform spawnLocation;
    public GameObject mSnowboarder;
    public GameObject mSnowboarderClone;
    public GameObject finishLine;
    private AudioSource crashSound;
    private int score = 0;
    public GameObject UIGame;
    public GameObject UIDie;
    public GameObject UIWin;
    public Camera camera;
    // UI text variables
    public Text landingText;
    public Text speedMulText;
    public Text trickText;
    public Text scoreText;
    public Image timerImage;
    private CameraScript cs;
    private playerBehavior pb;

    // Use this for initialization
    void Start()
    {
        //mSnowboarder = Instantiate(mSnowboarderClone, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        crashSound = GetComponent<AudioSource>();
        timerImage.fillAmount = 0;
        UIDie.SetActive(false);
        UIWin.SetActive(false);

        cs = (CameraScript)GetComponentInParent(typeof(CameraScript));
        pb = (playerBehavior)mSnowboarder.GetComponent(typeof(playerBehavior));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            //DestroyObject(mSnowboarder);
            PlayerDie();
//            score = 0;
            //mSnowboarder = Instantiate(mSnowboarderClone, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        }
        //Debug.Log(speedMulText.text);
        //Debug.Log(trickText.text);
        
        scoreText.text = "Score: " + score.ToString();

        if(mSnowboarder.transform.position.x > finishLine.transform.position.x)
        {
            PlayerWin();
            // win game screen
        }
    }

    public void UpdateLandingText(string newText)
    {
        landingText.text = newText;
    }

    public void UpdateSpeedMulText(string newText)
    {
        speedMulText.text = newText;
    }

    public void UpdateTrickText(string newText)
    {
        trickText.text = newText;
    }

    public void UpdateScore(int points)
    {
        score += points;
    }

    public void updateTimer(float initTime, float deltaTime)
    {
        timerImage.fillAmount = deltaTime / initTime;
    }

    public void PlayerWin()
    {
        score = 0;
        UIWin.SetActive(true);
        UIGame.SetActive(false);
        cs.StopCam();
    }
    public void PlayerDie()
    {
        score = 0;
        UIDie.SetActive(true);
        UIGame.SetActive(false);
        cs.StopCam();
    }
    public void RetryLevel()
    {
        UIWin.SetActive(false);
        UIDie.SetActive(false);
        UIGame.SetActive(true);
        pb.Retry();
        cs.StartCam();
        
    }
    public void DestroyMe()
    {
        //DestroyObject(mSnowboarder);
        crashSound.Play();
        speedMulText.text = "";
        trickText.text = "Trick:";
        score = 0;
        PlayerDie();
    }
}
