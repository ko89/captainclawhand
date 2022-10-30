using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _panelHowTo;
    [SerializeField]
    private Selectable _selectable;

    private void Start()
    {
        if (_selectable != null)
            _selectable.Select();

        if (_panelHowTo != null)
            _panelHowTo.SetActive(false);
    }

    public void HandleStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");
    }

    public void HandleHowTo()
    {
        if (_panelHowTo != null)
            _panelHowTo.SetActive(!_panelHowTo.activeSelf);
    }

    public void HandleQuit()
    {
        Application.Quit();
    }
}
