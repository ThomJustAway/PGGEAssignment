using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeCameraScript : MonoBehaviour
{
    private CameraType currentCameraType;
    [SerializeField] private ThirdPersonCamera camera;
    [SerializeField] private Text textToShowcaseTheCamera;
    private string constantWords = "Camera Type: ";
    private int count = 0;
    private void Start()
    {
        currentCameraType = CameraType.Follow_Track_Pos_Rot;
        ChangingCameraValue();

    }

    private void ChangingCameraValue()
    {
        camera.ChangeCameraType(currentCameraType);
        textToShowcaseTheCamera.text = constantWords + currentCameraType.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {

            ChangeCamera();
            ChangingCameraValue();
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            //quit the application
            Application.Quit();
        }
        else if (Input.GetKeyUp(KeyCode.Backspace))
        {
            //reset the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ChangeCamera()
    {
        count++;
        switch(count)
        {
            case 1:
                currentCameraType = CameraType.Follow_Track_Pos;
                break;
            case 2:
                currentCameraType = CameraType.Follow_Independent;
                break;
            case 3:
                currentCameraType = CameraType.Topdown;
                break;
            case 4:
                currentCameraType = CameraType.Track;
                break;

            default:
                count = 0;
                currentCameraType = CameraType.Follow_Track_Pos_Rot;
                break;

        }
    }
}
