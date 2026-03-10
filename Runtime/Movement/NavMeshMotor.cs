using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Thin wrapper around NavMeshAgent — provides clean movement verbs.</summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMotor : MonoBehaviour
    {
        // ═══════════════════════════════════════
        // STATE
        // ═══════════════════════════════════════
        private NavMeshAgent _agent;
        private bool _wasMoving;
        private bool _isMoving;

        public bool IsMoving => _isMoving;

        public bool HasArrived => _agent.isOnNavMesh
            && !_agent.pathPending
            && _agent.remainingDistance <= _agent.stoppingDistance;

        public float Speed => _agent.speed;

        // ═══════════════════════════════════════
        // OUTPUTS
        // ═══════════════════════════════════════
        public event Action OnStartedMoving;
        public event Action OnArrived;

        [Header("Events")]
        [SerializeField] private UnityEvent startedMovingEvent;
        [SerializeField] private UnityEvent arrivedEvent;

        // ═══════════════════════════════════════
        // LIFECYCLE
        // ═══════════════════════════════════════
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            bool moving = _agent.isOnNavMesh
                && !_agent.pathPending
                && _agent.remainingDistance > _agent.stoppingDistance;

            if (moving && !_wasMoving)
            {
                OnStartedMoving?.Invoke();
                startedMovingEvent?.Invoke();
            }
            else if (!moving && _wasMoving)
            {
                OnArrived?.Invoke();
                arrivedEvent?.Invoke();
            }

            _wasMoving = moving;
            _isMoving = moving;
        }

        // ═══════════════════════════════════════
        // INPUTS
        // ═══════════════════════════════════════

        /// <summary>Set a NavMesh destination.</summary>
        public void MoveTo(Vector3 position)
        {
            if (!_agent.isOnNavMesh) return;

            _agent.isStopped = false;
            _agent.SetDestination(position);
        }

        /// <summary>Stop movement and clear the current path.</summary>
        public void Stop()
        {
            if (!_agent.isOnNavMesh) return;

            _agent.isStopped = true;
            _agent.ResetPath();
        }
    }
}
