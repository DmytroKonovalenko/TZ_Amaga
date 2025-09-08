using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button settingsButton;

    [Header("Other")]
    [SerializeField] private Popup settingPopup;
    [SerializeField] private Popup setNamePopup;

    [SerializeField]private SetCharacterPopup setCharacterPopup;

    private void Awake()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitClicked);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnStartClicked()
    {
        setNamePopup.Open();
    }

    private void OnExitClicked()
    {
        Application.Quit();
    }

    private void OnSettingsClicked()
    {
        settingPopup.Open();
    }

    public void StartGame()
    {
        setCharacterPopup.SelectCurrentCharacter();
        SceneLoader.Instance.LoadScene("MainGameScene");
    }

}
