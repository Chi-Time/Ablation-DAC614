using UnityEngine;

class ObstacleSpawner : MonoBehaviour
{

    [Tooltip ("The min and max time an obstacle can take to spawn.")]
    [SerializeField] private Range _SpawnTimeRange = new Range (0.5f, 1.5f);
    [Tooltip ("The min and max positions an obstacle can spawn.")]
    [SerializeField] private RangeVector2 _SpawnPositionRange = new RangeVector2 (new Vector2 (-10f, -7.5f), new Vector2 (10f, 7.5f));
    [Tooltip ("The depth position to begin spawning obstacles from.")]
    [SerializeField] private float _ZSpawnPosition = 20f;
    [Tooltip ("The pool containing the various obstacles.")]
    [SerializeField] private Pool _ObstaclePool = new Pool ();

    private void OnEnable ()
    {
        Signals.OnTimeEvent += OnTimeEvent;
    }

    void OnTimeEvent (float speedOFfset, Range forceOffset, Range spawnOffset, Range exposureOffset, float fadeTime)
    {
        _SpawnTimeRange = spawnOffset;
    }

    private void OnDisable ()
    {
        Signals.OnTimeEvent -= OnTimeEvent;
    }

    private void Awake ()
    {
        _ObstaclePool.Constructor ("Obstacle Pool", "Obstacle");
    }

    private void Start ()
    {
        Invoke ("SpawnObstacle", GetSpawnTime ());
    }

    private float GetSpawnTime ()
    {
        return Random.Range (_SpawnTimeRange.Min, _SpawnTimeRange.Max);
    }

    private void SpawnObstacle ()
    {
        var obstacle = _ObstaclePool.RetrieveFromPoolRandom ();

        if (obstacle != null)
        {
            obstacle.GameObject.transform.position = GetSpawnPosition ();
        }
    
        Invoke ("SpawnObstacle", GetSpawnTime ());
    }

    private Vector3 GetSpawnPosition ()
    {
        float x = Random.Range (_SpawnPositionRange.Min.x, _SpawnPositionRange.Max.x);
        float y = Random.Range (_SpawnPositionRange.Min.y, _SpawnPositionRange.Max.y);
        return new Vector3 (x, y, _ZSpawnPosition);
    }
}
