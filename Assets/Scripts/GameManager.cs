using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[Serializable]
public class LeaderboardEntry
{
    public string name;
    public int score;
    public float timeSpent; // seconds used to get the score
    public long timestamp;  // recency
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public class GameManager : MonoBehaviour
{
    [Header("Gameplay")]
    [Tooltip("Max time at the beginning of the game")]
    public int maxTimeSeconds = 50;

    [Header("UI References (TMP)")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI statusText;

    private float timeLeft;
    private float elapsed;
    private int score;
    private bool running;
    private bool sessionCompleted;

    private const string LEADERBOARD_KEY = "LEADERBOARD_RECENT3";
    private LeaderboardData leaderboard = new LeaderboardData();

    private string CurrentPlayerName
    {
        get
        {
            // Pulls from UserSession
            if (UserSession.Instance != null && !string.IsNullOrWhiteSpace(UserSession.Instance.Username))
                return UserSession.Instance.Username.Trim();
            return "Player";
        }
    }

    private void Awake()
    {
        LoadLeaderboard();
    }

    private void Start()
    {
        ResetSession();
        StartSession(); // Auto-start when game scene loads
        UpdateUI();     // Show initial HUD state immediately
    }

    private void Update()
    {
        if (!running) return;

        // Tick
        timeLeft -= Time.deltaTime;
        elapsed += Time.deltaTime;

        // Scoring rule: +10 every 5 seconds based on elapsed time
        int newScore = Mathf.FloorToInt(elapsed / 5f) * 10;
        if (newScore != score)
            score = newScore;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            EndSession();
        }

        UpdateUI();
    }

    public void StartSession()
    {
        running = true;
    }

    public void EndSession()
    {
        if (!running) return;
        running = false;

        SaveCurrentRun(); // store into recent-3 list
        UpdateUI();

        
    }

    public void RestartSession()
    {
        ResetSession();
        StartSession();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timeText != null) timeText.text = $"{Mathf.CeilToInt(timeLeft)}";
        if (scoreText != null) scoreText.text = $"{score}";
        if (statusText != null) statusText.text = BuildStatusText();
    }

    private string BuildStatusText()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("<b>Status</b>");

        if (leaderboard.entries.Count == 0)
        {
            sb.AppendLine("No runs yet.");
            return sb.ToString();
        }

        var list = new List<LeaderboardEntry>(leaderboard.entries);
        // Most recent first
        list.Sort((a, b) => b.timestamp.CompareTo(a.timestamp));

        int count = Mathf.Min(3, list.Count);
        for (int i = 0; i < count; i++)
        {
            var e = list[i];
            string name = string.IsNullOrEmpty(e.name) ? "Player" : e.name;
            int t = Mathf.RoundToInt(e.timeSpent);
            sb.AppendLine($"{i + 1}. {name} — Score: {e.score} — Time: {t}s");
        }

        return sb.ToString();
    }

    private void SaveCurrentRun()
    {
        var entry = new LeaderboardEntry
        {
            name = CurrentPlayerName,
            score = score,
            timeSpent = Mathf.Min(maxTimeSeconds, elapsed),
            timestamp = DateTime.UtcNow.Ticks
        };

        // Append, then keep only 3 most recent
        leaderboard.entries.Add(entry);
        leaderboard.entries.Sort((a, b) => b.timestamp.CompareTo(a.timestamp)); // newest first
        while (leaderboard.entries.Count > 3)
            leaderboard.entries.RemoveAt(leaderboard.entries.Count - 1);

        SaveLeaderboard();
    }

    private void LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey(LEADERBOARD_KEY))
        {
            string json = PlayerPrefs.GetString(LEADERBOARD_KEY, "");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var data = JsonUtility.FromJson<LeaderboardData>(json);
                    leaderboard = data ?? new LeaderboardData();
                }
                catch
                {
                    leaderboard = new LeaderboardData();
                }
            }
            else
            {
                leaderboard = new LeaderboardData();
            }
        }
        else
        {
            leaderboard = new LeaderboardData();
        }
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboard);
        PlayerPrefs.SetString(LEADERBOARD_KEY, json);
        PlayerPrefs.Save();
    }

    [ContextMenu("Clear Leaderboard")]
    public void ClearLeaderboard()
    {
        leaderboard = new LeaderboardData();
        PlayerPrefs.DeleteKey(LEADERBOARD_KEY);
        UpdateUI();
    }


    //GAME END AND START IN UI

    public void StartSessionFromStartZone()
    {
        if (sessionCompleted) return;
        ResetSession();
        StartSession();
        UpdateUI();
    }

    public void EndSessionByGoal()
    {
        if (sessionCompleted) return;
        sessionCompleted = true;

        // We’re done: save this run as a completed/win state.
        // If you want to differentiate, you could add a field "won" to LeaderboardEntry.
        EndSession(); // This will save to recent-3 via SaveCurrentRun()
    }

    // Make sure ResetSession() clears the guard:
    public void ResetSession()
    {
        timeLeft = Mathf.Max(1, maxTimeSeconds);
        elapsed = 0f;
        score = 0;
        running = false;
        sessionCompleted = false;
    }

}
