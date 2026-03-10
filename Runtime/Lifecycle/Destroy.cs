using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Destroys this GameObject or a configured target.</summary>
    public class Destroy : MonoBehaviour
    {
        // ═══════════════════════════════════════
        // CONFIG
        // ═══════════════════════════════════════
        [Header("Config")]
        [Tooltip("Optional target to destroy (if empty, DestroySelf uses this GameObject)")]
        [SerializeField] private GameObject target;

        [Tooltip("Delay in seconds before destruction (0 = instant)")]
        [SerializeField] private float delay;

        // ═══════════════════════════════════════
        // OUTPUTS
        // ═══════════════════════════════════════
        public event Action OnDestroying;

        [Header("Events")]
        [SerializeField] private UnityEvent destroyingEvent;

        // ═══════════════════════════════════════
        // INPUTS
        // ═══════════════════════════════════════

        /// <summary>Destroy the configured target.</summary>
        [ContextMenu("Destroy Target")]
        public void DestroyTarget()
        {
            if (!target) return;

            OnDestroying?.Invoke();
            destroyingEvent?.Invoke();
            Destroy(target, delay);
        }

        /// <summary>Destroy this GameObject.</summary>
        [ContextMenu("Destroy Self")]
        public void DestroySelf()
        {
            OnDestroying?.Invoke();
            destroyingEvent?.Invoke();
            Destroy(gameObject, delay);
        }
    }
}
