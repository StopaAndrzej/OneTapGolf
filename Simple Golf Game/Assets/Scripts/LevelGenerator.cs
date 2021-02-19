using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject groundPrefab;
    [SerializeField] GameObject golfHolePrefab;
    [SerializeField] GameObject golfBallPrefab;
    private PlayerController playerController;

    private float left_worldPosScreenBorder, right_worldPosScreenBorder,
                    up_worldPosScreenBorder, down_worldPosScreenBorder;
    public float buildGroundLevelYOffset = 0f;

    private int level = 0;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();    
    }

    public void NewLevel()
    {
        level++;
        ResetLevel();
        GenerateGround();
        GenerateRandomGolfHole();
        SpawnGolfBall();
    }

    //destroy every level generated object
    void ResetLevel()
    {
        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);
    }

    void GenerateGround()
    {
        if (groundPrefab!=null)
        {
            for(float currentBlockPivotPosX = left_worldPosScreenBorder; (currentBlockPivotPosX - 0.64f)<= right_worldPosScreenBorder; currentBlockPivotPosX += 1.28f)
            {
                GameObject groundBlockObj = (GameObject)Instantiate(groundPrefab, transform);
                groundBlockObj.transform.position = new Vector2(currentBlockPivotPosX, buildGroundLevelYOffset);
            }

        }
    }

    //rand <from the middle of the screen to the border - 1f(boarder offset to eliminate spawn element behind the screen)>
    void GenerateRandomGolfHole()
    {
        if(golfHolePrefab!=null)
        {
            GameObject golfHoleObj = (GameObject)Instantiate(golfHolePrefab, transform);
            golfHoleObj.transform.position = new Vector2(Random.Range(0, right_worldPosScreenBorder - 1.0f), buildGroundLevelYOffset+0.64f);
        }
    }

    //spawn ball  1/3 distance between left screen boarder and middle sceen point
    void SpawnGolfBall()
    {
        if(golfBallPrefab!=null)
        {
            GameObject golfBallObj = (GameObject)Instantiate(golfBallPrefab, transform);
            playerController.player_GolfBall = golfBallObj;

            Vector2 spawnPoint = new Vector2(left_worldPosScreenBorder/3 * 2, 0);
            golfBallObj.transform.position = spawnPoint;
        }
    }

    //check only once when the game is started
    public void CalibrateWorldPosWithCurrentAspectRatio()
    {
        //reset camera
        Camera.main.transform.position = new Vector3(0, 0, -10);

        //find world pos of display boarders with current game aspect ratio to start generate level blocks between these two points
        Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        left_worldPosScreenBorder = -stageDimensions.x;
        right_worldPosScreenBorder = stageDimensions.x;
    }

}
