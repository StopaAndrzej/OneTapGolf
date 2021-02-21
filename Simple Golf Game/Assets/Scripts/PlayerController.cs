using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private List<Button> menuButtons;

    [SerializeField] private float startSpeedValue;
    private float actualSpeedValue;

    [SerializeField] private float ballLifeTime = 5.0f;          //used for ball life time after throw
    private float actualTimer;
    private bool runTimer;
    private bool autoThrow = false;
    private bool throwUsed;

    [HideInInspector] public enum PlayerState { inMenu, getReady,inGame}
    [HideInInspector] public PlayerState currentPlayerState;

    [HideInInspector] public GameObject player_GolfBall;
    private GameObject lastSpawned_player_GolfBall;           //if player was still holding action button and new ball spawned 
                                                             //this is additional secure to block throw action on the new golf ball
    private void Start()
    {
        levelGenerator.CalibrateWorldPosWithCurrentAspectRatio();
        MenuMode(true);
    }

    private void Update()
    {
        if (currentPlayerState == PlayerState.inGame && player_GolfBall != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !player_GolfBall.GetComponent<Trajectory>().CheckIfThrowWasUsed())
            {
                lastSpawned_player_GolfBall = player_GolfBall;
                player_GolfBall.GetComponent<Trajectory>().RunIncreaseDistance(actualSpeedValue, levelGenerator.right_worldPosScreenBorder);
            }

            if (Input.GetKeyUp(KeyCode.Space) && !autoThrow && !throwUsed)
            {
                PlayerThrowAction(false);
            }

            //GameOver timer: 
            //1. ball velocity = 0 and no goal
            //2. ball out of the screen
            //3. ball exceeded life time after throw
            if (runTimer)
            {
                actualTimer -= Time.deltaTime;
                if (actualTimer < 0 || player_GolfBall.transform.position.x > levelGenerator.right_worldPosScreenBorder || player_GolfBall.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
                {
                    MenuMode(false);
                }
            }
        }

        //when ball is spawned wait until it set up properly to get controll
        if(currentPlayerState == PlayerState.getReady)
        {
            actualTimer -= Time.deltaTime;
            if(actualTimer<0)
            {
                currentPlayerState = PlayerState.inGame;
            }
            
        }
    }



    public void IncreaseSpeedValue(float value)
    {
        actualSpeedValue *= value;
    }

    public void SetRunTime(bool value)
    {
        runTimer = value;
    }

    public void SetActualTimer(float value)
    {
        actualTimer = value;
    }

    public void SetThrowUsed (bool value)
    {
        throwUsed = value;
    }

    public void PlayerThrowAction(bool value)
    {
        if(player_GolfBall!= null && player_GolfBall == lastSpawned_player_GolfBall)
        {
            GolfBall golfBall = player_GolfBall.GetComponent<GolfBall>();
            Trajectory trajectory = player_GolfBall.GetComponent<Trajectory>();

            golfBall.Throw(golfBall.CalculateForce(player_GolfBall.transform.position, trajectory.lastMarkerObj.transform.position, 60f));

            actualTimer = ballLifeTime;
            runTimer = true;

            autoThrow = value;
            throwUsed = true;
        }
    }

    private void MenuMode(bool firstLaunch)
    {
        currentPlayerState = PlayerState.inMenu;
        foreach (Button element in menuButtons)
            element.gameObject.SetActive(true);

        runTimer = false;
        autoThrow = false;

        if (!firstLaunch)
            levelGenerator.ShowRecordScore();
        else
            levelGenerator.HideRecordScore();
    }

    public void NewGameStart()
    {
        foreach (Button element in menuButtons)
            element.gameObject.SetActive(false);

        levelGenerator.ResetPoints();
        levelGenerator.HideRecordScore();
        actualSpeedValue = startSpeedValue;
        levelGenerator.NewLevel();
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
         Application.Quit();
    #endif
    }

}
