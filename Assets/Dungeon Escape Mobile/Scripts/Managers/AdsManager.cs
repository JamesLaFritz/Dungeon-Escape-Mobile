using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener
{
    private static AdsManager _instance;

    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
    public static AdsManager Instance => _instance;

    [SerializeField] string m_androidGameId;
    [SerializeField] string m_iOsGameId;
    [SerializeField] bool m_testMode = true;
    [SerializeField] bool m_enablePerPlacementMode = true;
    private string m_gameId;

    private void Awake()
    {
        if (_instance != null)
        {
            string gameObjectName =
                // ReSharper disable Unity.InefficientPropertyAccess
                _instance.transform.parent != null ? _instance.transform.parent.name : _instance.name;
            // ReSharper restore Unity.InefficientPropertyAccess
            Debug.LogWarning($"There is already an instance of the Game Manager {gameObjectName}!");
            enabled = false;
            Destroy(this);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(this);

        InitializeAds();
    }

    private void InitializeAds()
    {
        m_gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? m_iOsGameId
            : m_androidGameId;
        Advertisement.Initialize(m_gameId, m_testMode, m_enablePerPlacementMode, this);
    }

    #region Implementation of IUnityAdsInitializationListener

    /// <inheritdoc />
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    /// <inheritdoc />
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    #endregion
}
