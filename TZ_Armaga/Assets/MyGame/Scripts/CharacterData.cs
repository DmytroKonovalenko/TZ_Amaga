using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Game/Character")]
public class CharacterData : ScriptableObject
{
    [Header("General Info")]
    public string characterName;
    [TextArea] public string description;
    public Sprite portrait;
    public bool isUnlocked = false;

    [Header("Deck")]
    public DeckData startingDeck; 


}
