using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private EntityHealth playerHealth;

    [Header("Visuals")]
    [SerializeField] private Color enabledColor = Color.white;
    [SerializeField] private Color disabledColor = Color.gray;
    [SerializeField] private List<Image> healthIcons = new List<Image>();

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < healthIcons.Count; i++)
        {
            healthIcons[i].color = i + 1 <= playerHealth.health ? enabledColor : disabledColor;
        }
    }
}
