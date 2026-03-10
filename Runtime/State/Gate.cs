using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Holds a boolean state and notifies when it changes.</summary>
    public class Gate : MonoBehaviour
    {
        // ═══════════════════════════════════════
        // CONFIG
        // ═══════════════════════════════════════
        [Header("Config")]
        [SerializeField] private bool initialValue;

        // ═══════════════════════════════════════
        // STATE
        // ═══════════════════════════════════════
        private bool _value;

        public bool Value => _value;

        // ═══════════════════════════════════════
        // OUTPUTS
        // ═══════════════════════════════════════
        public event Action<bool> OnChanged;

        [Header("Events")]
        [SerializeField] private UnityEvent<bool> changedEvent;

        // ═══════════════════════════════════════
        // LIFECYCLE
        // ═══════════════════════════════════════
        private void Awake()
        {
            _value = initialValue;
        }

        // ═══════════════════════════════════════
        // INPUTS
        // ═══════════════════════════════════════

        /// <summary>Set the gate to true. Fires OnChanged if value actually changes.</summary>
        [ContextMenu("Set True")]
        public void SetTrue() => Apply(true);

        /// <summary>Set the gate to false. Fires OnChanged if value actually changes.</summary>
        [ContextMenu("Set False")]
        public void SetFalse() => Apply(false);

        /// <summary>Flip the gate value.</summary>
        [ContextMenu("Toggle")]
        public void Toggle() => Apply(!_value);

        // ═══════════════════════════════════════
        // PRIVATE
        // ═══════════════════════════════════════
        private void Apply(bool newValue)
        {
            if (_value == newValue) return;

            _value = newValue;
            OnChanged?.Invoke(_value);
            changedEvent?.Invoke(_value);
        }
    }
}
