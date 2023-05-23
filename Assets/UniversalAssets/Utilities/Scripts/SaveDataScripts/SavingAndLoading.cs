using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UniversalAssetsProject.Utilities.SavingAndLoading
{
    public class SavingAndLoading : Singleton<SavingAndLoading>
    {
        public string GameName;
        public LoadablePrefabs LoadablePrefabs;
        private string saveFolder;
        private string completeFileLocation;
        private static Dictionary<string, ISaveable> saveableEntities = new Dictionary<string, ISaveable>();
        private void Awake()
        {
            saveFolder = Application.persistentDataPath + "/Saves/";
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            completeFileLocation = saveFolder + GameName + ".json";
            Debug.Log("Savepath located at: " + saveFolder); //Helps us find the save file location
        }

        #region SaveAndLoadCommands
        public void Save()
        {
            var state = File.Exists(completeFileLocation) ? JsonConversions.DeserializeToDictionary(completeFileLocation) : new Dictionary<string, object>();
            CaptureState(state);
            JsonConversions.SerializeToJson(state, completeFileLocation);
        }

        public void Load()
        {
            if (!File.Exists(completeFileLocation))
            {
                Debug.LogError("No File To Load");
                return;
            }
            var state = JsonConversions.DeserializeToDictionary(completeFileLocation);
            SpawnMissingPrefabs(state);
            RestoreState(state);
        }

        public void DeleteData()
        {
            if (File.Exists(completeFileLocation))
                File.Delete(completeFileLocation);
        }
        #endregion

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.Id] = saveable.CaptureState();
            }
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {

                if (state.TryGetValue(saveable.Id, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }

        private void SpawnMissingPrefabs(Dictionary<string, object> state) //TODO: can this be reworked and optimized? I'm concerned about scalability with larger games
        {
            foreach (var item in state)
            {
                if(!HasSaveableEntityWithId(item.Key))
                {
                    Dictionary<string, object> data = JsonConversions.DeserializeToDictionary(item.Value);

                    foreach(KeyValuePair<string, object> classLevelObject in data) //Double foreach loop to get down to the objects within the save data class
                    {
                        foreach(var entry in JsonConversions.DeserializeToDictionary(classLevelObject.Value))
                        {
                            if (entry.Key == "prefabName")
                            {
                                string prefabName = entry.Value.ToString();
                                GameObject prefab = LoadablePrefabs.GetPrefab(prefabName); //Get a reference to the prefab from our scriptable object

                                if (prefab == null)
                                {
                                    Debug.LogError("Prefab Not Found");
                                    continue;
                                }
                                GameObject spawnedObject = Instantiate(prefab);
                                SaveableEntity saveable = spawnedObject.GetComponent<SaveableEntity>(); 

                                if (saveable == null)
                                {
                                    Debug.LogError($"Prefab {prefabName} does not have a saveable entity attached to it.");
                                    continue;
                                }
                                saveable.SetId(item.Key);
                                saveableEntities[item.Key] = saveable.GetComponent<ISaveable>();
                            }
                        }
                    }
                }
            }
        }

        private bool HasSaveableEntityWithId(string id)
        {
            var saveableEntities = FindObjectsOfType<SaveableEntity>();
            foreach(var saveableEntity in saveableEntities)
            {
                if(saveableEntity.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public void RegisterSaveableEntity(string id, ISaveable saveable)
        {
            if (!saveableEntities.ContainsKey(id))
            {
                saveableEntities.Add(id, saveable);
            }
        }

        public void UnregisterSaveableEntity(string id)
        {
            if (saveableEntities.ContainsKey(id))
            {
                saveableEntities.Remove(id);
            }
        }
    }
}
