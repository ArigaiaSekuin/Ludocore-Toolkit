using UnityEngine;

namespace Ludocore
{
    /// <summary>Fauna behavior: flee threats, seek food, or wander. Replicates at high energy.</summary>
    public class FaunaController : MonoBehaviour
    {
        [Header("Modules")]
        [SerializeField] private Lifecycle lifecycle;
        [SerializeField] private NavMeshMotor motor;
        [SerializeField] private NavMeshWander wander;
        [SerializeField] private Sensor foodSensor;
        [SerializeField] private Sensor threatSensor;
        [SerializeField] private Spawner replicationSpawner;

        [Header("Behavior")]
        [SerializeField] private float fleeDistance = 10f;

        [Header("Replication")]
        [SerializeField] private float replicationThreshold = 0.8f;
        [SerializeField] private float replicationCost = 0.5f;

        private void Update()
        {
            if (!lifecycle.IsAlive) return;

            CheckReplication();

            bool busy = Flee() || Seek();
            wander.enabled = !busy;
        }

        private bool Flee()
        {
            if (!threatSensor.TryGetNearest(out var threat)) return false;

            var away = transform.position - threat.Object.transform.position;
            motor.MoveTo(transform.position + away.normalized * fleeDistance);
            return true;
        }

        private bool Seek()
        {
            if (!foodSensor.TryGetNearest(out var food)) return false;

            motor.MoveTo(food.Object.transform.position);
            return true;
        }

        private void CheckReplication()
        {
            if (lifecycle.EnergyRatio < replicationThreshold) return;

            replicationSpawner.SpawnOne();
            lifecycle.RemoveEnergy(lifecycle.CurrentEnergy * replicationCost);
        }
    }
}
