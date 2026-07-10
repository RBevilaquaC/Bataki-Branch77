using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class RankingPanel : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> players = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> scores = new List<TMP_Text>();

    public void UpdateRankingListUI()
    {

        List<(string playerName, int score)> rankingList = GameController.gc.dataManager.GetTop4Players();
        for (int i = 0; i < players.Count; i++)
        {
            if (i < rankingList.Count)
            {
                players[i].text = rankingList[i].playerName;
                scores[i].text = rankingList[i].score.ToString();
            }
            else
            {
                players[i].text = "";
                scores[i].text = "";
            }
        }
    }
}
