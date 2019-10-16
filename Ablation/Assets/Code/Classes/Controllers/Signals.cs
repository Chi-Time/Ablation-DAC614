using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class Signals
{
    public static event Action<GameObject, GameObject> OnObstacleCollision;
    public static void ObstacleCollision (GameObject player, GameObject obstacle) { OnObstacleCollision?.Invoke (player, obstacle); }

    public static event Action OnGameOver;
    public static void GameOver () { OnGameOver?.Invoke (); }

    //public static event Action OnGameOver;
    //public static void GameOver () { if (OnGameOver != null) OnGameOver (); }
}
