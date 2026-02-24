using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StartZone : MonoBehaviour
{
    [Tooltip("Tag used by the player GameObject.")]
    public string playerTag = "Player";

    private bool triggered = false;

    private void Reset()
    {
        // mark collider as trigger in editor when script is added before i forget lool
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        var gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.StartSessionFromStartZone();
        }
        else
        {
            UnityEngine.Debug.LogWarning("GameManager not found in scene for StartZone.");
        }

        triggered = true; // Avoid retriggering if player re-enters
    }
}