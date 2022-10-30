using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private float _health = 100f;
    private float _fillCapacity = 100f;
    private float _fillLevel = 0f;

    public event System.Action<float> HealtChanged;
    public event System.Action<float> PlayerDied;
    public event System.Action<float> FillChanged;
    public event System.Action<float, float> FillCapacityHit;

    public float Health
    {
        get { return _health; }
        set
        {
            if (value == _health)
                return;
            _health = value;
            HealtChanged?.Invoke(value);

            if (value <= 0f)
                PlayerDied?.Invoke(value);
        }
    }
    public float Fill
    {
        get { return _fillLevel; }
        set
        {
            if (value == _fillLevel)
                return;
            _fillLevel = value;
            FillChanged?.Invoke(value);

            if (value >= _fillCapacity)
                FillCapacityHit.Invoke(value, _fillCapacity);
        }
    }

    public float FillCapacity
    {
        get { return _fillCapacity; }
    }
}
