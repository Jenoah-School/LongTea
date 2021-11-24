using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class OnFinishVideo : MonoBehaviour
{
    [SerializeField] private UnityEvent onFinish;
    private VideoPlayer videoPlayer;
    private float videoLength = 0f;
    private bool videoHasPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoLength = ((float)videoPlayer.length);
    }

    public void StartCountdown()
    {
        Debug.Log("Starting ad");
        StartCoroutine(ExecuteOnFinish());
    }

    IEnumerator ExecuteOnFinish()
    {
        Debug.Log("Ad countdown started");
        yield return new WaitForSecondsRealtime(videoLength);
        onFinish.Invoke();
        Debug.Log("Ad finished");
    }


}
