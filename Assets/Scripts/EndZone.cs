using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EndZone : MonoBehaviour
{
    [Tooltip("Tag used by the player GameObject.")]
    public string playerTag = "Player";

    private bool triggered = false;

    private void Reset()
    {
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
            
            gm.EndSessionByGoal();
            
        }
        else
        {
            UnityEngine.Debug.LogWarning("GameManager not found in scene for EndZone.");
        }

        triggered = true;
    }
}