using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class Driver : MonoBehaviour
{
    // Ctrl + K + C comment
    // Ctrl + K + U uncomment
    [SerializeField] private StartingPos getPos;
    public Text displayText;
    private Vector3 _getPosition;
    private int _scaleFactor = 4;
    private AgvConnection _agvConnection;
    
  
    void Start()
    {
        _agvConnection = GameObject.Find("AGV").GetComponent<AgvConnection>();
        // For initializing the AGV position
        _getPosition = getPos.someCustomData.custom;
        transform.position = new Vector3(_scaleFactor*_getPosition.x, 0.27f, _scaleFactor*_getPosition.y);
        transform.localRotation = Quaternion.Euler(0, _getPosition.z, 0);
        displayText.text = string.Format("PosX: {0} - PosZ: {1} - Rotation: {2}", _getPosition.x, _getPosition.y,
            _getPosition.z);
    }
    public void DisplayAgv()
    {
        _getPosition = _agvConnection.GetPosition();
        transform.position = new Vector3(_scaleFactor*_getPosition.x, 0.27f, _scaleFactor*_getPosition.y);
        transform.localRotation = Quaternion.Euler(0, _getPosition.z, 0);
        displayText.text = string.Format("PosX: {0} - PosZ: {1} - Rotation: {2}", _getPosition.x, _getPosition.y,
            _getPosition.z);
    }
 
}
