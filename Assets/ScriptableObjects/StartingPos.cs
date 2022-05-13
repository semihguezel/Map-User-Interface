using UnityEngine;

namespace ScriptableObjects
{
    // fileName is the default name when creating a new Instance
// menuName is where to find it in the context menu of Create

    [CreateAssetMenu(fileName = "Data", menuName = "Examples/ScriptableObject")]
    public class StartingPos : ScriptableObject
    {
        public CustomDataClass someCustomData = null;
    }

    public class CustomDataClass
    {
        public Vector3 custom;
    }
}