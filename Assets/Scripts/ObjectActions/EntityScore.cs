using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityScore : MonoBehaviour
{
    [SerializeField] private int scoreAmount = 1;

    // Start is called before the first frame update
    void Start()
    {
        ScoreManager.instance.IncreaseTotalMinerals(1);
    }

    public void AddToScore()
    {
        ScoreManager.instance.IncreaseScore(scoreAmount);
    }
}
