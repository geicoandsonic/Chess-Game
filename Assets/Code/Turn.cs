using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public float speed = 5;
    private Quaternion currRot;
    private Quaternion newRot;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        if (Input.GetAxis("Mouse Y") != 0)
        {
            currRot = transform.rotation;
            newRot = Quaternion.Euler(Input.GetAxis("Mouse Y") * -speed * Time.deltaTime, 0, 0);
            Quaternion tempRot = currRot * newRot;
            if (tempRot.eulerAngles.x <= 45 || tempRot.eulerAngles.x >= 315)
            {
                transform.rotation = tempRot;
            }
        }


        if (Input.GetKey(KeyCode.Mouse0))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.Mouse1))
        {
            transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        }
        if(Input.GetAxis("Mouse X") != 0)
        {
            currRot = transform.rotation;
            newRot = Quaternion.Euler(0, Input.GetAxis("Mouse X") * speed * Time.deltaTime, 0);
            transform.rotation = newRot * currRot;
        }
        



    }
}
