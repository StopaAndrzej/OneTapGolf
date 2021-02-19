using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private List<Button> menuButtons;
    [SerializeField] private Text levelTxt;

    private enum PlayerState { inMenu, inGame, gameOver}
    private PlayerState currentPlayerState;

    [HideInInspector] public GameObject player_GolfBall;

    private void Start()
    {
        levelGenerator.CalibrateWorldPosWithCurrentAspectRatio();
        currentPlayerState = PlayerState.inMenu;
        levelTxt.gameObject.SetActive(false);
        foreach (Button element in menuButtons)
            element.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(currentPlayerState == PlayerState.inGame && player_GolfBall!=null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player_GolfBall.GetComponent<Trajectory>().allowToIncreaseDistance = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                GolfBall golfBall = player_GolfBall.GetComponent<GolfBall>();
                Trajectory trajectory = player_GolfBall.GetComponent<Trajectory>();

                player_GolfBall.GetComponent<Trajectory>().allowToIncreaseDistance = false;
                golfBall.Throw(golfBall.CalculateForce(transform.position, trajectory.lastMarkerObj.transform.position, 45f));
            }
        }
    }

    public void NewGameStart()
    {
        currentPlayerState = PlayerState.inGame;
        levelTxt.gameObject.SetActive(true);
        foreach (Button element in menuButtons)
            element.gameObject.SetActive(false);
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
