// ============================================
// Plant Grow
// ============================================
// PURPOSE: Animates a plant from nothing to full size and intensity.
//          Grows scale from zero to target and emission from zero to max.
// USAGE: Attach to plant prefab. Set target scale, emission color,
//        max intensity, and grow duration. Plays automatically on enable.
// ============================================

using DG.Tweening;
using UnityEngine;

namespace Ludocore
{
    public class PlantGrow : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Renderer targetRenderer;

        [Header("Scale")]
        [SerializeField] private float targetScale = 1f;
        [SerializeField] private Ease scaleEase = Ease.OutBack;

        [Header("Emission")]
        [SerializeField, ColorUsage(false, true)] private Color emissionColor = Color.white;
        [SerializeField] private float maxIntensity = 2f;
        [SerializeField] private Ease emissionEase = Ease.OutQuad;

        [Header("Timing")]
        [SerializeField] private float duration = 1f;

        private Material _material;

        private void Awake()
        {
            _material = targetRenderer.material;
        }

        private void OnEnable()
        {
            Grow();
        }

        private void Grow()
        {
            transform.localScale = Vector3.zero;
            _material.SetColor("_EmissionColor", Color.black);

            transform.DOScale(Vector3.one * targetScale, duration).SetEase(scaleEase);

            _material.DOColor(emissionColor * maxIntensity, "_EmissionColor", duration).SetEase(emissionEase);
        }

        private void OnDestroy()
        {
            transform.DOKill();
            if (_material)
            {
                _material.DOKill();
                Destroy(_material);
            }
        }
    }
}
