using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityScore : MonoBehaviour
{
    [SerializeField] private int scoreAmount = 1;

    public void AddToScore()
    {
        ScoreManager.instance.IncreaseScore(scoreAmount);
    }

    public void AddToMineral()
    {
        ScoreManager.instance.CollectMineral();
    }
}
