using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ludocore
{
    /// <summary>Instantiates prefabs at a point or within a randomized area.</summary>
    public class Spawner : MonoBehaviour
    {
        // ═══════════════════════════════════════
        // CONFIG
        // ═══════════════════════════════════════
        [Header("Config")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private int count = 1;

        [Header("Position")]
        [Tooltip("Where to spawn. Uses this transform if empty.")]
        [SerializeField] private Transform spawnPoint;

        [Tooltip("Randomization area around spawn point. Zero = exact point.")]
        [SerializeField] private Vector3 areaSize;

        [Header("Rotation")]
        [SerializeField] private bool useSpawnRotation;
        [SerializeField] private bool randomRotation;

        [Header("Parenting")]
        [SerializeField] private Transform parent;

        // ═══════════════════════════════════════
        // STATE
        // ═══════════════════════════════════════
        private int _totalSpawned;

        public int TotalSpawned => _totalSpawned;
        public GameObject LastSpawned { get; private set; }

        // ═══════════════════════════════════════
        // OUTPUTS
        // ═══════════════════════════════════════
        public event Action<GameObject> OnSpawned;

        [Header("Events")]
        [SerializeField] private UnityEvent<GameObject> spawnedEvent;

        // ═══════════════════════════════════════
        // INPUTS
        // ═══════════════════════════════════════

        /// <summary>Spawn the configured number of instances.</summary>
        [ContextMenu("Spawn")]
        public void Spawn()
        {
            if (!prefab) return;

            for (int i = 0; i < count; i++)
                SpawnOne();
        }

        /// <summary>Spawn a single instance, ignoring count.</summary>
        public void SpawnOne()
        {
            if (!prefab) return;

            var instance = Instantiate(prefab, GetPosition(), GetRotation(), parent);
            LastSpawned = instance;
            _totalSpawned++;

            OnSpawned?.Invoke(instance);
            spawnedEvent?.Invoke(instance);
        }

        /// <summary>Spawn a specific prefab, overriding the configured one.</summary>
        public void Spawn(GameObject overridePrefab)
        {
            if (!overridePrefab) return;

            var instance = Instantiate(overridePrefab, GetPosition(), GetRotation(), parent);
            LastSpawned = instance;
            _totalSpawned++;

            OnSpawned?.Invoke(instance);
            spawnedEvent?.Invoke(instance);
        }

        // ═══════════════════════════════════════
        // PRIVATE
        // ═══════════════════════════════════════
        private Vector3 GetPosition()
        {
            var origin = spawnPoint ? spawnPoint.position : transform.position;

            if (areaSize == Vector3.zero) return origin;

            return origin + new Vector3(
                UnityEngine.Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                UnityEngine.Random.Range(-areaSize.y / 2f, areaSize.y / 2f),
                UnityEngine.Random.Range(-areaSize.z / 2f, areaSize.z / 2f)
            );
        }

        private Quaternion GetRotation()
        {
            if (randomRotation) return UnityEngine.Random.rotation;
            if (useSpawnRotation && spawnPoint) return spawnPoint.rotation;
            return Quaternion.identity;
        }

        // ═══════════════════════════════════════
        // GIZMOS
        // ═══════════════════════════════════════
        private void OnDrawGizmosSelected()
        {
            var origin = spawnPoint ? spawnPoint.position : transform.position;

            if (areaSize != Vector3.zero)
            {
                Gizmos.color = new Color(0f, 1f, 1f, 0.15f);
                Gizmos.DrawCube(origin, areaSize);
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(origin, areaSize);
            }
            else
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(origin, 0.15f);
            }
        }
    }
}
