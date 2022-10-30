using UnityEngine;
using UnityEngine.UI;

public class BoatFillUI : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private BoatFill _boatFill;

    [SerializeField]
    private GameObject _panelGameOver;
    
    void Start()
    {
        if (_boatFill != null)
        {
            if (_slider != null)
                _slider.maxValue = _boatFill.MaxCapacity;

            if (_panelGameOver != null)
                _panelGameOver.SetActive(false);

            _boatFill.FillChanged += HandleBoatFillFillChanged;
            _boatFill.FillMaxReachedChanged += HandleBoatFillFillMaxReachedChanged;

            HandleBoatFillFillChanged(_boatFill.Fill);
        }
    }

    private void OnDestroy()
    {
        if (_boatFill != null)
        {
            _boatFill.FillChanged -= HandleBoatFillFillChanged;
            _boatFill.FillMaxReachedChanged -= HandleBoatFillFillMaxReachedChanged;
        }
    }

    private void HandleBoatFillFillMaxReachedChanged(float obj)
    {
        if (_panelGameOver != null)
            _panelGameOver.SetActive(true);
    }

    private void HandleBoatFillFillChanged(float value)
    {
        if (_slider != null)
            _slider.value = value;
    }
}
