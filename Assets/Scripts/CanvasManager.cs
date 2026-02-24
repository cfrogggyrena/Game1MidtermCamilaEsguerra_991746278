using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class CanvasManager : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField usernameInput;


    //START AND FINISH GAME FOR HOME SCENE:
    public void startGameButton()
    {
        //INPUT SETTINGS FOR USERNAME
        string name = usernameInput.text?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            // show small warning in case
            UnityEngine.Debug.Log("Please enter a username.");
            return;
        }

        // make sure there's a session holder (added on usersession)
        if (UserSession.Instance == null)
        {
            //self check in case i forget tod add usersession lol
            UnityEngine.Debug.LogError("UserSession is missing in this scene. Add the UserSession prefab/object on hierarchy pls :D.");
            return;
        }

        UserSession.Instance.SetUsername(name);

        //LOADS SCENE WITH INDEX NUMBER 1
        SceneManager.LoadScene(1);
    }


    //GO HOME BUTTON

    public void homeButton()
    {
        SceneManager.LoadScene(0);
    }

    //EXIT GAME BUTTON
    public void exitGameButton()
    {
        //tried to do application.quit but it wasn't working on engine so I found this solution online :(
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If the user navigated back to this scene, repopulate if needed
        if (UserSession.Instance != null && !string.IsNullOrEmpty(UserSession.Instance.Username))
            usernameInput.text = UserSession.Instance.Username;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
