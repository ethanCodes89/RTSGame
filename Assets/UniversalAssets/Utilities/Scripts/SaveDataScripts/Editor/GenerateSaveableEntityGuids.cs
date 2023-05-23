using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace UniversalAssetsProject.Utilities.SavingAndLoading
{
    [CustomEditor(typeof(SavingAndLoading))]
    public class GenerateSaveableEntityGuids : Editor
    {
        private UnityEvent GetGUID;
        public string GenerateGUID()
        {
            return System.Guid.NewGuid().ToString(); //Only generate a guid if one doesn't already exist
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Generate GUIDs"))
            {
                var saveableEntities = FindObjectsOfType<SaveableEntity>();
                foreach (SaveableEntity entity in saveableEntities)
                {
                    if (entity.Id == "")
                    {
                        entity.SetId(GenerateGUID());
                    }
                }
            }

        }

    }
}

