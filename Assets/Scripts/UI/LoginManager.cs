using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LoginManager : MonoBehaviour
{
    DataManager dataManager;
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField companyNameInput;
    [SerializeField] private Button beginButton;

    void Start()
    {
        dataManager = GameController.gc.dataManager;
        beginButton.interactable = false;
    }

    public void CheckNameInput()
    {
        beginButton.interactable = !string.IsNullOrEmpty(playerNameInput.text);
    }

    public void OnBeginButtonClicked()
    {
        string playerName = playerNameInput.text;
        string email = emailInput.text;
        string companyName = companyNameInput.text;

        dataManager.InsertPlayerData(playerName, email, companyName);
    }

}
