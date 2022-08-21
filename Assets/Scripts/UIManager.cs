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
    [SerializeField] private TextMeshProUGUI playersInGameText;
    private Button startHostButton;
    private Button startServerButton;
    private Button startClientButton;

    private void Awake()
    {
        startHostButton = startHostButtonObj.GetComponentInChildren<Button>();
        startServerButton = startServerButtonObj.GetComponentInChildren<Button>();
        startClientButton = startClientButtonObj.GetComponentInChildren<Button>();
        Cursor.visible = true;
    }

    private void Update()
    {
        playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
    }

    private void Start()
    {
        startHostButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartHost())
            {
                DisableUI();
                playersInGameText.gameObject.SetActive(true);
                Debug.Log("Host started...");
            }

            else
            {
                Debug.LogError("Host could not be started...");
            }
        });        
        
        startServerButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer())
            {
                DisableUI();
                playersInGameText.gameObject.SetActive(true);
                Debug.Log("Server started...");
            }

            else
            {
                Debug.LogError("Server could not be started...");
            }
        });   
        
        startClientButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                DisableUI();
                playersInGameText.gameObject.SetActive(true);
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