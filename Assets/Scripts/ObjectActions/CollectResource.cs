using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class CollectResource : MonoBehaviour
{
    [SerializeField] private float requiredTimeToCollect = 2f;
    [SerializeField] private Image timerImage = null;

    //Events
    [SerializeField] private UnityEvent onCollect = null;
    [SerializeField] private UnityEvent onEnterRadius = null;
    [SerializeField] private UnityEvent onExitRadius = null;
    [SerializeField] private EntityScore entityScore = null;

    private bool isInRadius = false;
    private bool hasBeenCollected = false;

    private float inRangeTimer = 0f;

    private void Start()
    {
        timerImage.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenCollected) return;
        if (isInRadius)
        {
            inRangeTimer += Time.deltaTime;
            timerImage.fillAmount = Mathf.Clamp01(inRangeTimer / requiredTimeToCollect);
            if(inRangeTimer >= requiredTimeToCollect)
            {
                hasBeenCollected = true;
                if (entityScore != null)
                {
                    entityScore.AddToScore();
                }
                else
                {
                    ScoreManager.instance.IncreaseScore(1);
                }
                onCollect.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenCollected) return;
        if (other.CompareTag("Player"))
        {
            isInRadius = true;
            onEnterRadius.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasBeenCollected) return;
        if (other.CompareTag("Player"))
        {
            isInRadius = false;
            inRangeTimer = 0f;
            timerImage.DOFillAmount(0f, 0.4f);
            onExitRadius.Invoke();
        }
    }
}
