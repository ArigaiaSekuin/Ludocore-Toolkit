using UnityEngine;

namespace Ludocore
{
    /// <summary>Flora behavior: spawns plants as energy grows, replicates at threshold.</summary>
    public class FloraController : MonoBehaviour
    {
        [Header("Modules")]
        [SerializeField] private Lifecycle lifecycle;
        [SerializeField] private Spawner plantSpawner;
        [SerializeField] private Spawner replicationSpawner;

        [Header("Settings")]
        [SerializeField] private float energyPerPlant = 5f;
        [SerializeField] private float replicationEnergy = 200f;

        private float _nextPlantThreshold;
        private bool _hasReplicated;

        private void OnEnable()
        {
            _nextPlantThreshold = lifecycle.CurrentEnergy + energyPerPlant;
            _hasReplicated = false;
            lifecycle.OnEnergyChanged += HandleEnergyChanged;
        }

        private void OnDisable()
        {
            lifecycle.OnEnergyChanged -= HandleEnergyChanged;
        }

        private void HandleEnergyChanged(float energy)
        {
            CheckPlant(energy);
            CheckReplication(energy);
        }

        private void CheckPlant(float energy)
        {
            if (!plantSpawner) return;
            if (energy < _nextPlantThreshold) return;

            plantSpawner.SpawnOne();
            _nextPlantThreshold += energyPerPlant;
        }

        private void CheckReplication(float energy)
        {
            if (_hasReplicated) return;
            if (energy < replicationEnergy) return;

            _hasReplicated = true;
            replicationSpawner.SpawnOne();
            Destroy(gameObject);
        }
    }
}
