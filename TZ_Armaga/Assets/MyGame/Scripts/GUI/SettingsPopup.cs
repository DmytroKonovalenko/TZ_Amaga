using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : Popup
{
    [SerializeField]private Slider volumeSlider;    
    private void Start()
    {
        AudioController.Instance.BindBgmSlider(volumeSlider);
    }
    public void OnCloseButton()
    {
        Close();
    }
}
