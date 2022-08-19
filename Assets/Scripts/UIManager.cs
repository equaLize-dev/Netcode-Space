using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
                startHostButtonObj.SetActive(false);
                startServerButtonObj.SetActive(false);
                startClientButtonObj.SetActive(false);
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
                startHostButtonObj.SetActive(false);
                startServerButtonObj.SetActive(false);
                startClientButtonObj.SetActive(false);
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
                startHostButtonObj.SetActive(false);
                startServerButtonObj.SetActive(false);
                startClientButtonObj.SetActive(false);
                Debug.Log("Client started...");
            }

            else
            {
                Debug.LogError("Client could not be started...");
            }
        });
    }
}
