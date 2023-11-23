using System.Collections;
using Features.Game;
using Features.Motion;
using Features.Space;
using Tiger.Swizzles;
using Tiger.Util;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Asteroids
{
    public class AsteroidSpawner : GameStateResponder
    {
        [SerializeField]
        private GameObject[] asteroidPrefabs;

        [SerializeField][Tooltip("Time until first spawn, in seconds. Decrements 1 second with each wave.")]
        private float preGameTime = 10;

        [SerializeField] [Tooltip("Time between waves. Decrements 1 second with each wave.")]
        private float initialWaveTime = 10;

        [SerializeField][Tooltip("Minimum spawn wave time, in seconds.")]
        private float minSpawnTime = 3;

        private float _currentSpawnTime;
        private float _timer;
    
        private WorldBounds _world;

        private void Awake()
        {
            _world = GetComponentInParent<WorldBounds>();
        }
    
        private IEnumerator Spawning()
        {
            yield return new WaitForSeconds(preGameTime);
        
            _currentSpawnTime = initialWaveTime;

            while (Application.isPlaying)
            {
                var spawnRange = _world.bounds.size._x0z();
                var spawnCenter = _world.bounds.center;
                Vector3 spawnXZ = Random.insideUnitCircle.normalized._x0y() * 2 * spawnRange;

                //BUG: The bounds need to be expanded by the prefab's bounds, rarely you can see them popping in.
                var position = _world.bounds.ClosestPoint(spawnCenter + spawnXZ);
                var rotation = Random.rotationUniform;
                Instantiate(asteroidPrefabs.Pick(), position, rotation, transform.parent);
            
                yield return new WaitForSeconds(_currentSpawnTime);
                _currentSpawnTime = math.max(_currentSpawnTime-1, minSpawnTime);
            }
        }
    
        protected override void OnEvent(GameState data)
        {
            if (data == GameState.Alive) StartCoroutine(Spawning());
            else StopAllCoroutines();
        }
    }
}
