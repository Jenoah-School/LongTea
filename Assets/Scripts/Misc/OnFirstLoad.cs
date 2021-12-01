using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnFirstLoad : MonoBehaviour
{
    [SerializeField] private UnityEvent firstLoad;
    [SerializeField] private UnityEvent notFirstLoad;

    public void ExecuteAction()
    {
        if (!PlayerPrefs.HasKey("firstLoad"))
        {
            firstLoad.Invoke();
            PlayerPrefs.SetInt("firstLoad", 1);
        }
        else
        {
            notFirstLoad.Invoke();
        }
    }
}
