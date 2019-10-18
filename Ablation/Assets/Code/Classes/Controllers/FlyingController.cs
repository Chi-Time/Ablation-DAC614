using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
class FlyingController : MonoBehaviour
{
    enum InputType { Mouse, Keyboard, Joystick }

    [Tooltip ("The speed at which the player character moves.")]
    [SerializeField] private float _Speed = 10.0f;
    [Tooltip ("The speed at which the character rotates.")]
    [SerializeField] private float _RotationSpeed = 35.0f;
    [Tooltip ("The clamped rotation angles that the character can rotate to.")]
    [SerializeField] private Range _RotationRange = new Range (-15.0f, 15.0f);
    [Tooltip ("The clamped range of the characters movement on the X axis.")]
    [SerializeField] private Range _XPositionRange = new Range (-13.0f, 13.0f);
    [Tooltip ("The clamped range of the characters movement on the Y axis.")]
    [SerializeField] private Range _YPositionRange = new Range (-6.0f, 6.0f);
    [Tooltip ("The input device to use for controlling the character.")]
    [SerializeField] private InputType _InputType = InputType.Joystick;

    private float _YawAmount = 0.0f;
    private Vector3 _Velocity = Vector3.zero;
    private Rigidbody _Rigidbody = null;

    private void Awake ()
    {
        SetupRigidbody ();
    }

    private void SetupRigidbody ()
    {
        _Rigidbody = GetComponent<Rigidbody> ();
        _Rigidbody.isKinematic = true;
        _Rigidbody.useGravity = false;
        _Rigidbody.freezeRotation = true;
    }

    private void LockCursor (bool shouldLock)
    {
        if (shouldLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update ()
    {
        SelectInput ();
        Move ();
        Rotate ();
    }

    private void SelectInput ()
    {
        switch (_InputType)
        {
            case InputType.Joystick:
                GetInputSmoothed ("Horizontal LS", "Vertical LS");
                break;
            
            //case InputType.Keyboard:
            //    GetInput ("Horizontal", "Vertical");
            //    break;
            //case InputType.Mouse:
            //    GetInput ("Mouse X", "Mouse Y");
            //    break;
        }
    }

    private void GetInput (string hAxis, string vAxis)
    {
        // Get the input from the given device axes and normalize it to stop diagonal boosts.
        _Velocity = new Vector3 (Input.GetAxisRaw (hAxis), Input.GetAxisRaw (vAxis), 0.0f);
        _Velocity.Normalize ();

        // Rotate the character toward the right angle.
        if (Input.GetAxisRaw (hAxis) >= 0.00001f)
        {
            _YawAmount += Time.deltaTime * _RotationSpeed;
        }
        // Rotate the character toward the left angle.
        else if (Input.GetAxisRaw (hAxis) <= -0.00001f)
        {
            _YawAmount -= Time.deltaTime * _RotationSpeed;
        }
        else
        {
            // Start rotating back to origin if we're on the right side.
            if (_YawAmount > 0.75f)
                _YawAmount -= Time.deltaTime * _RotationSpeed * 2;
            // Start rotating back to origin if we're on the left side.
            else if (_YawAmount < -0.75f)
                _YawAmount += Time.deltaTime * _RotationSpeed * 2;

            // Reset the rotation whilst no input is active to stop jittering.
            if (_YawAmount <= 0.75f && _YawAmount >= -0.75f)
                _YawAmount = 0.0f;
        }

        // Clamp the characters rotation between the min and max angles.
        _YawAmount = Mathf.Clamp (_YawAmount, _RotationRange.Min, _RotationRange.Max);
    }

    private void GetInputSmoothed (string hAxis, string vAxis)
    {
        // Get the input from the given device axes and normalize it to stop diagonal boosts.
        _Velocity = new Vector3 (Input.GetAxis (hAxis), Input.GetAxis (vAxis), 0.0f);
        _Velocity.Normalize ();

        // Rotate the character toward the right angle.
        if (Input.GetAxis (hAxis) >= 0.00001f)
        {
            _YawAmount += Time.deltaTime * _RotationSpeed;
        }
        // Rotate the character toward the left angle.
        else if (Input.GetAxis (hAxis) <= -0.00001f)
        {
            _YawAmount -= Time.deltaTime * _RotationSpeed;
        }
        else
        {
            // Start rotating back to origin if we're on the right side.
            if (_YawAmount > 0.75f)
                _YawAmount -= Time.deltaTime * _RotationSpeed * 2;
            // Start rotating back to origin if we're on the left side.
            else if (_YawAmount < -0.75f)
                _YawAmount += Time.deltaTime * _RotationSpeed * 2;

            // Reset the rotation whilst no input is active to stop jittering.
            if (_YawAmount <= 0.75f && _YawAmount >= -0.75f)
                _YawAmount = 0.0f;
        }

        // Clamp the characters rotation between the min and max angles.
        _YawAmount = Mathf.Clamp (_YawAmount, _RotationRange.Min, _RotationRange.Max);
    }

    private void FixedUpdate ()
    {
        //Move ();
        //Rotate ();
    }

    private void Move ()
    {
        if (_Velocity == Vector3.zero)
        {
            _Rigidbody.position = _Rigidbody.position;
            return;
        }

        // Calculate the movement of our next position, applying frame smoothing.
        _Velocity = _Rigidbody.position + _Velocity * _Speed * Time.fixedDeltaTime;

        //Calculate the position clamps of our x and y positions.
        float xClamped = Mathf.Clamp (_Velocity.x, _XPositionRange.Min, _XPositionRange.Max);
        float yClamped = Mathf.Clamp (_Velocity.y, _YPositionRange.Min, _YPositionRange.Max);

        //Re-apply the clamped values to our next position.
        _Velocity = new Vector3 (xClamped, yClamped, _Rigidbody.position.z);
        
        //Move to the next position.
        _Rigidbody.MovePosition (_Velocity);
    }

    private void Rotate ()
    {
        _Rigidbody.rotation = Quaternion.Euler (new Vector3 (
                transform.rotation.eulerAngles.x,
                _YawAmount,
                -_YawAmount));
    }
}
