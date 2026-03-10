using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Ticks at a configurable interval and reports progress.</summary>
    public class Timer : MonoBehaviour
    {
        // ═══════════════════════════════════════
        // CONFIG
        // ═══════════════════════════════════════
        [Header("Config")]
        [Tooltip("Duration of one tick in seconds")]
        [Min(0.01f)]
        [SerializeField] private float duration = 1f;

        [Tooltip("Number of ticks to run (0 = infinite)")]
        [Min(0)]
        [SerializeField] private int ticks = 1;

        [SerializeField] private bool autoStart = true;

        // ═══════════════════════════════════════
        // STATE
        // ═══════════════════════════════════════
        private float _elapsed;
        private int _currentTick;
        private bool _isRunning;
        private bool _isCompleted;

        public float Progress => Mathf.Clamp01(_elapsed / duration);
        public int CurrentTick => _currentTick;
        public bool IsRunning => _isRunning;
        public bool IsCompleted => _isCompleted;

        // ═══════════════════════════════════════
        // OUTPUTS
        // ═══════════════════════════════════════
        public event Action<float> OnProgress;
        public event Action OnTick;
        public event Action OnCompleted;

        [Header("Events")]
        [SerializeField] private UnityEvent<float> progressEvent;
        [SerializeField] private UnityEvent tickEvent;
        [SerializeField] private UnityEvent completedEvent;

        // ═══════════════════════════════════════
        // LIFECYCLE
        // ═══════════════════════════════════════
        private void Start()
        {
            if (autoStart) Run();
        }

        private void Update()
        {
            if (!_isRunning || _isCompleted) return;

            _elapsed += Time.deltaTime;

            float progress = Progress;
            OnProgress?.Invoke(progress);
            progressEvent?.Invoke(progress);

            if (_elapsed < duration) return;

            _elapsed = 0f;
            _currentTick++;
            OnTick?.Invoke();
            tickEvent?.Invoke();

            if (ticks <= 0 || _currentTick < ticks) return;

            _isRunning = false;
            _isCompleted = true;
            OnCompleted?.Invoke();
            completedEvent?.Invoke();
        }

        // ═══════════════════════════════════════
        // INPUTS
        // ═══════════════════════════════════════

        /// <summary>Start or resume the timer.</summary>
        [ContextMenu("Run")]
        public void Run()
        {
            if (_isCompleted) return;
            _isRunning = true;
        }

        /// <summary>Pause the timer without resetting.</summary>
        [ContextMenu("Stop")]
        public void Stop()
        {
            _isRunning = false;
        }

        /// <summary>Reset all state to initial values.</summary>
        [ContextMenu("Clear")]
        public void Clear()
        {
            _isRunning = false;
            _isCompleted = false;
            _elapsed = 0f;
            _currentTick = 0;
        }

        /// <summary>Reset and start immediately.</summary>
        [ContextMenu("Restart")]
        public void Restart()
        {
            Clear();
            Run();
        }
    }
}
