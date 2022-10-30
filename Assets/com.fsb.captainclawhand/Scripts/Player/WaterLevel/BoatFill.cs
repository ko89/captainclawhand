using UnityEngine;

public class BoatFill : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;
    [SerializeField]
    private Transform _playerUp;
    [SerializeField]
    private float _fillSpeed = 1f;
    [SerializeField]
    private float _fillThreshold = 0.5f;
    
    private Vector3 _worldUp = Vector3.up;

    private void Update()
    {
        if (_playerData == null || _playerUp == null)
            return;

        var dot = Vector3.Dot(_playerUp.up, _worldUp);
        if (dot < _fillThreshold)
            _playerData.Fill += _fillSpeed * Time.deltaTime;
    }

}
