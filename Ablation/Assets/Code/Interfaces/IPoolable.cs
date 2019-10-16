using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>Marks an object as a poolable type.</summary>
public interface IPoolable
{
    /// <summary>Provides a reference to the gameobject this interface is attached to.</summary>
    GameObject GameObject { get; }
    /// <summary>Set's the current pool for this object to be returned to.</summary>
    /// <param name="pool">The object pool this object is associated with.</param>
    void SetPool (Pool pool);
    /// <summary>Cull's the current object from play and returns them to the pool.</summary>
    void Cull ();
}
