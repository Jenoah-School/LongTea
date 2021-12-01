using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    int currentScreen = 1;

    public bool secondaryFinish = false;

    [SerializeField] private UnityEvent finish;
    [SerializeField] private UnityEvent secondary;    

    public void SetSecondaryToTrue()
    {
        secondaryFinish = true;
    }

    public void OnClickTutorialScreen()
    {
        transform.GetChild(currentScreen - 1).gameObject.SetActive(false);
        currentScreen++;     
        transform.GetChild(currentScreen - 1).gameObject.SetActive(true);

        if(currentScreen == 8)
        {
            if (!secondaryFinish)
            {
                finish.Invoke();
            }
            else
            {
                secondary.Invoke();
            }
            currentScreen = 1;
        }
    }
}
