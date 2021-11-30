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
    [SerializeField] private int totalMinerals = 0;
    [SerializeField] private int mineralAmount = 0;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreTextGameOver;
    [SerializeField] private TextMeshProUGUI scoreTextWin;
    [SerializeField] private TextMeshProUGUI mineralText;
    [SerializeField] private TextMeshProUGUI mineralTextGameOver;
    [SerializeField] private TextMeshProUGUI mineralTextWin;
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
        UpdateMineralLabel();
        UpdateScoreLabel();
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        UpdateScoreLabelAnimated();
        CheckScore();
    }

    public void SetMineralCount(int newMineralScore)
    {
        mineralAmount = newMineralScore;
        UpdateScoreLabelAnimated();
        CheckScore();
    }

    public void SetTotalMinerals(int newTotalScore)
    {
        totalMinerals = newTotalScore;
        UpdateMineralLabel();
        CheckScore();
    }
    public void IncreaseTotalMinerals(int incrementAmount)
    {
        totalMinerals += incrementAmount;
        UpdateMineralLabel();
        CheckScore();
    }

    public void IncreaseScore(int incrementAmount)
    {
        score += incrementAmount;
        UpdateScoreLabelAnimated();
        CheckScore();
    }

    public int GetTotalMinerals()
    {
        return totalMinerals;
    }

    public int GetScore()
    {
        return score;
    }

    public void CollectMineral()
    {
        mineralAmount += 1;
        UpdateMineralLabelAnimated();
    }

    public void UpdateMineralLabel()
    {
        mineralText.text = $"{mineralAmount} / {totalMinerals}";
    }

    public void UpdateScoreLabel()
    {
        scoreText.text = score.ToString("000000");
    }

    public void UpdateScoreLabelAnimated()
    {
        scoreText.text = score.ToString("000000");
        scoreText.DOComplete();
        scoreText.rectTransform.DOPunchScale(Vector3.one * squishAmount, squishTime);
    }

    public void UpdateMineralLabelAnimated()
    {
        mineralText.text = $"{mineralAmount} / {totalMinerals}";
        mineralText.DOComplete();
        mineralText.rectTransform.DOPunchScale(Vector3.one * squishAmount, squishTime);
    }

    public void CheckScore()
    {
        if(!maxScoreReached && score >= totalMinerals)
        {
            OnMaxScore.Invoke();
            maxScoreReached = true;
        }
    }

    public void UpdateExternalLabels() {
        scoreTextGameOver.text = score.ToString("000000"); ;
        scoreTextWin.text = score.ToString("000000"); ;
        mineralTextGameOver.text = $"{mineralAmount} / {totalMinerals}";
        mineralTextWin.text = $"{mineralAmount} / {totalMinerals}";
    }
}
