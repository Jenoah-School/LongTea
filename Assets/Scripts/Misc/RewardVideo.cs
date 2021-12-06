using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardVideo : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidGameID = "XXXXXX";
    [SerializeField] private string iOSGameID = "XXXXXX";
    [SerializeField] private string androidAdUnitID = "Rewarded_Android";
    [SerializeField] private string iOSAdUnitID = "Rewarded_iOS";
    [SerializeField] private UIAnimations rewardVideoButton;
    [Space(20)]
    [SerializeField] private UnityEvent onCompleteAd;

    [SerializeField] private bool testMode = true;

    private string _adUnitId;
    private string _gameId;

    private void Awake()
    {
        InitializeAds();
    }

    #region Initialization

    public void InitializeAds()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _gameId = iOSGameID;
            _adUnitId = iOSAdUnitID;
        }
        else
        {
            _gameId = androidGameID;
            _adUnitId = androidAdUnitID;
        }

        Advertisement.Initialize(_gameId, testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    #endregion

    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + _adUnitId);
        Advertisement.Show(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Ad Loaded: " + placementId);

        if (placementId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            rewardVideoButton.OnSuccesfulClick.AddListener(ShowAd);
            // Enable the button for users to click:
            //rewardVideoButton.isInteractable = true;
        }
        else
        {
            Debug.Log($"Can't load ad: {placementId} vs {_adUnitId}");
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
        rewardVideoButton.isInteractable = false;
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        rewardVideoButton.isInteractable = false;
        onCompleteAd.Invoke();
    }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");

            onCompleteAd.Invoke();

            Advertisement.Load(_adUnitId, this);
        }
    }

    void OnDestroy()
    {
        // Clean up the button listeners:
        rewardVideoButton.OnSuccesfulClick.RemoveAllListeners();
    }
}
