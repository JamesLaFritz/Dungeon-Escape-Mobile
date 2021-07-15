using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
    public static GameManager Instance => _instance;

    [SerializeField] private IntReference m_playerGems;
    [SerializeField] private int m_startingGems = 3;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
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
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        // ReSharper disable once PossibleNullReferenceException
        m_playerGems.Value = m_startingGems;
    }
}
