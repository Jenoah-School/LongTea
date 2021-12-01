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
    [SerializeField] private int totalVisualMinerals = 0;
    [SerializeField] private int mineralAmount = 0;
    [SerializeField] private int totalMinerals = 0;
    [SerializeField] private int totalCollectedMinerals = 0;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreTextGameOver;
    [SerializeField] private TextMeshProUGUI scoreTextWin;
    [SerializeField] private TextMeshProUGUI mineralText;
    [SerializeField] private TextMeshProUGUI mineralTextGameOver;
    [SerializeField] private TextMeshProUGUI mineralTextWin;
    [SerializeField] private float squishTime = 0.4f;
    [SerializeField] private float squishAmount = 0.1f;

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
    }

    public void SetMineralCount(int newMineralScore)
    {
        mineralAmount = newMineralScore;
        UpdateMineralLabelAnimated();
    }

    public void SetVisualTotalMinerals(int newVisualTotalScore)
    {
        totalVisualMinerals = newVisualTotalScore;
        totalMinerals += newVisualTotalScore;
        UpdateMineralLabel();
    }
    public void IncreaseVisualTotalMinerals(int incrementAmount)
    {
        totalVisualMinerals += incrementAmount;
        totalMinerals += incrementAmount;
        UpdateMineralLabel();
    }

    public void IncreaseScore(int incrementAmount)
    {
        score += incrementAmount;
        UpdateScoreLabelAnimated();
    }

    public int GetVisualTotalMinerals()
    {
        return totalVisualMinerals;
    }

    public int GetScore()
    {
        return score;
    }

    public void CollectMineral()
    {
        mineralAmount += 1;
        totalCollectedMinerals += 1;
        UpdateMineralLabelAnimated();
    }

    public void UpdateMineralLabel()
    {
        mineralText.text = $"{mineralAmount} / {totalVisualMinerals}";
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
        mineralText.text = $"{mineralAmount} / {totalVisualMinerals}";
        mineralText.DOComplete();
        mineralText.rectTransform.DOPunchScale(Vector3.one * squishAmount, squishTime);
    }

    public void UpdateExternalLabels() {
        scoreTextGameOver.text = score.ToString("000000"); ;
        scoreTextWin.text = score.ToString("000000"); ;
        mineralTextGameOver.text = $"{totalCollectedMinerals} / {totalMinerals}";
        mineralTextWin.text = $"{totalCollectedMinerals} / {totalMinerals}";
        SafeScore();
    }

    public void SafeScore()
    {
        int highScore = PlayerPrefs.GetInt("Highscore", 0);
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("Highscore", highScore);
        }
        int totalMineralsScore = PlayerPrefs.GetInt("TotalCollectedMinerals", 0);
        if(totalCollectedMinerals > totalMineralsScore)
        {
            totalMineralsScore = totalCollectedMinerals;
            PlayerPrefs.SetInt("TotalCollectedMinerals", totalMineralsScore);
            PlayerPrefs.SetInt("TotalMinerals", totalMinerals);

            Debug.Log($"{PlayerPrefs.GetInt("TotalCollectedMinerals")} / {PlayerPrefs.GetInt("TotalMinerals")}");
        }
    }
}
