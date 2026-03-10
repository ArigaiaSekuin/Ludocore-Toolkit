using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Rolls against a probability and reports success or failure.</summary>
    public class Chance : MonoBehaviour
    {
        // ═══════════════════════════════════════
        // CONFIG
        // ═══════════════════════════════════════
        [Header("Config")]
        [Tooltip("Probability of success (0 = never, 1 = always)")]
        [Range(0f, 1f)]
        [SerializeField] private float probability = 0.5f;

        // ═══════════════════════════════════════
        // STATE
        // ═══════════════════════════════════════
        private bool _lastResult;

        public bool LastResult => _lastResult;
        public float Probability => probability;

        // ═══════════════════════════════════════
        // OUTPUTS
        // ═══════════════════════════════════════
        public event Action OnSuccess;
        public event Action OnFail;

        [Header("Events")]
        [SerializeField] private UnityEvent successEvent;
        [SerializeField] private UnityEvent failEvent;

        // ═══════════════════════════════════════
        // INPUTS
        // ═══════════════════════════════════════

        /// <summary>Roll against probability. Fires OnSuccess or OnFail.</summary>
        [ContextMenu("Roll")]
        public void Roll()
        {
            _lastResult = UnityEngine.Random.value <= probability;

            if (_lastResult)
            {
                OnSuccess?.Invoke();
                successEvent?.Invoke();
            }
            else
            {
                OnFail?.Invoke();
                failEvent?.Invoke();
            }
        }

        /// <summary>Set the probability at runtime (0–1).</summary>
        public void SetProbability(float value)
        {
            probability = Mathf.Clamp01(value);
        }
    }
}
