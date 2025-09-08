using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/AddPowerEffect")]
public class AddPowerEffect : CardLogic
{
    public override void Execute(CardData cardData, Vector3 worldPos)
    {
        GameManager.Instance.AddPower(cardData.powerAmount);
    }
}
