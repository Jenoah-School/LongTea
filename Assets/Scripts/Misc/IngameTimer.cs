using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IngameTimer : MonoBehaviour
{
    [SerializeField] private float remainingTime = 300f;
    [SerializeField] private bool startCountdownOnStart = true;
    [SerializeField] private UnityEvent onFinish;
    [SerializeField] private TMPro.TextMeshProUGUI timeText = null;
    [SerializeField] private int scorePerSecond = 10;

    private float startTime = 300f;
    private bool hasStarted = false;
    private bool hasEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = remainingTime;
        if (startCountdownOnStart) hasStarted = true;
        if (timeText != null) InvokeRepeating("UpdateLabel", 0.25f, 1f);
        UpdateLabel();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            remainingTime -= Time.deltaTime;
        }

        if(!hasEnded && remainingTime <= 0)
        {
            hasEnded = true;
            onFinish.Invoke();
        }
    }

    public void StartTimer()
    {
        hasStarted = true;
    }

    public void UpdateLabel()
    {
        int minutesLeft = Mathf.FloorToInt(remainingTime / 60);
        int secondsLeft = Mathf.FloorToInt(remainingTime % 60);
        timeText.text = minutesLeft.ToString("00") + ":" + secondsLeft.ToString("00");
    }

    public int GetRewardScore()
    {
        return Mathf.RoundToInt(((int)startTime - (int)remainingTime) * scorePerSecond);
    }

    public void AddRewardScore()
    {
        ScoreManager.instance.IncreaseScore(GetRewardScore());
    }
}
