using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject inputField;
    [SerializeField] private GameObject startHostButtonObj;
    [SerializeField] private GameObject startServerButtonObj;
    [SerializeField] private GameObject startClientButtonObj;
    private Button _startHostButton;
    private Button _startServerButton;
    private Button _startClientButton;

    private void Awake()
    {
        _startHostButton = startHostButtonObj.GetComponentInChildren<Button>();
        _startServerButton = startServerButtonObj.GetComponentInChildren<Button>();
        _startClientButton = startClientButtonObj.GetComponentInChildren<Button>();
        Cursor.visible = true;
    }

    private void Start()
    {
        _startHostButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartHost())
            {
                DisableUI();
                Debug.Log("Host started...");
            }

            else
            {
                Debug.LogError("Host could not be started...");
            }
        });        
        
        _startServerButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer())
            {
                DisableUI();
                Debug.Log("Server started...");
            }

            else
            {
                Debug.LogError("Server could not be started...");
            }
        });   
        
        _startClientButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                DisableUI();
                Debug.Log("Client started...");
            }

            else
            {
                Debug.LogError("Client could not be started...");
            }
        });
    }

    private void DisableUI()
    {
        // Makes inputField invisible instead of disable it, because PlayerHud.cs takes player names from this inputField.
        inputField.transform.SetParent(transform, worldPositionStays: false);
        startHostButtonObj.SetActive(false);
        startServerButtonObj.SetActive(false);
        startClientButtonObj.SetActive(false);
    }
}