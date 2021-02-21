using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    private Animator animator;
    [HideInInspector] public LevelGenerator levelGenerator;
    private bool collisionFound;                                //can be done only once (protection)

    private void Start()
    {
        foreach(Transform child in transform)
            if(child.GetComponent<Animator>())
            {
                animator = child.GetComponent<Animator>();
                break;
            }

        collisionFound = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(!collisionFound)
            {
                collisionFound = !collisionFound;
                //timer of ball lifetime after throw
                levelGenerator.StopRunTimer();
                levelGenerator.AddPoint();
                animator.Play("Goal");
            }
        }
    }
}
