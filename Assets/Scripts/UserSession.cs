using UnityEngine;

public class UserSession : MonoBehaviour
{
    public static UserSession Instance { get; private set; }
    public string Username { get; private set; } = string.Empty;

    private void Awake()
    {
        // tarts from the Game Settings scene only
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // survive into Game scene lmao
    }

    public void SetUsername(string name)
    {
        Username = name?.Trim() ?? string.Empty;
    }

    public void Clear()
    {
        Username = string.Empty;
    }
}
