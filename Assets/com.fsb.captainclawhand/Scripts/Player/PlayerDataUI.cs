using UnityEngine;
using UnityEngine.UI;

public class PlayerDataUI : MonoBehaviour
{
    [SerializeField]
    private Slider _sliderFill;
    [SerializeField]
    private Slider _sliderHealth;

    [SerializeField]
    private PlayerData _playerData;

    [SerializeField]
    private GameObject _panelGameOver;

    void Start()
    {
        if (_playerData != null)
        {
            if (_sliderFill != null)
                _sliderFill.maxValue = _playerData.FillCapacity;
            if (_sliderHealth != null)
                _sliderHealth.maxValue = _playerData.Health;

            if (_panelGameOver != null)
                _panelGameOver.SetActive(false);

            _playerData.FillChanged += HandleFillChanged;
            _playerData.FillCapacityHit += HandleFillCapacityHit;

            _playerData.HealtChanged += HandleHealtChanged;
            _playerData.PlayerDied += HandlePlayerDied;

            HandleFillChanged(_playerData.Fill);
        }
    }

    private void HandlePlayerDied(float obj)
    {
        if (_panelGameOver != null)
            _panelGameOver.SetActive(true);
    }

    private void HandleHealtChanged(float value)
    {
        if (_sliderFill != null)
            _sliderFill.value = value;
    }

    private void OnDestroy()
    {
        if (_playerData != null)
        {
            _playerData.FillChanged -= HandleFillChanged;
            _playerData.FillCapacityHit -= HandleFillCapacityHit;

            _playerData.HealtChanged -= HandleHealtChanged;
            _playerData.PlayerDied -= HandlePlayerDied;
        }
    }

    private void HandleFillCapacityHit(float value, float value2)
    {
        if (_panelGameOver != null)
            _panelGameOver.SetActive(true);
    }

    private void HandleFillChanged(float value)
    {
        if (_sliderFill != null)
            _sliderFill.value = value;
    }
}
