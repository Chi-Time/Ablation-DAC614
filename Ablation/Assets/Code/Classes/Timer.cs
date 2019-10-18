using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
class TimeEvent
{
    public string EventName = "";
    public float TimeStamp = 0.0f;
    public float SpeedOffset = 0.0f;
    public Range ForceOffset = new Range (0.0f, 0.0f);
    public Range SpawnOffset = new Range (0.0f, 0.0f);
    public Range ExposureOffset = new Range (0.0f, 0.0f);
    public float FadeTime = 0.0f;
}

class Timer : MonoBehaviour
{
    public List<TimeEvent> _TimeEvents = new List<TimeEvent> ();

    private List<TimeEvent> _CachedTimeEvents = new List<TimeEvent> ();

    [SerializeField] private float _CurrentTime = 0.0f;

    private void Awake ()
    {
        
    }

    private void FixedUpdate ()
    {
        _CurrentTime += Time.deltaTime;

        if (Input.GetMouseButtonUp (0))
        {
            _CachedTimeEvents.Add (new TimeEvent
            {
                TimeStamp = _CurrentTime
            });
        }

        for (int i = _TimeEvents.Count () - 1; i >= 0; i--)
        {
            var timeEvent = _TimeEvents[i];

            if (Approximately (timeEvent.TimeStamp, _CurrentTime, 0.01f))
            {
                Signals.CallTimeEvent (timeEvent.SpeedOffset, timeEvent.ForceOffset, timeEvent.SpawnOffset, timeEvent.ExposureOffset, timeEvent.FadeTime);
                _TimeEvents.Remove (timeEvent);
            }
        }

        if (_TimeEvents.Count <= 0)
        {
            Signals.GameOver ();
        }
    }

    private void OnDisable ()
    {
        foreach (TimeEvent timeEvent in _CachedTimeEvents)
        {
            Debug.Log (timeEvent.TimeStamp);
        }
    }

    private bool Approximately (float a, float b, float tolerance)
    {
        return (Mathf.Abs (a - b) < tolerance);
    }
}
