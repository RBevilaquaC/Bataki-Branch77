using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleMiniGame : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text titleText;

    private string originalText;
    private int currentIndex = -1;

    private void Start()
    {
        titleText = GetComponent<TMP_Text>();
        originalText = titleText.text;
        SelectRandomCharacter();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int clickedIndex = TMP_TextUtilities.FindIntersectingCharacter(
            titleText,
            eventData.position,
            eventData.pressEventCamera,
            true);

        if (clickedIndex == -1)
            return;

        if (clickedIndex == currentIndex)
        {
            SelectRandomCharacter();
        }
    }

    private void SelectRandomCharacter()
    {
        currentIndex = Random.Range(0, originalText.Length);

        UpdateColors();
    }

    private void UpdateColors()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        for (int i = 0; i < originalText.Length; i++)
        {
            if (i == currentIndex)
                builder.Append($"<color=green>{originalText[i]}</color>");
            else
                builder.Append($"<color=white>{originalText[i]}</color>");
        }

        titleText.text = builder.ToString();
    }
}