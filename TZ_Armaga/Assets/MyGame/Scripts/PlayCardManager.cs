using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class PlayCardManager : MonoBehaviour
{
    [Header("Card Slots")]
    [SerializeField] private GameObject[] cardSlots;

    [Header("Animation Settings")]
    [SerializeField] private float flipDuration = 0.5f;
    [SerializeField] private float delayBeforeStart = 0.7f;
    [SerializeField] private float delayBetweenCards = 0.2f;
    [SerializeField] private Ease flipEaseIn = Ease.InBack;
    [SerializeField] private Ease flipEaseOut = Ease.OutBack;

    private CharacterData character;
    private DeckData deck;
    private List<CardData> remainingCards;

    private bool firstCardShown;

    private void Start()
    {
        character = GameManager.Instance.SelectedCharacter;
        if (character == null || character.startingDeck == null) return;

        deck = character.startingDeck;
        remainingCards = new List<CardData>(deck.cards);

        InitializeCards();
        ShowStartingCard();
    }

    private void InitializeCards()
    {
        foreach (var slot in cardSlots)
        {
            slot.SetActive(false);
            var img = slot.GetComponentInChildren<Image>();
            if (img != null) img.sprite = deck.bgCard;

            var texts = slot.GetComponentsInChildren<TMP_Text>(true);
            foreach (var t in texts) t.gameObject.SetActive(false);
        }
    }

    private void ShowStartingCard()
    {
        if (remainingCards.Count == 0) return;

        var firstCard = remainingCards[0];
        var slot = cardSlots[1];
        slot.SetActive(true);

        var draggable = slot.GetComponent<DraggableCard>();
        if (draggable != null) draggable.Initialize(firstCard);

        AnimateCardFlip(slot, firstCard, 0);

        firstCardShown = true;
        remainingCards.RemoveAt(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShowRandomThreeCards();
    }

    private void ShowRandomThreeCards()
    {
        foreach (var slot in cardSlots)
        {
            slot.SetActive(false);
            var texts = slot.GetComponentsInChildren<TMP_Text>(true);
            foreach (var t in texts) t.gameObject.SetActive(false);
        }

        if (remainingCards.Count == 0) return;

        var shuffled = new List<CardData>(remainingCards);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int rnd = Random.Range(i, shuffled.Count);
            var temp = shuffled[i];
            shuffled[i] = shuffled[rnd];
            shuffled[rnd] = temp;
        }

        int cardsToShow = Mathf.Min(3, shuffled.Count);
        for (int i = 0; i < cardsToShow && i < cardSlots.Length; i++)
        {
            var slot = cardSlots[i];
            slot.SetActive(true);

            var img = slot.GetComponentInChildren<Image>();
            if (img != null) img.sprite = deck.bgCard;

            var draggable = slot.GetComponent<DraggableCard>();
            if (draggable != null) draggable.Initialize(shuffled[i]);

            AnimateCardFlip(slot, shuffled[i], i);
        }

        firstCardShown = false;
    }

    private void AnimateCardFlip(GameObject cardSlot, CardData card, int index)
    {
        var img = cardSlot.GetComponentInChildren<Image>();
        var texts = cardSlot.GetComponentsInChildren<TMP_Text>(true);
        var draggable = cardSlot.GetComponent<DraggableCard>();

        if (draggable != null) draggable.SetAnimating(true);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delayBeforeStart + index * delayBetweenCards);
        seq.Append(cardSlot.transform.DOScaleX(0f, flipDuration / 2).SetEase(flipEaseIn));
        seq.AppendCallback(() =>
        {
            if (img != null) img.sprite = card.artwork;
            if (texts.Length > 0) texts[0].text = card.cardName;
            if (texts.Length > 1) texts[1].text = card.description;
            foreach (var t in texts) t.gameObject.SetActive(true);
        });
        seq.Append(cardSlot.transform.DOScaleX(1f, flipDuration / 2).SetEase(flipEaseOut)
            .OnComplete(() => { if (draggable != null) draggable.SetAnimating(false); }));
    }

    public void RefreshCardsWithDelay(float delay = 2f) => StartCoroutine(RefreshCardsCoroutine(delay));

    private IEnumerator RefreshCardsCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowRandomThreeCards();
    }

    public IEnumerator RefreshUsedCard(DraggableCard card, float delay)
    {
        yield return new WaitForSeconds(delay);
        card.gameObject.SetActive(true);
        card.ReturnToStart();
        ShowRandomThreeCards();
    }

    public void DisableAllCardsTemporarily(float delay)
    {
        foreach (var slot in cardSlots)
        {
            var draggable = slot.GetComponent<DraggableCard>();
            if (draggable != null)
            {
                slot.SetActive(false);
                draggable.ReturnToStart();
            }
        }
        StartCoroutine(EnableAllCardsAfterDelay(delay));
    }

    private IEnumerator EnableAllCardsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var slot in cardSlots) slot.SetActive(true);
        ShowRandomThreeCards();
    }
    public IEnumerator PlayBuildingEffectAndPlace(Vector3 worldPos, CardData cardData, DraggableCard card)
    {
        card.gameObject.SetActive(false);

        GameObject effect = Instantiate(cardData.buildingEffectPrefab, worldPos, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(effect);

        BuildingPlacer.Instance.SetTile(worldPos, cardData.buildingTile);

        DisableAllCardsTemporarily(2f);
    }

}
