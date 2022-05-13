using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
public class GetCoordinates : MonoBehaviour
{
    private float[] _posAgv= new float[3];
    private int _index;
    public float[] ReadFile() {
        Thread.Sleep(1100);
        var source = new StreamReader("C:/Users/serba/Desktop/unity_python/agv_pos.txt");
        var fileContents = source.ReadToEnd();
        
        var lines = fileContents.Split("\n"[0]).Where(x => !string.IsNullOrWhiteSpace(x));;

        foreach (string line in lines)
        {
            float newLine = float.Parse(line, CultureInfo.InvariantCulture.NumberFormat);
            // float newLine = Convert.ToSingle(line);
            Debug.Log(newLine);
            if (_index < 3)
            {
                _posAgv[_index] = newLine;
                _index++;
            }
            
        }
        source.Close();
        return _posAgv;
    }
    
}
