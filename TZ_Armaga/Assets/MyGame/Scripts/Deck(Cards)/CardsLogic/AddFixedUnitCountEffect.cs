using UnityEngine;
using static DraggableCard;

[CreateAssetMenu(menuName = "Cards/Effects/AddFixedUnitCountEffect")]
public class AddFixedUnitCountEffect : CardLogic, IUnitSpellLogic
{
    [SerializeField] private int addCount = 1; 

    public override void Execute(CardData cardData, Vector3 worldPos)
    {

        Vector3Int cellPos = BuildingPlacer.Instance.buildTilemap.WorldToCell(worldPos);
        if (BuildingPlacer.Instance.TryGetUnitStack(cellPos, out var stack))
        {
            stack.AddUnits(addCount);
            GameManager.Instance.AddPower(addCount);

        }
    }
}
