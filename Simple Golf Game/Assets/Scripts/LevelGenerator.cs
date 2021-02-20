using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject groundPrefab;
    [SerializeField] GameObject golfHolePrefab;
    [SerializeField] GameObject golfBallPrefab;

    [SerializeField] private Text scoreTxt;
    [SerializeField] Transform parentGroundBlocksFolder;
    private PlayerController playerController;

    [HideInInspector] public float left_worldPosScreenBorder, right_worldPosScreenBorder,
                      up_worldPosScreenBorder, down_worldPosScreenBorder;

    public float buildGroundLevelYOffset = 0f;
    
    //required to set the ground collider correctly
    private Vector2 golfHolePos; 
    [HideInInspector] public int points = 0;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        scoreTxt.gameObject.SetActive(false);
    }

    public void NewLevel()
    {
        ResetLevel();
        UpdatePointsAndDifficulty(points);
        GenerateRandomGolfHole();                       //create golfhole first to set ground colliders properly
        GenerateGround();
        SpawnGolfBall();
    }

    //destroy every level generated object
    void ResetLevel()
    {
        //remove objects
        foreach(Transform child in parentGroundBlocksFolder)
            GameObject.Destroy(child.gameObject);

        //remove attached components (ex. gen.grounds edge colliders)
        foreach (Component comp in parentGroundBlocksFolder.gameObject.GetComponents<Component>())
            if (!(comp is Transform))
                Destroy(comp);

    }

    void GenerateGround()
    {
        if (groundPrefab!=null)
        {
            for(float currentBlockPivotPosX = left_worldPosScreenBorder; (currentBlockPivotPosX - 0.64f)<= right_worldPosScreenBorder; currentBlockPivotPosX += 1.28f)
            {
                GameObject groundBlockObj = (GameObject)Instantiate(groundPrefab, parentGroundBlocksFolder);
                groundBlockObj.transform.position = new Vector2(currentBlockPivotPosX, buildGroundLevelYOffset);
            }

            EdgeCollider2D collider1 = parentGroundBlocksFolder.gameObject.AddComponent<EdgeCollider2D>();
            EdgeCollider2D collider2 = parentGroundBlocksFolder.gameObject.AddComponent<EdgeCollider2D>();

            Vector2[] verticles = new Vector2[2];
            verticles[0] = new Vector2(left_worldPosScreenBorder, buildGroundLevelYOffset + 0.64f);
            verticles[1] = new Vector2(golfHolePos.x - 0.45f, buildGroundLevelYOffset + 0.64f);
            collider1.points = verticles;

            verticles[0] = new Vector2(golfHolePos.x + 0.45f, buildGroundLevelYOffset + 0.64f);
            verticles[1] = new Vector2(right_worldPosScreenBorder, buildGroundLevelYOffset + 0.64f);
            collider2.points = verticles;
        }
    }

    //rand <from the middle of the screen to the border - 1f(boarder offset to eliminate spawn element behind the screen)>
    private void GenerateRandomGolfHole()
    {
        if(golfHolePrefab!=null)
        {
            GameObject golfHoleObj = (GameObject)Instantiate(golfHolePrefab, parentGroundBlocksFolder);
            //attach levelGen script to spawned object(next level start)
            golfHoleObj.GetComponent<GoalDetector>().levelGenerator = this.GetComponent<LevelGenerator>();
            foreach (Transform child in golfHoleObj.transform)
                if (child.GetComponent<GoalNextLevel>())
                {
                    child.GetComponent<GoalNextLevel>().levelGenerator = this.GetComponent<LevelGenerator>();
                    break;
                }
                

            golfHolePos = new Vector2(Random.Range(0, right_worldPosScreenBorder - 1.0f), buildGroundLevelYOffset + 0.64f);
            golfHoleObj.transform.position = golfHolePos;
        }
    }

    //spawn ball  1/3 distance between left screen boarder and middle sceen point
    private void SpawnGolfBall()
    {
        if(golfBallPrefab!=null)
        {
            GameObject golfBallObj = (GameObject)Instantiate(golfBallPrefab, parentGroundBlocksFolder);
            playerController.player_GolfBall = golfBallObj;

            Vector2 spawnPoint = new Vector2(left_worldPosScreenBorder/3 * 2, 0);
            golfBallObj.transform.position = spawnPoint;
        }
    }

    private void UpdatePointsAndDifficulty(int points)
    {
        scoreTxt.gameObject.SetActive(true);
        if(scoreTxt != null)
        {
            scoreTxt.text = "SCORE: " + points;
        }

        //x1.2f
        playerController.IncreaseSpeedValue(1.2f);
    }

    public void AddPoint()
    {
        points++;
    }
    public int GetPoints()
    {
        return points;
    }
    public void ResetPoints()
    {
        points = 0;
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
        up_worldPosScreenBorder = stageDimensions.y;
        down_worldPosScreenBorder = -stageDimensions.y;
    }

    public void StopRunTimer()
    {
        playerController.SetRunTime(false);
    }

}
