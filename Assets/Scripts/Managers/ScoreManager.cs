using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    [SerializeField] private int score = 0;
    [SerializeField] private int totalScore = 0;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float squishTime = 0.4f;
    [SerializeField] private float squishAmount = 0.1f;

    [Header("Events")]
    [SerializeField] private UnityEvent OnMaxScore;

    private bool maxScoreReached = false;

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
        UpdateScoreLabelAnimated();
        CheckScore();
    }

    public void SetTotalScore(int newTotalScore)
    {
        totalScore = newTotalScore;
        UpdateScoreLabel();
        CheckScore();
    }

    public void IncreaseScore(int incrementAmount)
    {
        score += incrementAmount;
        UpdateScoreLabelAnimated();
        CheckScore();
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

    public void UpdateScoreLabelAnimated()
    {
        scoreText.text = $"{score}/{totalScore}";
        scoreText.rectTransform.DOPunchScale(Vector3.one * squishAmount, squishTime);
    }

    public void CheckScore()
    {
        if(!maxScoreReached && score >= totalScore)
        {
            OnMaxScore.Invoke();
            maxScoreReached = true;
        }
    }
}
