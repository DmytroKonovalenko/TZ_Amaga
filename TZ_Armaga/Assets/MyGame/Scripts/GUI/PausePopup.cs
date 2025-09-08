using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class PausePopup : Popup
{
    [SerializeField] private Slider volumeSlider;
    private void Start()
    {
        AudioController.Instance.BindBgmSlider(volumeSlider);
    }
    public void OnCloseContinueButton()
    {
        Close();
    }
    public void BackToMenu()
    {
        SceneLoader.Instance.LoadScene("MainMenuScene");
    }

}
