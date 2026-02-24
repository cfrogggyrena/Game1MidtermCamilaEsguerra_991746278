using System.Diagnostics;
using UnityEngine;

public class TerrainToggle : MonoBehaviour
{
    [SerializeField]
    private GameObject terrainObject;

    private bool isVisible = false;

    public void ToggleTerrain()
    {
        if (terrainObject == null)
        {
            UnityEngine.Debug.LogWarning("Terrain is not assigned!");
            return;
        }

        isVisible = !isVisible;
        terrainObject.SetActive(isVisible);
    }

    void Start()
    {
        terrainObject.SetActive(isVisible);
    }
}
