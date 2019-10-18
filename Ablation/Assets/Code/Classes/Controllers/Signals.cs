using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class Signals
{
    public static event Action OnCharacterHit;
    public static void HitCharacter () { OnCharacterHit?.Invoke (); }

    public static event Action OnShowText;
    public static void ShowText () { OnShowText?.Invoke (); }

    public static event Action OnBoySeizure;
    public static void SeizureBoy () { OnBoySeizure?.Invoke (); }

    public static event Action<GameObject, GameObject> OnObstacleCollision;
    public static void ObstacleCollision (GameObject player, GameObject obstacle) { OnObstacleCollision?.Invoke (player, obstacle); }

    public static event Action OnGameOver;
    public static void GameOver () { OnGameOver?.Invoke (); }

    public static event Action<float, Range, Range, Range, float> OnTimeEvent;
    public static void CallTimeEvent (float speedOFfset, Range forceOffset, Range spawnOffset, Range exposureOffset, float fadeTime) { OnTimeEvent?.Invoke (speedOFfset, forceOffset, spawnOffset, exposureOffset, fadeTime); }

    //public static event Action OnGameOver;
    //public static void GameOver () { if (OnGameOver != null) OnGameOver (); }
}
