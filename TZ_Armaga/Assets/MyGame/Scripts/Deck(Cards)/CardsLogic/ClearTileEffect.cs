using UnityEngine;
using UnityEngine.Tilemaps;
using static DraggableCard;

[CreateAssetMenu(menuName = "Cards/Effects/ClearTileEffect")]
public class ClearTileEffect : CardLogic, IClearTileSpellLogic
{
    public override void Execute(CardData cardData, Vector3 worldPos)
    {
        if (BuildingPlacer.Instance == null) return;

        Vector3Int cellPos = BuildingPlacer.Instance.buildTilemap.WorldToCell(worldPos);

        if (BuildingPlacer.Instance.TryGetUnitStack(cellPos, out var unitStack))
        {
            int unitCount = unitStack.Count;                 
            GameManager.Instance.AddPower(-unitCount);      
            BuildingPlacer.Instance.RemoveUnit(worldPos);    
        }

        TileBase tile = BuildingPlacer.Instance.buildTilemap.GetTile(cellPos);
        if (tile != null && tile != BuildingPlacer.Instance.castleTile)
        {

            int buildingPower = BuildingPlacer.Instance.GetBuildingPower(cellPos);
            GameManager.Instance.AddPower(-buildingPower);


            BuildingPlacer.Instance.SetTile(worldPos, null); 
        }
    }
}
