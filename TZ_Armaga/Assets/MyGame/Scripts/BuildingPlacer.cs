using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using TMPro;

public class BuildingPlacer : MonoBehaviour
{
    public Tilemap buildTilemap;
    [SerializeField] private TileBase defaultBuildingTile;
    public Tilemap allowedAreaTilemap;
    public TileBase castleTile;
    public static BuildingPlacer Instance;

    private Dictionary<Vector3Int, UnitStack> unitsOnTiles = new Dictionary<Vector3Int, UnitStack>();
    private Dictionary<Vector3Int, CardData> buildingDataOnTiles = new Dictionary<Vector3Int, CardData>();
    private void Awake()
    {
        Instance = this;
    }

    public bool CanPlaceBuilding(Vector3 worldPos)
    {
        Vector3Int cellPos = buildTilemap.WorldToCell(worldPos);
        if (buildTilemap.HasTile(cellPos)) return false;
        if (unitsOnTiles.ContainsKey(cellPos)) return false;
        if (allowedAreaTilemap != null && allowedAreaTilemap.HasTile(cellPos)) return true;
        return false;
    }

    public bool CanPlaceUnit(Vector3 worldPos)
    {
        Vector3Int cellPos = buildTilemap.WorldToCell(worldPos);
        if (buildTilemap.HasTile(cellPos)) return false;
        if (allowedAreaTilemap != null && allowedAreaTilemap.HasTile(cellPos)) return true;
        return false;
    }

    public void SetTile(Vector3 worldPos, TileBase tile, CardData cardData = null)
    {
        Vector3Int cellPos = buildTilemap.WorldToCell(worldPos);
        buildTilemap.SetTile(cellPos, tile);

        if (tile != null && cardData != null)
            buildingDataOnTiles[cellPos] = cardData;
        else
            buildingDataOnTiles.Remove(cellPos);
    }

    public int GetBuildingPower(Vector3Int cellPos)
    {
        if (buildingDataOnTiles.TryGetValue(cellPos, out var cardData))
            return cardData.powerAmount;

        return 0;
    }


    public bool PlaceUnit(Vector3 worldPos, GameObject unitPrefab, int count)
    {
        Vector3Int cellPos = buildTilemap.WorldToCell(worldPos);
        Vector3 cellCenter = buildTilemap.GetCellCenterWorld(cellPos);

        if (unitsOnTiles.TryGetValue(cellPos, out UnitStack stack))
        {
            if (stack.unitPrefab == unitPrefab)
            {
                stack.AddUnits(count);
                return true;
            }

            return false;
        }

        GameObject unit = Instantiate(unitPrefab, cellCenter, Quaternion.identity);
        stack = new UnitStack(unit, unitPrefab, count);
        unitsOnTiles[cellPos] = stack;
        return true;
    }


    public bool TryGetUnitStack(Vector3Int cellPos, out UnitStack stack)
    {
        return unitsOnTiles.TryGetValue(cellPos, out stack);
    }

    public void RemoveUnit(Vector3 worldPos)
    {
        Vector3Int cellPos = buildTilemap.WorldToCell(worldPos);
        if (unitsOnTiles.TryGetValue(cellPos, out UnitStack stack))
        {
            Destroy(stack.unitObject);
            unitsOnTiles.Remove(cellPos);
        }
    }
    public IEnumerable<UnitStack> GetAllUnitStacks()
    {
        return unitsOnTiles.Values;
    }

    public class UnitStack
    {
        public GameObject unitObject;
        public GameObject unitPrefab;
        private int count;
        private TextMeshProUGUI countText;

        public int Count => count; 

        public UnitStack(GameObject obj, GameObject prefab, int initialCount)
        {
            unitObject = obj;
            unitPrefab = prefab;
            count = initialCount;

            countText = unitObject.GetComponentInChildren<TextMeshProUGUI>(true);
            UpdateText();
        }

        public void AddUnits(int addCount)
        {
            count += addCount;
            UpdateText();
        }

        private void UpdateText()
        {
            if (countText != null)
                countText.text = count.ToString();
        }
    }

}
