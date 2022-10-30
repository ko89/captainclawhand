using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _text;

    private float _time;
    private DateTime _startTime;

    public float TimeValue
    {
        get { return _time; }
        set
        {
            if (value == _time)
                return;
            _time = value;

        }
    }

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        _time += Time.deltaTime;
        SetUIText(DateTime.Now - _startTime);
    }

    public void StartTimer()
    {
        _time = 0f;
        _startTime = DateTime.Now;
        enabled = true;
    }

    public void StopTimer()
    {
        enabled = false;
    }

    private void SetUIText(TimeSpan value)
    {
        _text.text = value.ToString(@"mm\:ss\.fff");
    }
}
