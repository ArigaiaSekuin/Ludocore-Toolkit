using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Sets or animates a color property on a material.</summary>
    public class MaterialColor : MonoBehaviour
    {
        //==================== CONFIG =====================
        [Header("Config")]
        [Tooltip("Renderer whose material to modify")]
        [SerializeField] private Renderer targetRenderer;
        [Tooltip("Index of the material on the renderer")]
        [Min(0)]
        [SerializeField] private int materialIndex;
        [Tooltip("Shader property name (e.g. _BaseColor, _EmissionColor)")]
        [SerializeField] private string propertyName = "_BaseColor";

        //==================== STATE =====================
        private Material _material;
        private bool _initialized;
        private Tween _tween;

        public Color CurrentColor => _initialized && _material.HasProperty(propertyName)
            ? _material.GetColor(propertyName)
            : Color.white;

        public bool IsAnimating => _tween is { active: true };

        //==================== OUTPUTS =====================
        public event Action OnCompleted;

        [Header("Events")]
        [Tooltip("Fired when the color animation completes")]
        [SerializeField] private UnityEvent completedEvent;

        //==================== INPUTS =====================
        /// <summary>Set the property color instantly.</summary>
        public void SetColor(Color color)
        {
            if (!EnsureMaterial()) return;
            if (!_material.HasProperty(propertyName)) return;

            KillTween();
            _material.SetColor(propertyName, color);
        }

        /// <summary>Animate the property color over a duration.</summary>
        public void Animate(Color color, float duration)
        {
            if (!EnsureMaterial()) return;
            if (!_material.HasProperty(propertyName)) return;

            KillTween();
            _tween = _material.DOColor(color, propertyName, duration)
                .OnComplete(HandleCompleted);
        }

        //==================== PRIVATE =====================
        private bool EnsureMaterial()
        {
            if (_initialized) return _material;

            if (!targetRenderer) return false;

            _material = targetRenderer.materials[materialIndex];
            _initialized = true;
            return _material;
        }

        private void KillTween()
        {
            if (_tween is { active: true }) _tween.Kill();
        }

        private void HandleCompleted()
        {
            OnCompleted?.Invoke();
            completedEvent?.Invoke();
        }

        private void OnDestroy()
        {
            KillTween();
            if (_initialized && _material && Application.isPlaying)
                Destroy(_material);
        }
    }
}
