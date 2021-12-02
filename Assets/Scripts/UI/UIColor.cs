using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIColor : MonoBehaviour
{
    [SerializeField] private Image imageReference = null;
    [SerializeField] private Color targetColor = Color.white;

    public void SetToColor(float fadeSpeed)
    {
        imageReference.DOColor(targetColor, fadeSpeed).SetUpdate(true);
    }
}
