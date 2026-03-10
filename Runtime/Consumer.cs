// ============================================
// Consumer
// ============================================
// PURPOSE: Eats the nearest target detected by a Sensor when close enough.
//          Gains energy, destroys the target. Fully automatic.
//          Reusable: Fauna eats Flora, Apex eats Fauna — same mechanic.
// USAGE: Attach to entity root. Assign a Sensor (the food sensor) and
//        own Lifecycle. Set consume radius and energy gain.
//        The controller moves toward food, Consumer eats it on arrival.
// ============================================

using System;
using UnityEngine;

namespace Ludocore
{
    public class Consumer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Lifecycle lifecycle;
        [SerializeField] private Sensor sensor;

        [Header("Settings")]
        [SerializeField] private float consumeRadius = 1.5f;
        [SerializeField] private float energyGain = 30f;

        public event Action<GameObject> OnConsumed;

        private void Update()
        {
            if (!lifecycle.IsAlive) return;
            if (!sensor.TryGetNearest(out var signal)) return;
            if (signal.Distance > consumeRadius) return;

            lifecycle.AddEnergy(energyGain);
            OnConsumed?.Invoke(signal.Object);
            Destroy(signal.Object);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.4f);
            Gizmos.DrawWireSphere(transform.position, consumeRadius);
        }
    }
}
