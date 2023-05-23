using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalAssetsProject.Utilities.SavingAndLoading
{
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string id = string.Empty;
        public string Id => id;
        public void SetId(string id) { this.id = id; }

        private bool isQuitting = false;

        private void Start()
        {
            if (Id == "") //if a prefab is instantiated while playing, add a guid to it for saving purposes
            {
                SetId(System.Guid.NewGuid().ToString());
            }
            SavingAndLoading.Instance.RegisterSaveableEntity(id, GetComponent<ISaveable>());
        }

        public object CaptureState()
        {
            var state = new Dictionary<string, object>();

            foreach (var saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            var stateDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(state.ToString());

            foreach (var saveable in GetComponents<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();

                if (stateDictionary.TryGetValue(typeName, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
#region HandleApplicationQuit
        private void OnEnable()
        {
            Application.quitting += OnQuitting;
        }

        private void OnDisable()
        {
            Application.quitting -= OnQuitting;
        }

        private void OnQuitting()
        {
            isQuitting = true;
        }
        private void OnDestroy()
        {
            if (!isQuitting) //Only unregister if it's being unregistered through gameplay, not through the application ending.
                SavingAndLoading.Instance.UnregisterSaveableEntity(id);
        }
#endregion
    }
}

