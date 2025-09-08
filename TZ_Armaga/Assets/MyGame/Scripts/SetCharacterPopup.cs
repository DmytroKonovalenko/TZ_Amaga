using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SetCharacterPopup : Popup
{
    [Header("UI References")]
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private GameObject btn_start;

    [SerializeField] private CanvasGroup contentGroup;

    [Header("Deck UI")]
    [SerializeField] private Image[] deckSlots = new Image[9];

    [Header("Characters")]
    [SerializeField] private CharacterData[] characters;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private Ease fadeEase = Ease.OutCubic;

    private int currentIndex = 0;
    private bool isAnimating = false;

    protected override void Awake()
    {
        base.Awake();

        if (characters == null || characters.Length == 0)
        {
            return;
        }

        currentIndex = 0;
        ForceUpdateUI();
    }

    private void ForceUpdateUI()
    {
        if (characters == null || characters.Length == 0) return;

        CharacterData character = characters[currentIndex];

        portraitImage.sprite = character.portrait;
        nameText.text = character.characterName;
        descriptionText.text = character.description;

        if (lockedOverlay != null)
            lockedOverlay.SetActive(!character.isUnlocked);
        if (btn_start != null)
            btn_start.SetActive(character.isUnlocked);

        UpdateDeckUI(character);
    }

    private void UpdateDeckUI(CharacterData character)
    {
        if (deckSlots.Length != 9 || character.startingDeck == null) return;

        for (int i = 0; i < 9; i++)
        {
            Image slot = deckSlots[i];

            CardData card = character.startingDeck.cards.Length > i ? character.startingDeck.cards[i] : null;

            if (!character.isUnlocked)
            {
                slot.sprite = character.startingDeck.bgCard;

                foreach (Transform child in slot.transform)
                    child.gameObject.SetActive(false);
            }
            else
            {
                slot.sprite = card != null ? card.artwork : character.startingDeck.bgCard;

                foreach (Transform child in slot.transform)
                {
                    child.gameObject.SetActive(true);

                    TextMeshProUGUI tmp = child.GetComponent<TextMeshProUGUI>();
                    if (tmp != null)
                    {
                        if (child.name.ToLower().Contains("name"))
                            tmp.text = card != null ? card.cardName : "";
                        else if (child.name.ToLower().Contains("description"))
                            tmp.text = card != null ? card.description : "";
                    }
                }
            }
        }
    }

    private void AnimateTransition(int newIndex)
    {
        if (isAnimating) return;
        isAnimating = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(contentGroup.DOFade(0f, fadeDuration).SetEase(fadeEase));
        seq.AppendCallback(() =>
        {
            currentIndex = newIndex;
            ForceUpdateUI();
        });
        seq.Append(contentGroup.DOFade(1f, fadeDuration).SetEase(fadeEase));

        seq.OnComplete(() => isAnimating = false);
    }

    public void NextCharacter()
    {
        if (characters == null || characters.Length == 0) return;

        int newIndex = (currentIndex + 1) % characters.Length;
        AnimateTransition(newIndex);
    }

    public void PreviousCharacter()
    {
        if (characters == null || characters.Length == 0) return;

        int newIndex = (currentIndex - 1 + characters.Length) % characters.Length;
        AnimateTransition(newIndex);
    }

    public void SelectCurrentCharacter()
    {
        if (characters == null || characters.Length == 0) return;

        CharacterData selected = characters[currentIndex];
        if (!selected.isUnlocked)
        {
            return;
        }

        GameManager.Instance.SetSelectedCharacter(selected);
    }
}
