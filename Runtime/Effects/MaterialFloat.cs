using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Sets or animates a float property on a material.</summary>
    public class MaterialFloat : MonoBehaviour
    {
        // ═══════════════════════════════════════
        // CONFIG
        // ═══════════════════════════════════════
        [Header("Config")]
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private int materialIndex;
        [Tooltip("Shader property name (e.g. _Metallic, _Smoothness, _Cutoff)")]
        [SerializeField] private string propertyName = "_Metallic";

        // ═══════════════════════════════════════
        // STATE
        // ═══════════════════════════════════════
        private Material _material;
        private bool _initialized;
        private Tween _tween;

        public float CurrentValue => _initialized && _material.HasProperty(propertyName)
            ? _material.GetFloat(propertyName)
            : 0f;

        public bool IsAnimating => _tween is { active: true };

        // ═══════════════════════════════════════
        // OUTPUTS
        // ═══════════════════════════════════════
        public event Action OnCompleted;

        [Header("Events")]
        [SerializeField] private UnityEvent completedEvent;

        // ═══════════════════════════════════════
        // INPUTS
        // ═══════════════════════════════════════

        /// <summary>Set the property value instantly.</summary>
        public void SetValue(float value)
        {
            if (!EnsureMaterial()) return;
            if (!_material.HasProperty(propertyName)) return;

            KillTween();
            _material.SetFloat(propertyName, value);
        }

        /// <summary>Animate the property value over a duration.</summary>
        public void Animate(float value, float duration)
        {
            if (!EnsureMaterial()) return;
            if (!_material.HasProperty(propertyName)) return;

            KillTween();
            _tween = _material.DOFloat(value, propertyName, duration)
                .OnComplete(HandleCompleted);
        }

        // ═══════════════════════════════════════
        // PRIVATE
        // ═══════════════════════════════════════
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
