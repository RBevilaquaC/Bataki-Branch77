using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour
{
    [SerializeField] private GameObject countdownScene;
    [SerializeField] private GameObject GameScene;
    [SerializeField] private GameObject countdownText;
    private float countdownTime;

    public void StartCountdown()
    {
        GameController.gc.lightControl.CountdownLayout();
        countdownTime = 3f;
        countdownText.GetComponent<TMPro.TMP_Text>().text = countdownTime.ToString();
        StartCoroutine(CountdownCoroutine());
    }

    private void StartGame()
    {
        countdownScene.SetActive(false);
        GameScene.SetActive(true);
        GameController.gc.lightControl.DefaultLayout();
        GameController.gc.StartGame();
    }

    private IEnumerator CountdownCoroutine()
    {
        float currentTime = countdownTime;

        while (currentTime > 0)
        {
            countdownText.GetComponent<TMPro.TMP_Text>().text = Mathf.Ceil(currentTime).ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        StartGame();
    }
}
