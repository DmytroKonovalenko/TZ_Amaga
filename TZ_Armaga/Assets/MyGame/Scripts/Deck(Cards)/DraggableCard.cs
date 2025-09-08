using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;

public class DraggableCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Hover Settings")]
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    [SerializeField] private float dragScaleDivider = 2f;
    [SerializeField] private float hoverYOffset = 30f;
    [SerializeField] private float animDuration = 0.25f;

    private Vector3 startPos;
    private Vector3 startScale;
    private Quaternion startRotation;
    private Canvas canvas;
    private RectTransform rectTransform;

    private CardData cardData;
    private bool isAnimating;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        startScale = rectTransform.localScale;
        startRotation = rectTransform.localRotation;

        canvas = GetComponentInParent<Canvas>();
    }

    public void Initialize(CardData card) => cardData = card;

    public void SetAnimating(bool anim) => isAnimating = anim;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging || isAnimating) return;

        rectTransform.DOKill();
        rectTransform.DOLocalRotate(Vector3.zero, animDuration);
        rectTransform.DOScale(startScale * hoverScaleMultiplier, animDuration);
        rectTransform.DOAnchorPos(startPos + Vector3.up * hoverYOffset, animDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        ReturnToStart();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isAnimating) return;

        rectTransform.DOKill();
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = startScale / dragScaleDivider;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isAnimating) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isAnimating) return;
        if (cardData == null) { ReturnToStart(); return; }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;

        var manager = Object.FindFirstObjectByType<PlayCardManager>();

        if (cardData.type == CardType.Building)
        {
            if (BuildingPlacer.Instance.CanPlaceBuilding(worldPos))
            {
                if (cardData.logic != null)
                {
                    cardData.logic.Execute(cardData, worldPos);
                }
                if (manager != null)
                    manager.StartCoroutine(manager.PlayBuildingEffectAndPlace(worldPos, cardData, this));

                return;
            }
        }
        else if (cardData.type == CardType.Unit)
        {
            if (BuildingPlacer.Instance.CanPlaceUnit(worldPos))
            {
                bool success = BuildingPlacer.Instance.PlaceUnit(worldPos, cardData.unitPrefab, cardData.unitCount);
                if (success)
                {
                    if (cardData.logic != null)
                    {
                        cardData.logic.Execute(cardData, worldPos);
                    }

                    manager.DisableAllCardsTemporarily(2f);
                    gameObject.SetActive(false);
                }
                else
                {
                    ReturnToStart();
                }
                return;
            }

        }
        else if (cardData.type == CardType.Spell)
        {
            if (cardData.logic != null)
            {
                Vector3Int cellPos = BuildingPlacer.Instance.buildTilemap.WorldToCell(worldPos);

                if (cardData.logic is IUnitSpellLogic) 
                {
                    if (BuildingPlacer.Instance.TryGetUnitStack(cellPos, out var unitStack))
                    {
                        cardData.logic.Execute(cardData, worldPos);

                        if (manager != null)
                            manager.DisableAllCardsTemporarily(2f);

                        gameObject.SetActive(false);
                        return;
                    }
                    else
                    {
                        ReturnToStart(); 
                        return;
                    }
                }

                if (cardData.logic is IClearTileSpellLogic)
                {

                    TileBase tile = BuildingPlacer.Instance.buildTilemap.GetTile(cellPos);
                    if (tile == BuildingPlacer.Instance.castleTile)
                    {
                        ReturnToStart();
                        return;
                    }

                    if (BuildingPlacer.Instance.allowedAreaTilemap.HasTile(cellPos))
                    {
                        cardData.logic.Execute(cardData, worldPos);

                        if (manager != null)
                            manager.DisableAllCardsTemporarily(2f);

                        gameObject.SetActive(false);
                        return;
                    }
                    else
                    {
                        ReturnToStart();
                        return;
                    }
                }

            }
        }



        ReturnToStart();
    }


    public void ReturnToStart()
    {
        rectTransform.DOKill();
        rectTransform.DOLocalRotateQuaternion(startRotation, animDuration);
        rectTransform.DOScale(startScale, animDuration);
        rectTransform.DOAnchorPos(startPos, animDuration);
    }

    public interface IUnitSpellLogic { }
    public interface IClearTileSpellLogic { }
}
