using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerHud : NetworkBehaviour
{
     private NetworkVariable<NetworkString> _playerName = new();
     private TMP_InputField _inputField;
     private bool overlaySet;

     public override void OnNetworkSpawn()
     {
          _inputField = FindObjectOfType<TMP_InputField>();

          if (IsClient && IsOwner)
          {
               UpdateClientNicknameServerRpc(_inputField.text);
               SetOverlay(_inputField.text);
          }
     }

     private void SetOverlay(string text)
     {
          gameObject.name = text;
          var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
          localPlayerOverlay.text = text;
     }

     private void Update()
     {
          if (!overlaySet && !string.IsNullOrEmpty(_playerName.Value))
          {
               SetOverlay(_playerName.Value);
               overlaySet = true;
          }
     }

     [ServerRpc]
     private void UpdateClientNicknameServerRpc(string nickname)
     {
          _playerName.Value = nickname;
     }
}
