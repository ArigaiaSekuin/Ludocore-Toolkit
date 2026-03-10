// ============================================
// Lifecycle Data
// ============================================
// PURPOSE: Defines energy and decay parameters for any living entity.
// USAGE: Create via Assets > Create > Ludocore/Lifecycle Data.
//        Assign to Lifecycle component on the prefab.
//        One asset per entity type — Flora, Fauna, Apex each get their own.
// ============================================

using UnityEngine;

namespace Ludocore
{
    [CreateAssetMenu(fileName = "NewLifecycleData", menuName = "Ludocore/Lifecycle Data")]
    public class LifecycleData : ScriptableObject
    {
        [Header("Energy")]
        [SerializeField] private float startingEnergy = 100f;
        [SerializeField] private float maxEnergy = 100f;
        [SerializeField] private float energyDecayRate = 5f;

        public float StartingEnergy => startingEnergy;
        public float MaxEnergy => maxEnergy;
        public float EnergyDecayRate => energyDecayRate;
    }
}
