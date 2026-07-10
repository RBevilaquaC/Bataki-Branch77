using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GameController : MonoBehaviour
{

    #region Variables
    public static GameController gc;

    [Header("Managers")]
    public LightControl lightControl;
    public DataManager dataManager;
    public AudioManager audioManager;
    [SerializeField] private RankingPanel rankingPanel;


    [Header("Scenes")]
    [SerializeField] private GameObject introScene;
    [SerializeField] private GameObject loginScene;
    [SerializeField] private GameObject ruleScene;
    [SerializeField] private GameObject gameScene;
    [SerializeField] private GameObject countdownScene;
    [SerializeField] private GameObject rankingScene;

    [Header("UI Elements")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private TMP_Text hitCountText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text reactionTimeAvgText;
    [SerializeField] private TMP_Text bestStreakText;

    [Header("Game Settings")]
    public string playerName;
    private int ButtonCount;
    private int score;
    private int scoreMultiplier;
    private int comboCount;
    private int hitCount;
    private int bestStreak;
    private int currentStreak;
    public float reactionTimeAvg;
    [SerializeField] private int scorePerHit = 10;
    [SerializeField] private int hitPerCombo = 5;
    [SerializeField] private float gameDuration = 30f;
    private float timer = -1;

    [SerializeField] private float maxAFKTime = 20;

    private float afkTimer;

    #endregion




    void Start()
    {
        gc = this;
        ResetToIntroScene();
        ResetAFKTimer();
        ButtonCount = gamePanel.transform.childCount;
    }

    public void ResetToIntroScene()
    {
        introScene.SetActive(true);
        loginScene.SetActive(false);
        ruleScene.SetActive(false);
        gameScene.SetActive(false);
        countdownScene.SetActive(false);
        rankingScene.SetActive(false);
        lightControl.IntroLayout();
    }

    public void ResetAFKTimer()
    {
        afkTimer = maxAFKTime;
    }




    #region Game Manipulation
    public void StartGame()
    {   
        score = 0;
        scoreMultiplier = 1;
        hitCount = 0;
        bestStreak = 0;
        currentStreak = 0;
        reactionTimeAvg = 0f;
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

            if(comboCount % hitPerCombo == 0 && comboCount > 0)
            {
                audioManager.PlayIncreaseCombo();
            } else {
                audioManager.PlayButtonClick();
            }

            comboCount++;
            currentStreak = comboCount;
            if(currentStreak > bestStreak)
            {
                bestStreak = currentStreak;
            }

            scoreMultiplier = Mathf.CeilToInt(comboCount/hitPerCombo)+1;
            hitCount++;

            AddScore();
            HighlightRandomButton();
        } else
        {
            comboCount = 0;
            currentStreak = 0;
            scoreMultiplier = 1;
        }
        ResetAFKTimer();
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString();
        multiplierText.text = "x" + (scoreMultiplier).ToString();
        hitCountText.text = hitCount.ToString();
        timerText.text = Mathf.FloorToInt(timer).ToString() + "s";
    }

    private void AddScore()
    {
        score += scorePerHit * scoreMultiplier;
    }

    public void EndGame()
    {
        AddCurrentScoreToRanking();

        reactionTimeAvg = hitCount > 0 ? (gameDuration / hitCount) : 0f;

        highScoreText.text = score .ToString();
        bestStreakText.text = "x" + bestStreak.ToString();
        reactionTimeAvgText.text = reactionTimeAvg.ToString("F2") + "s";

        rankingPanel.UpdateRankingListUI();

        rankingScene.SetActive(true);
        gameScene.SetActive(false);

        lightControl.DefaultLayout();

        ResetAFKTimer();
    }

    public void AddCurrentScoreToRanking()
    {
        dataManager.InsertScore(playerName, score);
    }

    void FixedUpdate()
    {
        if(timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            timerText.text = Mathf.FloorToInt(timer).ToString() + "s";
            if(timer <= 0)
            {
                EndGame();
            }
        }

        if(afkTimer > 0)
        {
            afkTimer -= Time.fixedDeltaTime;
            if(afkTimer <= 0)
            {
                ResetToIntroScene();
            }
        }

    }

    #endregion

}
