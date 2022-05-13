using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GetInput : MonoBehaviour
{
    private string _getX;
    private string _getZ;
    private string _getRotation;
    private Vector3 _initialPos ;
    private InputConnection _inputConnection;
    [SerializeField] StartingPos startingPos;
    [SerializeField] GameObject inputFieldX;
    [SerializeField] GameObject inputFieldZ;
    [SerializeField] GameObject inputFieldR;

    private void Start()
    {
        _inputConnection = GameObject.Find("Connection").GetComponent<InputConnection>();
    }

    public void ReadStringInput()
    {
        _getX = inputFieldX.GetComponent<Text>().text;
        _getZ = inputFieldZ.GetComponent<Text>().text;
        _getRotation = inputFieldR.GetComponent<Text>().text;
        Debug.Log("input x"+_getX + "input y"+_getZ +"input rot"+ _getRotation);
        if (!string.IsNullOrWhiteSpace(_getX) && !string.IsNullOrWhiteSpace(_getZ) && !string.IsNullOrWhiteSpace(_getRotation))
        {
            // if (_getX.All(Char.IsDigit) && _getZ.All(Char.IsDigit) && _getRotation.All(Char.IsDigit))
            if (!_getX.All(Char.IsDigit) && !_getZ.All(Char.IsLetter) && !_getRotation.All(Char.IsLetter))
            {
                _initialPos = new Vector3(float.Parse(_getX, CultureInfo.InvariantCulture.NumberFormat),
                                          float.Parse(_getZ, CultureInfo.InvariantCulture.NumberFormat),
                                          float.Parse(_getRotation, CultureInfo.InvariantCulture.NumberFormat));
                Debug.Log("initial pos of agv" + _initialPos);

                startingPos.someCustomData = new CustomDataClass{custom = _initialPos/10};
                _inputConnection.SendMessage("next connection,"+ _getX + "," + _getZ + "," + _getRotation);

                if (_inputConnection.ConnectionStatus() == 1)
                {
                    StartCoroutine(LoadNextScene());
                }
                

            }
            
            else
            {
                EditorUtility.DisplayDialog("Type error has been occured", " Input field type can not be a string","ok", "");
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Blank space error has been occured", " Input field can not be leave as empty","ok", "");
        }
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
    
}
