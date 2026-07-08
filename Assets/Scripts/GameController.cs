using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GameController : MonoBehaviour
{

    #region Variables
    public static GameController gc;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private TMP_Text hitCountText;
    [SerializeField] private TMP_Text timerText;
    private int ButtonCount;
    private int Score;
    private int ScoreMultiplier;
    private int comboCount;
    private int hitCount;
    private float timer;

    #endregion


    void Awake()
    {
        if(gc == null) gc = this; 
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ButtonCount = gamePanel.transform.childCount;
        StartGame();
    }



    #region Game Manipulation
    public void StartGame()
    {   
        Score = 0;
        ScoreMultiplier = 1;
        hitCount = 0;
        timer = 30;
        HighlightRandomButton();
        UpdateUI();
    }   

    private void HighlightRandomButton()
    {
        Button randomButton = gamePanel.transform.GetChild(Random.Range(0, ButtonCount)).GetComponent<Button>();
        randomButton.GetComponent<Image>().color = Color.green;
    }

    public void CheckButtonPress()
    {
        GameObject pressedButton = EventSystem.current.currentSelectedGameObject;
        print(pressedButton.GetComponent<Image>().color);
        if(pressedButton.GetComponent<Image>().color == Color.green)
        {
            pressedButton.GetComponent<Image>().color = Color.white;
            comboCount++;
            ScoreMultiplier = Mathf.FloorToInt(comboCount/5) + 1;
            hitCount++;
            AddScore();
            HighlightRandomButton();
        } else
        {
            comboCount=0;
            ScoreMultiplier = 1;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = Score.ToString();
        multiplierText.text = "x" + (ScoreMultiplier-1).ToString();
        hitCountText.text = hitCount.ToString();
        timerText.text = Mathf.FloorToInt(timer).ToString() + "s";
    }

    private void AddScore()
    {
        Score += 10 * ScoreMultiplier;
        ScoreMultiplier++;
        print("Score: " + Score);
    }

    void FixedUpdate()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.FloorToInt(timer).ToString() + "s";
        }
    }


    #endregion

}
