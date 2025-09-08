using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public abstract class Popup : MonoBehaviour
{
    [Header("Popup Settings")]
    [SerializeField] protected float animationDuration = 0.3f;
    [SerializeField] protected Ease ease = Ease.Linear;

    protected bool isOpen = false;
    protected Tween currentTween;

    protected CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
    }

    public virtual void Open()
    {
        if (isOpen) return;
        isOpen = true;

        gameObject.SetActive(true);

        currentTween?.Kill();
        canvasGroup.alpha = 0f;
        currentTween = canvasGroup.DOFade(1f, animationDuration).SetEase(ease);
    }

    public virtual void Close()
    {
        if (!isOpen) return;
        isOpen = false;

        currentTween?.Kill();
        currentTween = canvasGroup.DOFade(0f, animationDuration).SetEase(ease)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}
