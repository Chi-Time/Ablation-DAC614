using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SceneryPool : MonoBehaviour
{
    [Tooltip ("The speed at which the scenery moves down the screen.")]
    [SerializeField] private float _Speed = 0.0f;
    [Tooltip ("The offset size of each scenery set to place.")]
    [SerializeField] private float _Offset = 40f;
    [SerializeField] private float _CullingPoint = -40f;
    [SerializeField] private int _PrespawnSize = 10;
    [SerializeField] private Pool _ScenerySetPool = new Pool ();

    private Transform _CurrentSceneSet = null;

    private void OnEnable ()
    {
        Signals.OnTimeEvent += OnTimeEvent;
    }

    void OnTimeEvent (float speedOFfset, Range forceOffset, Range spawnOffset, Range exposureOffset, float fadeTime)
    {
        _Speed = speedOFfset;
    }

    private void OnDisable ()
    {
        Signals.OnTimeEvent -= OnTimeEvent;
    }

    private void Awake ()
    {
        _ScenerySetPool.Constructor ("Scenery Set Pool", "Scenery Set");

        for (int i = 0; i < _PrespawnSize; i++)
        {
            SpawnScenerySet ();
        }
    }

    private void SpawnScenerySet ()
    {
        var poolObject = _ScenerySetPool.RetrieveFromPoolRandom ();

        if (poolObject != null)
        {
            var set = poolObject.GameObject.GetComponent<ScenerySet> ();

            if (set != null)
            {
                set.Speed = _Speed;
                set.CullingPoint = _CullingPoint;

                if (_CurrentSceneSet == null)
                {
                    set.transform.position = Vector3.zero;
                    _CurrentSceneSet = set.transform;

                    return;
                }

                set.transform.position = _CurrentSceneSet.transform.position + new Vector3 (0.0f, 0.0f, _Offset);
                _CurrentSceneSet = set.transform;
            }
        }

        Invoke ("SpawnScenerySet", .1f);
    }
}
