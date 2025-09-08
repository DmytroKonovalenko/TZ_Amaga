using UnityEngine;

public abstract class CardLogic : ScriptableObject
{
    public abstract void Execute(CardData cardData, Vector3 worldPos);
}
