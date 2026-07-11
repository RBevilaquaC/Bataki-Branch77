using UnityEngine;

public class PortraitAnchor : MonoBehaviour
{
    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        AdjustPortraitArea();
    }

    void AdjustPortraitArea()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float portraitAspect = 9f / 16f;

        if (screenAspect > portraitAspect)
        {
            float width = portraitAspect / screenAspect;

            rect.anchorMin = new Vector2((1 - width) / 2, 0);
            rect.anchorMax = new Vector2((1 + width) / 2, 1);
        }
        else
        {
            float height = screenAspect / portraitAspect;

            rect.anchorMin = new Vector2(0, (1 - height) / 2);
            rect.anchorMax = new Vector2(1, (1 + height) / 2);
        }

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}