using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Counts increments and notifies when a target is reached.</summary>
    public class Counter : MonoBehaviour
    {
        // ═══════════════════════════════════════
        // CONFIG
        // ═══════════════════════════════════════
        [Header("Config")]
        [SerializeField] private int targetCount = 5;
        [SerializeField] private bool autoReset;

        // ═══════════════════════════════════════
        // STATE
        // ═══════════════════════════════════════
        private int _count;

        public int Count => _count;
        public int TargetCount => targetCount;
        public float Ratio => targetCount > 0 ? (float)_count / targetCount : 0f;
        public bool IsComplete => _count >= targetCount;

        // ═══════════════════════════════════════
        // OUTPUTS
        // ═══════════════════════════════════════
        public event Action<int> OnChanged;
        public event Action OnTargetReached;

        [Header("Events")]
        [SerializeField] private UnityEvent<int> changedEvent;
        [SerializeField] private UnityEvent targetReachedEvent;

        // ═══════════════════════════════════════
        // INPUTS
        // ═══════════════════════════════════════

        /// <summary>Add one to the count. Fires OnTargetReached when target is hit.</summary>
        [ContextMenu("Increment")]
        public void Increment()
        {
            _count++;
            OnChanged?.Invoke(_count);
            changedEvent?.Invoke(_count);

            if (_count < targetCount) return;

            OnTargetReached?.Invoke();
            targetReachedEvent?.Invoke();

            if (autoReset) _count = 0;
        }

        /// <summary>Reset the count to zero.</summary>
        [ContextMenu("Reset")]
        public void Reset()
        {
            _count = 0;
            OnChanged?.Invoke(_count);
            changedEvent?.Invoke(_count);
        }
    }
}
