using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/IncreaseUnitsByRatio")]
public class IncreaseUnitsByRatio : CardLogic
{
    [SerializeField] private int ratio = 5;   

    public override void Execute(CardData cardData, Vector3 worldPos)
    {
        int totalAdded = 0;

        foreach (var stack in BuildingPlacer.Instance.GetAllUnitStacks())
        {
            int extra = stack.Count / ratio; 
            if (extra > 0)
            {
                stack.AddUnits(extra);
                totalAdded += extra;
            }
        }

        if (totalAdded > 0)
        {
            GameManager.Instance.AddPower(totalAdded);
        }
    }
}
