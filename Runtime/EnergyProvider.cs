// ============================================
// Energy Provider
// ============================================
// PURPOSE: Provides energy into a Lifecycle at a constant rate.
//          Can be toggled on/off for future conditions (sun, water, soil).
// USAGE: Attach alongside Lifecycle. Set rate in Inspector or via data.
//        Enable/disable to simulate environmental conditions.
// ============================================

using UnityEngine;

namespace Ludocore
{
    public class EnergyProvider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Lifecycle lifecycle;

        [Header("Settings")]
        [SerializeField] private float energyPerSecond = 10f;

        private void Update()
        {
            ProvideEnergy();
        }

        private void ProvideEnergy()
        {
            if (!lifecycle.IsAlive) return;

            lifecycle.AddEnergy(energyPerSecond * Time.deltaTime);
        }
    }
}
