using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GameController : MonoBehaviour
{

    #region Variables
    public static GameController gc;

    [SerializeField] private LightControl lightControl;

    [SerializeField] private GameObject introScene;
    [SerializeField] private GameObject loginScene;
    [SerializeField] private GameObject ruleScene;
    [SerializeField] private GameObject gameScene;
    [SerializeField] private GameObject countdownScene;
    [SerializeField] private GameObject rankingScene;

    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private TMP_Text hitCountText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text highScoreText;

    private int ButtonCount;
    private int Score;
    private int ScoreMultiplier;
    private int comboCount;
    private int hitCount;
    [SerializeField] private float gameDuration = 30f;
    private float timer = -1;

    #endregion





    void Awake()
    {
        if(gc == null) gc = this; 
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ResetToIntroScene();
        ButtonCount = gamePanel.transform.childCount;
    }

    void ResetToIntroScene()
    {
        introScene.SetActive(true);
        loginScene.SetActive(false);
        ruleScene.SetActive(false);
        gameScene.SetActive(false);
        countdownScene.SetActive(false);
        rankingScene.SetActive(false);
        lightControl.IntroLayout();
    }





    #region Game Manipulation
    public void StartGame()
    {   
        Score = 0;
        ScoreMultiplier = 1;
        hitCount = 0;
        timer = gameDuration;
        ResetButtons();
        HighlightRandomButton();
        UpdateUI();
    }   

    private void ResetButtons()
    {
        for(int i = 0; i < ButtonCount; i++)
        {
            gamePanel.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
    }

    private void HighlightRandomButton()
    {
        Button randomButton = gamePanel.transform.GetChild(Random.Range(0, ButtonCount)).GetComponent<Button>();
        randomButton.GetComponent<Image>().color = Color.green;
    }

    public void CheckButtonPress()
    {
        GameObject pressedButton = EventSystem.current.currentSelectedGameObject;
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
            comboCount = 0;
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
        highScoreText.text = Score.ToString();
    }

    private void AddScore()
    {
        Score += 10 * ScoreMultiplier;
    }

    public void EndGame()
    {
        rankingScene.SetActive(true);
        gameScene.SetActive(false);
    }

    void FixedUpdate()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.FloorToInt(timer).ToString() + "s";
            if(timer <= 0)
            {
                EndGame();
            }
        }
    }

    #endregion

}
