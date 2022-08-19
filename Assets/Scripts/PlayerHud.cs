using TMPro;
using Unity.Netcode;

public class PlayerHud : NetworkBehaviour
{
     private NetworkVariable<NetworkString> _playersName = new();

     private bool overlaySet;

     public override void OnNetworkSpawn()
     {
          if (IsServer)
          {
               _playersName.Value = $"Player {OwnerClientId}";
          }
     }

     public void SetOverlay()
     {
          var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
          localPlayerOverlay.text = _playersName.Value;
     }

     private void Update()
     {
          if (!overlaySet && !string.IsNullOrEmpty(_playersName.Value))
          {
               SetOverlay();
               overlaySet = true;
          }
     }
}
