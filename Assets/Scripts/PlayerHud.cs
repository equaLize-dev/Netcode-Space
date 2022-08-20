using TMPro;
using Unity.Netcode;

public class PlayerHud : NetworkBehaviour
{
     private NetworkVariable<NetworkString> _playerName = new();
     private bool overlaySet;

     public override void OnNetworkSpawn()
     {
          if (IsServer)
          {
               NicknameInputField.Instance.OnNicknameAvailable += delegate (string nickname)
               {
                    _playerName.Value = nickname;
               };
          }
     }

     public void SetOverlay()
     {
          var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
          localPlayerOverlay.text = _playerName.Value;
     }

     private void Update()
     {
          if (!overlaySet && !string.IsNullOrEmpty(_playerName.Value))
          {
               SetOverlay();
               overlaySet = true;
          }
     }
}
