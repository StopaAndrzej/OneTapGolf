using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private GameObject trajectoryMarkerPrefab;

    [HideInInspector] public GameObject lastMarkerObj;
    private GameObject[] markers;

    public int maxNumberOfMarkers;
    private int actualNumberOfMarkers;

    [HideInInspector] public bool allowToIncreaseDistance;
    [HideInInspector] public float throwDistance;
    [HideInInspector] public float increasingSpeed;

    private Vector2 startMarkerPos;
    private Vector2 endMarkerPos;

    private void Start()
    {
        lastMarkerObj = Instantiate(trajectoryMarkerPrefab);
        isVisible(lastMarkerObj, false);

        //markers = new GameObject[maxNumberOfMarkers];
        //lastMarkerObj = Instantiate(trajectoryMarkerPrefab, parentFolder);
        //isVisible(lastMarkerObj, false);

        //for (int i=0; i< maxNumberOfMarkers; i++)
        //{
        //    GameObject singleMarker = Instantiate(trajectoryMarkerPrefab, parentFolder);
        //    markers[i] = singleMarker;
        //    isVisible(markers[i], false);
        //}

        //throwDistance = 0;
        //actualNumberOfMarkers = 0;
    }

    //private void Update()
    //{
    //    //if(Input.GetKey(KeyCode.Space))
    //    //{


    //    //   // throwDistance += Time.deltaTime * powerSpeed;
    //    //    //Vector2 tmp = CalculateVelocity(transform.position, throwDistance ,1f);

    //    //    //startMarkerPos = transform.position;
    //    //    //endMarkerPos = new Vector2(startMarkerPos.x + throwDistance, levelGenerator.buildGroundLevelYOffset + 0.64f);
    //    //    //lastMarkerObj.transform.position = endMarkerPos;
    //    //    //isVisible(lastMarkerObj, true);

    //    //    //actualNumberOfMarkers = (int)throwDistance % maxNumberOfMarkers;

    //    //    //for(int i=0; i<actualNumberOfMarkers; i++)
    //    //    //{
    //    //    //    Vector2 pos = (Vector2)transform.position + (new Vector2(1,1).normalized * throwDistance *powerSpeed *0.1f * (i+1)) + 0.5f *Physics2D.gravity * 0.5f *0.5f;
    //    //    //    markers[i].transform.position = pos;
    //    //    //   // markers[i].transform.position = new Vector2((throwDistance / (actualNumberOfMarkers + 1)) * (i+1) + startMarkerPos.x, levelGenerator.buildGroundLevelYOffset + 0.64f);
    //    //    //    isVisible(markers[i], true);
    //    //    //}
    //    //}

    //    //if(Input.GetKeyUp(KeyCode.Space))
    //    //{
    //    //    ResetMarkers();
    //    //}
    //}

    private void Update()
    {
        if(allowToIncreaseDistance)
        {
            startMarkerPos = transform.position;
            throwDistance += Time.deltaTime * increasingSpeed;
            endMarkerPos = new Vector2(startMarkerPos.x + throwDistance, -3.2f + 0.64f);
            lastMarkerObj.transform.position = endMarkerPos;
        }
    }

    //public IEnumerator IncreaseTheThrowDistance(bool value)
    //{
    //    isVisible(lastMarkerObj, true);
    //    Vector2 startMarkerPos = transform.position;
    //    Vector2 endMarkerPos;

    //    allowToIncreaseDistance = value;
    //    while (allowToIncreaseDistance)
    //    {
    //        throwDistance += Time.deltaTime * increasingSpeed;
    //        //endMarkerPos = new Vector2(startMarkerPos.x + throwDistance, levelGenerator.buildGroundLevelYOffset + 0.64f);
    //        endMarkerPos = new Vector2(startMarkerPos.x + throwDistance, -3.2f + 0.64f);
    //        lastMarkerObj.transform.position = endMarkerPos;
    //        yield return null;
    //    }
    //}

    public void isVisible(GameObject obj, bool value)
    {
        obj.SetActive(value);
    }
    public void ResetMarkers()
    {
        isVisible(lastMarkerObj, false);
        for (int i = 0; i < actualNumberOfMarkers; i++)
        {
            isVisible(markers[i], false);
        }

        actualNumberOfMarkers = 0;
        throwDistance = 0;
    }
}
