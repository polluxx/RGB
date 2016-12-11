using System;

[Serializable]
public class IntRandom {

    public int min;
    public int max;

    public IntRandom(int minimum, int maximum)
    {
        min = minimum;
        max = maximum;
    }

    public int Random
    {
        get { return UnityEngine.Random.Range(min, max); }
    }
}
