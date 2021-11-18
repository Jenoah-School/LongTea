using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private int score = 0;
    [SerializeField] private int totalScore = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateScoreLabel();
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        UpdateScoreLabel();
    }

    public void SetTotalScore(int newTotalScore)
    {
        totalScore = newTotalScore;
        UpdateScoreLabel();
    }

    public void IncreaseScore(int incrementAmount)
    {
        score += incrementAmount;
        UpdateScoreLabel();
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public int GetScore()
    {
        return score;
    }

    public void UpdateScoreLabel()
    {
        scoreText.text = $"{score}/{totalScore}";
    }
}
