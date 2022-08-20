using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NicknameInputField : Singleton<NicknameInputField>
{
    public event Action<string> OnNicknameAvailable = delegate {  };
    
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button continueButton;
    [SerializeField] private int minCharacters;
    [SerializeField] private int maxCharacters;
    private NetworkVariable<NetworkString> _nickname = new ();
    private string _oldNickname;

    public override void OnNetworkSpawn()
    {
        if (IsClient && IsOwner)
        {
            UpdateClientNicknameServerRpc(inputField.text);
            OnNicknameAvailable.Invoke(_nickname.Value);
        }
    }

    private void Start()
    {
        inputField.onValueChanged.AddListener((value) =>
        {
            if (value.Length > maxCharacters)
            {
                inputField.text = inputField.text.Remove(inputField.text.Length - 1);
            }
            
            if (value.Length < minCharacters)
            {
                continueButton.interactable = false;
                return;
            }
            
            continueButton.interactable = true;
        } );
    }

    [ServerRpc]
    private void UpdateClientNicknameServerRpc(string clientNickname)
    { 
        _nickname.Value = clientNickname;
    }
}
