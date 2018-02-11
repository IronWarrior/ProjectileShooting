using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CannonInterface : MonoBehaviour 
{
    [SerializeField]
    Cursor targetCursor;

    [SerializeField]
    CannonController cannon;

    [SerializeField]
    Text timeOfFlightText;

    [SerializeField]
    float defaultFireSpeed = 35;

    [SerializeField]
    float defaultFireAngle = 45;

    private float initialFireAngle;
    private float initialFireSpeed;
    private bool useLowAngle;

    private bool useInitialAngle;

    void Awake()
    {
        useLowAngle = true;

        initialFireAngle = defaultFireAngle;
        initialFireSpeed = defaultFireSpeed;

        useInitialAngle = true;
    }

    void Update()
    {
        if (useInitialAngle)
            cannon.SetTargetWithAngle(targetCursor.transform.position, initialFireAngle);
        else
            cannon.SetTargetWithSpeed(targetCursor.transform.position, initialFireSpeed, useLowAngle);

        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
            cannon.Fire();
        }

        timeOfFlightText.text = Mathf.Clamp(cannon.lastShotTimeOfFlight - (Time.time - cannon.lastShotTime), 0, float.MaxValue).ToString("F3");
    }

    public void SetInitialFireAngle(string angle)
    {
        initialFireAngle = Convert.ToSingle(angle);
    }

    public void SetInitialFireSpeed(string speed)
    {
        initialFireSpeed = Convert.ToSingle(speed);
    }

    public void SetLowAngle(bool useLowAngle)
    {
        this.useLowAngle = useLowAngle;
    }

    public void UseInitialAngle(bool value)
    {
        useInitialAngle = value;
    }
}
