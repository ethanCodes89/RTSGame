using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadablePrefabs", menuName = "ScriptableObjects/LoadablePrefabs")]
public class LoadablePrefabs : ScriptableObject
{
    public List<GameObject> Prefabs;

    public GameObject GetPrefab(string prefabName)
    {
        foreach (GameObject prefab in Prefabs)
        {
            if (prefab.name == prefabName)
            {
                return prefab;
            }
        }
        return null;
    }
}
