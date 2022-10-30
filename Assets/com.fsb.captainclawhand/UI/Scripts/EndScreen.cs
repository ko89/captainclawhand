using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private Selectable _button;
    [SerializeField]
    private TMPro.TextMeshProUGUI _text;

    private void OnEnable()
    {
        if (_button != null)
            _button.Select();
    }

    public void ShowSuccess()
    {
        gameObject.SetActive(true);
        _text.text = "\\o/";
    }

    public void ShowFail()
    {
        gameObject.SetActive(true);
        _text.text = "/o\\";
    }

    public void HandleLeave()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }
}
