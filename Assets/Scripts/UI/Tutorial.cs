using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    int currentScreen = 1;
    [SerializeField] private UnityEvent finish;

    public void OnClickTutorialScreen()
    {
        transform.GetChild(currentScreen - 1).gameObject.SetActive(false);
        currentScreen++;     
        transform.GetChild(currentScreen - 1).gameObject.SetActive(true);

        if(currentScreen == 8)
        {
            finish.Invoke();
            currentScreen = 1;
        }
    }
}
