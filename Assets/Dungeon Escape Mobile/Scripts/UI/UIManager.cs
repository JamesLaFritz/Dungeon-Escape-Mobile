using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIManager : MonoBehaviour
{
    #region Singelton

    private static UIManager _instance;

    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
    public static UIManager Instance => _instance;

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
            Debug.LogWarning($"There is already an instance of the UI Manager {gameObjectName}!");
            enabled = false;
            Destroy(this);
            return;
        }

        _instance = this;
    }

    #endregion

    [SerializeField] private GameObject m_ShopUI;
    private bool m_hasShopUI;
    [SerializeField] private InputSystemUIInputModule m_uiInputModule;

    [SerializeField] private BoolReference m_playerInShop;
    private bool m_shopActive;

    [SerializeField] private Message m_message;
    private bool m_hasMessage;

    private void CancelActionPerformed(InputAction.CallbackContext obj)
    {
        System.Diagnostics.Debug.Assert(m_playerInShop != null, nameof(m_playerInShop) + " != null");
        m_playerInShop.Value = false;
    }


    /// <summary>
    /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    ///
    /// This function can be a coroutine.
    /// </summary>
    private void Start()
    {
        if (m_uiInputModule != null)
        {
            System.Diagnostics.Debug.Assert(m_uiInputModule.cancel != null, "m_uiInputModule.cancel != null");
            System.Diagnostics.Debug.Assert(m_uiInputModule.cancel.action != null, "m_uiInputModule.cancel.action != null");
            m_uiInputModule.cancel.action.performed += CancelActionPerformed;
        }

        m_hasMessage = m_message != null;

        IntShop();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        ShopUI();
    }

    public void DisplayMessage(string message)
    {
        if (!m_hasMessage) return;

        System.Diagnostics.Debug.Assert(m_message != null, nameof(m_message) + " != null");
        m_message.Display(message);
    }

    #region Shop UI

    private void IntShop()
    {
        m_hasShopUI = m_ShopUI != null;
        if (!m_hasShopUI)
        {
            Debug.LogWarning("Shops requires a UI to use. Please assign one in the Inspector", gameObject);
        }

        System.Diagnostics.Debug.Assert(m_playerInShop != null, nameof(m_playerInShop) + " != null");
        m_shopActive = m_playerInShop.Value;
        CloseShopUI();
    }

    private void ShopUI()
    {
        System.Diagnostics.Debug.Assert(m_playerInShop != null, nameof(m_playerInShop) + " != null");
        if (m_shopActive == m_playerInShop.Value) return;

        if (m_shopActive)
            CloseShopUI();
        else
            OpenShopUI();
    }

    private void CloseShopUI()
    {
        if (!m_shopActive) return;

        System.Diagnostics.Debug.Assert(m_playerInShop != null, nameof(m_playerInShop) + " != null");
        m_shopActive = m_playerInShop.Value = false;

        if (!m_hasShopUI) return;
        System.Diagnostics.Debug.Assert(m_ShopUI != null, nameof(m_ShopUI) + " != null");
        m_ShopUI.SetActive(false);
    }

    private void OpenShopUI()
    {
        if (m_shopActive) return;

        System.Diagnostics.Debug.Assert(m_playerInShop != null, nameof(m_playerInShop) + " != null");
        m_shopActive = m_playerInShop.Value = true;

        if (!m_hasShopUI) return;
        System.Diagnostics.Debug.Assert(m_ShopUI != null, nameof(m_ShopUI) + " != null");
        m_ShopUI.SetActive(true);
    }

    #endregion
}
