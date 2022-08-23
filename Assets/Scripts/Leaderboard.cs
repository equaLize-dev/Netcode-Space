using System.Linq;
using TMPro;
using UnityEngine;

public class Leaderboard : Singleton<Leaderboard>
{
    [SerializeField] private TMP_Text[] winnersText;

    public void ShowLeaderboard()
    {
        PlayerCollider[] winners = FindObjectsOfType<PlayerCollider>().OrderByDescending(player => player.Score.Value).ToArray();
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
