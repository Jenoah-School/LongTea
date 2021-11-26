using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textField;

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 10 == 0) textField.text = "FPS: " + ((int)(1f / Time.unscaledDeltaTime)).ToString();
    }
}
