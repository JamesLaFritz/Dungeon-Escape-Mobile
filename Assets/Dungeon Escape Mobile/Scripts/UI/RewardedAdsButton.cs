using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string m_androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string m_iOsAdUnitId = "Rewarded_iOS";
    private string m_adUnitId;
    private Button m_showAdButton;

    [SerializeField] private IntReference m_playerGems;
    [SerializeField] private int m_rewardAmount = 100;

    private void Awake()
    {
        // Get the Ad Unit ID for the current platform:
        m_adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? m_iOsAdUnitId
            : m_androidAdUnitId;

        m_showAdButton = GetComponent<Button>();
        //Disable button until ad is ready to show
        m_showAdButton.interactable = false;
    }

    private void Start()
    {
        LoadAd();
    }

    private void OnDestroy()
    {
        // Clean up the button listeners:
        m_showAdButton.onClick.RemoveAllListeners();
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        Debug.Log("Loading Ad: " + m_adUnitId);
        Advertisement.Load(m_adUnitId, this);
    }

    // Implement a method to execute when the user clicks the button.
    public void ShowAd()
    {
        // Disable the button:
        m_showAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(m_adUnitId, this);
    }

    #region Implementation of IUnityAdsLoadListener

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(m_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            m_showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            m_showAdButton.interactable = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    #endregion

    #region Implementation of IUnityAdsLoadListener

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(m_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            m_playerGems.Value += m_rewardAmount;
        }
    }

    #endregion
}