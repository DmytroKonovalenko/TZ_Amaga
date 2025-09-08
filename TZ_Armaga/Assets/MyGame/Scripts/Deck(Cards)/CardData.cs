using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    [Header("General Info")]
    public string cardName;
    [TextArea] public string description;
    public Sprite artwork;
    public CardType type;

    [Header("Logic")]
    public CardLogic logic;   
    public int powerAmount;   

    [Header("Building Settings")]
    public TileBase buildingTile;
    public GameObject buildingEffectPrefab;

    [Header("Unit Settings")]
    public GameObject unitPrefab;
    public int unitCount = 5;
}
