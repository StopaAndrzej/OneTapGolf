using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalNextLevel : MonoBehaviour
{
    [HideInInspector] public LevelGenerator levelGenerator;

    public void NextLevel()
    {
        levelGenerator.NewLevel();
    }
}
