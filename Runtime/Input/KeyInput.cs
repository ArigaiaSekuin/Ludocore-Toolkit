using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Fires events when a keyboard or mouse key is pressed or released.</summary>
    public class KeyInput : MonoBehaviour
    {
        // ═══════════════════════════════════════
        // CONFIG
        // ═══════════════════════════════════════
        [Header("Config")]
        [Tooltip("The key to listen for (includes Mouse0-6)")]
        [SerializeField] private KeyCode key = KeyCode.Space;

        // ═══════════════════════════════════════
        // STATE
        // ═══════════════════════════════════════
        private bool _isHeld;

        public bool IsHeld => _isHeld;

        // ═══════════════════════════════════════
        // OUTPUTS
        // ═══════════════════════════════════════
        public event Action OnPressed;
        public event Action OnReleased;

        [Header("Events")]
        [SerializeField] private UnityEvent pressedEvent;
        [SerializeField] private UnityEvent releasedEvent;

        // ═══════════════════════════════════════
        // LIFECYCLE
        // ═══════════════════════════════════════
        private void Update()
        {
            if (Input.GetKeyDown(key))
            {
                _isHeld = true;
                OnPressed?.Invoke();
                pressedEvent?.Invoke();
            }

            if (Input.GetKeyUp(key))
            {
                _isHeld = false;
                OnReleased?.Invoke();
                releasedEvent?.Invoke();
            }
        }
    }
}
