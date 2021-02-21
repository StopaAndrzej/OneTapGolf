using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private GameObject trajectoryMarkerPrefab;
    private LevelGenerator levelGenerator;
    private GolfBall golfBallScript;

    [HideInInspector] public GameObject lastMarkerObj;
    private GameObject[] markers;

    public int maxNumberOfMarkers;
    private int actualNumberOfMarkers;

    public float throwDistance;
    private float increasingSpeed;
    private float maxDistanceValue;
    private float colorChangeTime;

    private Vector2 startMarkerPos;
    private Vector2 endMarkerPos;

    private bool throwChanceUsed;
    private bool runIncrease;

    private void Start()
    {
        golfBallScript = this.GetComponent<GolfBall>();
        lastMarkerObj = Instantiate(trajectoryMarkerPrefab, transform);
        lastMarkerObj.SetActive(false);
        runIncrease = false;
        throwChanceUsed = false;
        throwDistance = 0;

        markers = new GameObject[maxNumberOfMarkers];
        for (int i = 0; i < maxNumberOfMarkers; i++)
        {
            GameObject singleMarker = Instantiate(trajectoryMarkerPrefab, transform);
            markers[i] = singleMarker;
            markers[i].SetActive(false);
        }

        actualNumberOfMarkers = 5;
    }

    private void Update()
    {
        if(runIncrease)
        {
            throwDistance += Time.deltaTime * increasingSpeed;
            endMarkerPos = new Vector2(startMarkerPos.x + throwDistance, -3.2f + 0.64f);
            lastMarkerObj.transform.position = endMarkerPos;

            UpdateMarkers(throwDistance);
            UpdateDistanceColor(throwDistance);

            //if ball reach max distance - auto throw
            if(lastMarkerObj.transform.position.x > maxDistanceValue)
            {
                if (levelGenerator != null)
                    levelGenerator.ForceToPushBall();
            }
        }
    }

    private void UpdateMarkers(float distanceValue)
    {
        for (int i = 0; i < maxNumberOfMarkers; i++)
            markers[i].transform.position = golfBallScript.CalculatePosInTime(this.transform.position, lastMarkerObj.transform.position ,golfBallScript.CalculateForce(this.transform.position, lastMarkerObj.transform.position, 60), maxNumberOfMarkers ,i);

    }

    void UpdateDistanceColor(float value)
    {
        float duration = 5f;
        for(int i=0; i<maxNumberOfMarkers; i++)
        {
            markers[i].GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.yellow, colorChangeTime);
        }

        lastMarkerObj.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.yellow, colorChangeTime);

        colorChangeTime += Time.deltaTime / duration;
    }

    public bool CheckIfThrowWasUsed()
    {
        return throwChanceUsed;
    }

    public void RunIncreaseDistance(float speedValue, float maxDistanceValue)
    {
        increasingSpeed = speedValue;
        this.maxDistanceValue = maxDistanceValue;

        throwChanceUsed = true;
        colorChangeTime = 0;
        startMarkerPos = transform.position;
        lastMarkerObj.SetActive(true);
        for (int i = 0; i < maxNumberOfMarkers; i++)
            markers[i].SetActive(true);
        runIncrease = true;
    }

    public void StopIncreaseDistance()
    {
        runIncrease = false;
        ResetMarkers();
    }

    public void ResetMarkers()
    {
        lastMarkerObj.SetActive(false);
        for (int i = 0; i < maxNumberOfMarkers; i++)
        {
            markers[i].SetActive(false);
        }
        actualNumberOfMarkers = 0;
        throwDistance = 0;
    }

    public void GetAccessToLevelGenerator(LevelGenerator levelGen)
    {
        levelGenerator = levelGen;
    }
}
