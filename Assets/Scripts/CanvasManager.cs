using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CanvasManager : MonoBehaviour
{



    //START AND FINISH GAME FOR HOME SCENE:
    public void startGameButton()
    {
        //loads scene with index 1 when function is used
        SceneManager.LoadScene(1);
    }

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
