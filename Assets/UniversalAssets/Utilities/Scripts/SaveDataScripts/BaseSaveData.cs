using Newtonsoft.Json;
using UnityEngine;
/*This is a base class that can be inherited from for any object that needs to have persistent data saved between plays. 
 */
namespace UniversalAssetsProject.Utilities.SavingAndLoading
{
    [System.Serializable]
    public class BaseSaveData : MonoBehaviour, ISaveable
    {
        private string prefabName;

        private void Start()
        {
            if(prefabName == null)
            {
                string name = gameObject.name;
                //name.Replace("(Clone)", "");
                prefabName = name.Replace("(Clone)", "");
            }
        }

        public object CaptureState()
        {
            return new SaveData
            {
                position = transform.localPosition,
                rotation = transform.localRotation,
                scale = transform.localScale,
                prefabName = prefabName
            };
        }

        public void RestoreState(object state)
        {
            var saveData = JsonConvert.DeserializeObject<SaveData>(state.ToString());
            string prefabName = saveData.prefabName;
            transform.localPosition = saveData.position;
            transform.localRotation = saveData.rotation;
            transform.localScale = saveData.scale;
        }

        [System.Serializable]
        private struct SaveData
        {
            public string prefabName;
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
        }

    }
}

