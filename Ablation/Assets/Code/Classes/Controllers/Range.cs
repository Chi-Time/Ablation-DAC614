using UnityEngine;

[System.Serializable]
struct TRange<T>
{
    public T Min;
    public T Max;

    public TRange (T min, T max)
    {
        Min = min;
        Max = max;
    }
}

/// <summary>Supports holding a range from minimum value to maximum value.</summary>
[System.Serializable]
struct Range
{
    /// <summary>The minimum value this range can hold.</summary>
    public float Min;
    /// <summary>The maximum value this range can hold.</summary>
    public float Max;
    /// <summary>Shorthand for Range (0.0f, 0.0f)</summary>
    public static Range Zero = new Range (0.0f, 0.0f);

    public Range (float min, float max)
    {
        Min = min;
        Max = max;
    }

    public static Range operator + (Range a, Range b)
    {
        return new Range (a.Min + b.Min, a.Max + b.Max);
    }

    public static Range operator - (Range a, Range b)
    {
        return new Range (a.Min - b.Min, a.Max - b.Max);
    }

    public static Range operator * (Range a, Range b)
    {
        return new Range (a.Min * b.Min, a.Max * b.Max);
    }

    public static Range operator / (Range a, Range b)
    {
        return new Range (a.Min / b.Min, a.Max / b.Max);
    }
}

/// <summary>Supports holding a range from minimum value to maximum value.</summary>
[System.Serializable]
struct RangeInt
{
    /// <summary>The minimum value this range can hold.</summary>
    public int Min;
    /// <summary>The maximum value this range can hold.</summary>
    public int Max;
    /// <summary>Shorthand for Range (0, 0)</summary>
    public static RangeInt Zero = new RangeInt (0, 0);

    public RangeInt (int min, int max)
    {
        Min = min;
        Max = max;
    }
}

/// <summary>Supports holding a range from minimum value to maximum value.</summary>
[System.Serializable]
struct RangeVector2
{
    /// <summary>The minimum value this range can hold.</summary>
    public Vector2 Min;
    /// <summary>The maximum value this range can hold.</summary>
    public Vector2 Max;
    /// <summary>Shorthand for Range (Vector2 (0, 0), Vector2 (0, 0))</summary>
    public static RangeVector2 Zero = new RangeVector2 (Vector2.zero, Vector2.zero);

    public RangeVector2 (Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
    }
}

/// <summary>Supports holding a range from minimum value to maximum value.</summary>
[System.Serializable]
struct RangeVector3
{
    /// <summary>The minimum value this range can hold.</summary>
    public Vector3 Min;
    /// <summary>The maximum value this range can hold.</summary>
    public Vector3 Max;
    /// <summary>Shorthand for Range (Vector3 (0, 0, 0), Vector3 (0, 0, 0))</summary>
    public static RangeVector3 Zero = new RangeVector3 (Vector3.zero, Vector3.zero);

    public RangeVector3 (Vector3 min, Vector3 max)
    {
        Min = min;
        Max = max;
    }
}
