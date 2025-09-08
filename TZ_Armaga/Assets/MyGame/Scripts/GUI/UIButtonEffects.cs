using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class UIButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float clickScale = 0.9f;
    [SerializeField] private float animationDuration = 0.2f;
    [SerializeField] private Ease ease = Ease.OutBack;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private bool isHovered = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        rectTransform.DOScale(originalScale * hoverScale, animationDuration).SetEase(ease);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        rectTransform.DOScale(originalScale, animationDuration).SetEase(ease);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        rectTransform.DOScale(originalScale * clickScale, animationDuration / 2)
                     .SetEase(Ease.InBack)
                     .OnComplete(() =>
                     {
                         Vector3 targetScale = isHovered ? originalScale * hoverScale : originalScale;
                         rectTransform.DOScale(targetScale, animationDuration / 2).SetEase(Ease.OutBack);
                     });
    }

}
