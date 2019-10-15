using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Script : MonoBehaviour
{
    [SerializeField] private float _Sensitivity = 15f;
    [SerializeField] private float speed;
    private Vector3 targetLocation;
    private Vector3 targetRotation;
    private float _PitchRot;
    private float _YawRot;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        targetLocation = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0.0f);// * _Sensitivity;
        targetRotation = new Vector3(0.0f, Input.GetAxis("Mouse X"), 0.0f);
        targetLocation.Normalize();

        _PitchRot = targetLocation.x;
        _YawRot = targetLocation.y;

        Debug.Log(targetLocation);

        

        transform.Translate(targetLocation * speed * Time.deltaTime);

        //Mathf.Clamp(targetRotation.z, -5f, 5f);


        //Mathf.Clamp(transform.eulerAngles.y, -5f, 5f);
        //transform.Rotate(targetRotation * speed * Time.deltaTime);
        

    }
}
