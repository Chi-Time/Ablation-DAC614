using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GameController
{
    private Pool _ObstaclePool = new Pool ();

    private void Awake ()
    {
        _ObstaclePool.Constructor ("Obstacle Pool", "Obstacle");
    }
}
