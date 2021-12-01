using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Pool;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float lifeTime = 4f;
    [SerializeField] private float growSize = 2f;
    [SerializeField] private bool unparentAndDestroy = false;
    [SerializeField] private float moveSpeed = 1f;

    [Header("References")]
    [SerializeField] private FadeUI fadeReference = null;
    [SerializeField] private EntityScore entityScore = null;
    [SerializeField] private TMPro.TextMeshProUGUI textReference = null;

    private void Start()
    {
        if (entityScore != null) textReference.text = "+" + entityScore.GetScore().ToString();
    }

    public void ShowScore()
    {
        gameObject.transform.DOScale(transform.localScale * growSize, lifeTime);
        Vector3 tempPos = transform.position;
        if (unparentAndDestroy) transform.SetParent(null, true);
        transform.position = tempPos;
        transform.DOMove(transform.position + transform.TransformDirection(Vector3.up) * moveSpeed, lifeTime, false);
        StartCoroutine(ShowScoreEnum());
    }

    IEnumerator ShowScoreEnum()
    {
        fadeReference.FadeIn(fadeSpeed);
        yield return new WaitForSeconds(lifeTime - fadeSpeed);
        fadeReference.FadeOut(fadeSpeed);
        yield return new WaitForSeconds(fadeSpeed);
        if (unparentAndDestroy) LeanPool.Despawn(gameObject);
    }
}
