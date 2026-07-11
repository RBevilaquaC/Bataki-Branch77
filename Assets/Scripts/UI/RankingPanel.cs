using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Collections.Generic;

public class RankingPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text congratulationsText;
    public string playerName;
    private int maxPlayersToShow;
    [SerializeField] private GameObject Content;
    [SerializeField] private GameObject RankLabelPrefab;
    private List<TMP_Text> players;
    private List<TMP_Text> scores;
    [SerializeField] private float spacing = 10f;

    void CreateRankingLabels()
    {
        players = new List<TMP_Text>();
        scores = new List<TMP_Text>();

        RectTransform contentRect = Content.GetComponent<RectTransform>();
        RectTransform prefabRect = RankLabelPrefab.GetComponent<RectTransform>();

        float aspectRatio = prefabRect.rect.height / prefabRect.rect.width;

        float itemWidth = contentRect.rect.width;

        float itemHeight = itemWidth * aspectRatio *.5f;

        float contentHeight = itemHeight * maxPlayersToShow + spacing * (maxPlayersToShow - 1);

        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);


        for (int i = 0; i < maxPlayersToShow; i++)
        {
            GameObject label = Instantiate(RankLabelPrefab, Content.transform);
            label.name = $"RankLabel_{i + 1}";

            RectTransform rect = label.GetComponent<RectTransform>();

            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(0.5f, 1);

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemWidth);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemHeight);

            rect.anchoredPosition = new Vector2(
                0,
                -(itemHeight + spacing) * i
            );


            TMP_Text rankText = label.transform.GetChild(0).GetComponent<TMP_Text>();
            rankText.text = (i + 1).ToString();

            players.Add(label.transform.GetChild(1).GetComponent<TMP_Text>());
            scores.Add(label.transform.GetChild(2).GetComponent<TMP_Text>());
        }
    }



    public void UpdateRankingListUI()
    {  
        maxPlayersToShow = GameController.gc.rankingListSize;

        CreateRankingLabels();

        List<(string playerName, int score)> rankingList = GameController.gc.dataManager.GetTopPlayers();

        for (int i = 0; i < players.Count; i++)
        {
            if (i < rankingList.Count)
            {
                string firstName = rankingList[i].playerName
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];

                players[i].text = firstName;
                scores[i].text = rankingList[i].score.ToString();
            }
            else
            {
                players[i].text = "";
                scores[i].text = "";
            }
        }
    }

    public void UpdateCongratulationsText()
    {
        string firstName = playerName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
        congratulationsText.text = $"Mandou bem, {firstName}!";
    }
}
