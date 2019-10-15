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
    [Tooltip ("The minimum angle the character can rotate to.")]
    [SerializeField] private float _RotationRangeMin = -15.0f;
    [Tooltip ("The maximum angle the character can rotate to.")]
    [SerializeField] private float _RotationgRangeMax = 15.0f;
    [Tooltip ("The input device to use for controlling the character.")]
    [SerializeField] private InputType _InputType = InputType.Joystick;

    private float _YawAmount = 0.0f;
    private Vector3 _Velocity = Vector3.zero;
    private Rigidbody _Rigidbody = null;

    private void Awake ()
    {
        SetupRigidbody ();
        LockCursor (true);
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
    }

    private void SelectInput ()
    {
        switch (_InputType)
        {
            case InputType.Mouse:
                GetInput ("Mouse X", "Mouse Y");
                break;
            case InputType.Keyboard:
                GetInput ("Horizontal", "Vertical");
                break;
            case InputType.Joystick:
                GetInputSmoothed ("Horizontal LS", "Vertical LS");
                break;
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
        _YawAmount = Mathf.Clamp (_YawAmount, _RotationRangeMin, _RotationgRangeMax);
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
        _YawAmount = Mathf.Clamp (_YawAmount, _RotationRangeMin, _RotationgRangeMax);
    }

    private void FixedUpdate ()
    {
        Move ();
        Rotate ();
    }

    private void Move ()
    {
        _Rigidbody.MovePosition (_Rigidbody.position + _Velocity * _Speed * Time.deltaTime);
    }

    private void Rotate ()
    {
        _Rigidbody.rotation = Quaternion.Euler (new Vector3 (
                transform.rotation.eulerAngles.x,
                _YawAmount,
                -_YawAmount));
    }
}
