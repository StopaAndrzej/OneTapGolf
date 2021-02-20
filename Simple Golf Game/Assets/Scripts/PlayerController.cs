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

    private enum PlayerState { inMenu, inGame}
    private PlayerState currentPlayerState;

    [HideInInspector] public GameObject player_GolfBall;

    private void Start()
    {
        levelGenerator.CalibrateWorldPosWithCurrentAspectRatio();
        MenuMode();
    }

    private void Update()
    {
        if(currentPlayerState == PlayerState.inGame && player_GolfBall!=null)
        {
            if(Input.GetKeyDown(KeyCode.Space) && !player_GolfBall.GetComponent<Trajectory>().CheckIfThrowWasUsed())
            {
                player_GolfBall.GetComponent<Trajectory>().RunIncreaseDistance(actualSpeedValue);
            }

            if(Input.GetKeyUp(KeyCode.Space))
            {
                GolfBall golfBall = player_GolfBall.GetComponent<GolfBall>();
                Trajectory trajectory = player_GolfBall.GetComponent<Trajectory>();
                golfBall.Throw(golfBall.CalculateForce(player_GolfBall.transform.position, trajectory.lastMarkerObj.transform.position, 45f));

                actualTimer = ballLifeTime;
                runTimer = true;
            }
        }

        //GameOver timer: 
        //1. ball velocity = 0 and no goal
        //2. ball out of the screen
        //3. ball exceeded life time after throw
        if(runTimer)
        {
            actualTimer -= Time.deltaTime;
            if(actualTimer < 0 || player_GolfBall.transform.position.x > levelGenerator.right_worldPosScreenBorder || player_GolfBall.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            {
                MenuMode();
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

    private void MenuMode()
    {
        currentPlayerState = PlayerState.inMenu;
        foreach (Button element in menuButtons)
            element.gameObject.SetActive(true);

        runTimer = false;
    }

    public void NewGameStart()
    {
        currentPlayerState = PlayerState.inGame;
        foreach (Button element in menuButtons)
            element.gameObject.SetActive(false);

        levelGenerator.ResetPoints();
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
