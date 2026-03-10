using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Fires events when a float value crosses a threshold.</summary>
    public class ThresholdResponse : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private float threshold = 0.5f;

        [Tooltip("Fire when value goes above threshold (true) or below it (false).")]
        [SerializeField] private bool fireAbove = true;

        [Header("Events")]
        [SerializeField] private UnityEvent onCrossed;
        [SerializeField] private UnityEvent onRecovered;

        private bool _isCrossed;
        private bool _initialized;

        /// <summary>Feed a value from any module. Wire via UnityEvent or call from code.</summary>
        public void SetValue(float value)
        {
            bool crossed = fireAbove ? value >= threshold : value <= threshold;

            if (!_initialized)
            {
                _isCrossed = crossed;
                _initialized = true;
                return;
            }

            if (crossed == _isCrossed) return;

            _isCrossed = crossed;

            if (_isCrossed)
                onCrossed?.Invoke();
            else
                onRecovered?.Invoke();
        }
    }
}
