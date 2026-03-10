using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Fires events when objects physically collide with this object.</summary>
    [RequireComponent(typeof(Collider))]
    public class CollisionSensor : MonoBehaviour
    {
        // ═══════════════════════════════════════
        // CONFIG
        // ═══════════════════════════════════════
        [Header("Config")]
        [Tooltip("Only react to objects with this tag (empty = any)")]
        [SerializeField] private string filterTag;

        // ═══════════════════════════════════════
        // STATE
        // ═══════════════════════════════════════
        private int _collisionCount;

        public bool IsColliding => _collisionCount > 0;
        public int CollisionCount => _collisionCount;

        // ═══════════════════════════════════════
        // OUTPUTS
        // ═══════════════════════════════════════
        public event Action<GameObject> OnEntered;
        public event Action<GameObject> OnExited;

        [Header("Events")]
        [SerializeField] private UnityEvent<GameObject> enteredEvent;
        [SerializeField] private UnityEvent<GameObject> exitedEvent;

        // ═══════════════════════════════════════
        // LIFECYCLE
        // ═══════════════════════════════════════
        private void OnCollisionEnter(Collision collision)
        {
            if (!PassesFilter(collision.gameObject)) return;

            _collisionCount++;
            OnEntered?.Invoke(collision.gameObject);
            enteredEvent?.Invoke(collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!PassesFilter(collision.gameObject)) return;

            _collisionCount = Mathf.Max(0, _collisionCount - 1);
            OnExited?.Invoke(collision.gameObject);
            exitedEvent?.Invoke(collision.gameObject);
        }

        // ═══════════════════════════════════════
        // PRIVATE
        // ═══════════════════════════════════════
        private bool PassesFilter(GameObject obj)
        {
            return string.IsNullOrEmpty(filterTag) || obj.CompareTag(filterTag);
        }
    }
}
