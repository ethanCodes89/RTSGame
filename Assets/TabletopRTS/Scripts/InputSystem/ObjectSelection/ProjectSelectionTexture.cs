using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class ProjectSelectionTexture : MonoBehaviour
{
    [FormerlySerializedAs("UnitRef")] public BaseSelectable selectableRef;
    private DecalProjector selectionProjector;
    private void Awake()
    {
        selectionProjector = GetComponentInChildren<DecalProjector>();
    }

    private void Update()
    {
        selectionProjector.gameObject.SetActive(selectableRef.IsSelected);
    }
}
