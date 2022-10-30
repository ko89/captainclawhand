using UnityEngine;

public class BoatFill : MonoBehaviour
{
    [SerializeField]
    private Transform _playerUp;
    [SerializeField]
    private float _fillSpeed = 1f;
    [SerializeField]
    private float _fillThreshold = 0.5f;
    [SerializeField]
    private float _maxCapacity = 100;


    private float _fillLevel = 0f;

    private Vector3 _worldUp = Vector3.up;

    public event System.Action<float> FillChanged;
    public event System.Action<float> FillMaxReachedChanged;

    public float Fill
    {
        get { return _fillLevel; }
        set
        {
            if (value == _fillLevel)
                return;
            _fillLevel = value;
            FillChanged?.Invoke(value);

            if (value >= _maxCapacity)
                FillMaxReachedChanged.Invoke(value);
        }
    }

    public float MaxCapacity
    {
        get { return _maxCapacity; }
    }

    private void Update()
    {
        if (_playerUp == null)
            return;

        var dot = Vector3.Dot(_playerUp.up, _worldUp);
        if (dot < _fillThreshold)
            Fill += _fillSpeed * Time.deltaTime;
    }
}
