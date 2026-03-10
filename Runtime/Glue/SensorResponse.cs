using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Bridges sensor detection state to UnityEvents each frame.</summary>
    public class SensorResponse : MonoBehaviour
    {
        [Header("Source")]
        [SerializeField] private Sensor sensor;

        [Header("Events")]
        [Tooltip("Fires every frame while a signal is detected. Passes nearest position.")]
        [SerializeField] private UnityEvent<Vector3> whileDetected;

        [Tooltip("Fires once when the first signal is detected.")]
        [SerializeField] private UnityEvent onFirstDetected;

        [Tooltip("Fires once when the last signal is lost.")]
        [SerializeField] private UnityEvent onAllLost;

        private bool _hadSignal;

        private void Update()
        {
            if (!sensor) return;

            bool hasSignal = sensor.TryGetNearest(out var nearest);

            if (hasSignal)
            {
                if (!_hadSignal) onFirstDetected?.Invoke();
                whileDetected?.Invoke(nearest.Object.transform.position);
            }
            else if (_hadSignal)
            {
                onAllLost?.Invoke();
            }

            _hadSignal = hasSignal;
        }
    }
}
