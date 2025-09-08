using UnityEngine;
using static DraggableCard;

[CreateAssetMenu(menuName = "Cards/Effects/DoubleUnitCountOnTileEffect")]
public class DoubleUnitCountOnTileEffect : CardLogic, IUnitSpellLogic
{
    public override void Execute(CardData cardData, Vector3 worldPos)
    {
        if (cardData.type != CardType.Spell) return;

        Vector3Int cellPos = BuildingPlacer.Instance.buildTilemap.WorldToCell(worldPos);

        if (BuildingPlacer.Instance.TryGetUnitStack(cellPos, out var stack))
        {
            int originalCount = stack.Count;
            stack.AddUnits(originalCount);
            GameManager.Instance.AddPower(originalCount);
        }
    }
}
