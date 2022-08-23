using TMPro;
using Unity.Netcode;
using UnityEngine;

public sealed class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject leaderboard;
    [SerializeField] private TMP_Text[] winnersText;

    public void ShowLeaderboard(PlayerScore[] winners)
    {
        leaderboard.SetActive(true);

        if (winners.Length > 0)
        {
            for (var i = 0; i < winners.Length; i++)
            {
                if (winnersText.Length >= i + 1)
                {
                    winnersText[i].text = $"{i + 1}. {winners[i].gameObject.name}            {winners[i].Score.Value}";
                }
            }
        }
    }
}
