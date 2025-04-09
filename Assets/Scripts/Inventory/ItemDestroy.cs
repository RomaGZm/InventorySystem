using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IS.Inventory
{
    public class ItemDestroy : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Transform targetTransform;
        private Tween scaleTween;
        private Vector3 originalScale;

        void Awake()
        {
            originalScale = targetTransform.localScale;

        }
        /// <summary>
        /// Start Fade animation
        /// </summary>
        public void StartFade()
        {
            if (scaleTween != null && scaleTween.IsActive() && scaleTween.IsPlaying()) return;

            // Scale 
            scaleTween = targetTransform.DOScale(originalScale * 1.5f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        /// <summary>
        /// Stop Fade animation
        /// </summary>
        public void StopFade()
        {

            if (scaleTween != null && scaleTween.IsActive())
            {
                scaleTween.Kill();
                targetTransform.localScale = originalScale;
            }
        }
    }
}

