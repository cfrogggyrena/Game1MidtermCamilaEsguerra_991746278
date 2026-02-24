using UnityEngine;
using TMPro;
using System.Numerics;

public class PlayerNameTag : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private UnityEngine.Vector3 offset = new UnityEngine.Vector3(0f, 0f, 0f);

    private Transform cam;

    private void Start()
    {
        // if no session or username was set then fallback
        string nameToShow = (UserSession.Instance != null && !string.IsNullOrEmpty(UserSession.Instance.Username))
            ? UserSession.Instance.Username
            : "Player";

        if (nameText != null)
            nameText.text = nameToShow;

        cam = Camera.main?.transform;
    }

    private void LateUpdate()
    {
        if (nameText == null) return;

        // keeps label above the player (pray...)
        nameText.transform.position = transform.position + offset;

        // billboard toward camera, might delete lol still trying world
        if (cam != null)
        {
            nameText.transform.rotation = UnityEngine.Quaternion.LookRotation(nameText.transform.position - cam.position);
        }
    }
}