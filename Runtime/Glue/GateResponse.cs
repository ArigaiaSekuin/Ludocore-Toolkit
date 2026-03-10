using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Splits a Gate's boolean change into separate true/false events.</summary>
    public class GateResponse : MonoBehaviour
    {
        [Header("Source")]
        [SerializeField] private Gate gate;

        [Header("Events")]
        [SerializeField] private UnityEvent onTrue;
        [SerializeField] private UnityEvent onFalse;

        private void OnEnable()
        {
            if (gate) gate.OnChanged += HandleChanged;
        }

        private void OnDisable()
        {
            if (gate) gate.OnChanged -= HandleChanged;
        }

        private void HandleChanged(bool value)
        {
            if (value)
                onTrue?.Invoke();
            else
                onFalse?.Invoke();
        }
    }
}
