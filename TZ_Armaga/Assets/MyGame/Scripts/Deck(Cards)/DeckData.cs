using UnityEngine;

[CreateAssetMenu(fileName = "NewDeck", menuName = "Cards/Deck")]
public class DeckData : ScriptableObject
{
    [Header("Deck Info")]
    public string deckName;
    public CardData[] cards = new CardData[9];
    public Sprite bgCard;
}
