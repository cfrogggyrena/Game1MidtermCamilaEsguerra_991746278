using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] cams;
    int camNum = 0;

    void SetCams()
    {
        camNum++;

        if (camNum >= cams.LongLength)
            camNum = 0;
        foreach (Camera c in cams)
            c.enabled = false;


        cams[camNum].enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SetCams();
    }
}
