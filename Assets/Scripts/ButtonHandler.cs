using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    private int _counter;
    private Vector3 _coords;
    private Driver _driver;
    private AgvConnection _connection;
    public GameObject radarWindow;
    public GameObject displayText;
    private void Start()
    {
        // var height = 2*Camera.main.orthographicSize;
        // var width = height*Camera.main.aspect;
        _driver = GameObject.Find("AGV").GetComponent<Driver>();
        _connection = GameObject.Find("AGV").GetComponent<AgvConnection>();
    }
    public void StartClicked()
    {
        _counter = 1;
        _connection.SendMessage("1");
    }
    public void StopClicked()
    {
        _connection.SendMessage("0");
        StartCoroutine(LoadFirstScene());
    }

    public void ShowHide()
    {
        if (radarWindow.activeInHierarchy)
        {
            radarWindow.SetActive(false);
        }
        else
        {
            radarWindow.SetActive(true);
        }
    }

    public void ShowPosition()
    {
        if (displayText.activeInHierarchy)
        {
            displayText.SetActive(false);
        }
        else
        {
            displayText.SetActive(true);
        }
    }
    private void Update()
    {
        if (_counter == 1)
        {
            _driver.DisplayAgv();
        }
        
    }
    IEnumerator LoadFirstScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
  
}
