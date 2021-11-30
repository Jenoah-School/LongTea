using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnFirstLoad : MonoBehaviour
{
    [SerializeField] private UnityEvent firstLoad;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("firstLoad"))
        {
            firstLoad.Invoke();
            PlayerPrefs.SetInt("firstLoad", 1);
        }
    }
}
