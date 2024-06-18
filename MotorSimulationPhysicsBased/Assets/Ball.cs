using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    float density = 2.70f;//  g/cm³;
    [SerializeField]
    float radius = 7.5f;// mm;
    [SerializeField]
    public float offsetFromPivot;
    [SerializeField]
    public float gravity = 9.8f;
    public float mass { get; private set; }

    float releaseAngle;
    float initialVelocity;
    Vector3 initialPosition = Vector3.zero;
    bool isReleased = false;
    private float releaseTime;

    Vector3 ballResetPosition;
    Quaternion resetRotation;
    Transform resetParent;
    Vector3 resetScale;
    Quaternion CameraresetRotation;
    private void Start()
    {
        ballResetPosition = transform.localPosition;
        resetRotation = transform.localRotation;
        resetParent = transform.parent;
        resetScale = transform.localScale;
        CameraresetRotation = Camera.main.transform.localRotation;
    }
    public float CalculateMOI() // mass * length from the pivot of the arm 
    {
        return CalculateMass() * Mathf.Pow(offsetFromPivot, 2);
    }
    float CalculateMass() // Volume * Density 
    {
        //convert Density to from g/cm`3
        return GetVolume() * GetDensityinStdUnit();
    }
    float GetVolume() // 4/3 Pi r`3
    {
        return Mathf.PI * Mathf.Pow(radius, 3) * (4/3);
    }
    public void ReleaseBall(float _initialVelocity , float _releaseAngle)
    {
        Debug.Log("velocity " + _initialVelocity);
        transform.SetParent(null);
        initialVelocity = 3.2f;// _initialVelocity;
        releaseAngle = _releaseAngle;
        initialPosition = transform.localPosition;
        releaseTime = Time.time;
        isReleased = true;
    }
    float GetDensityinStdUnit()
    {
        return density * 1000f;
    }
    private void Update()
    {
        if (isReleased)
        {
            transform.localPosition = GetPosition(Time.time - releaseTime);
            //Camera.main.transform.LookAt(transform);
        }
    }
    public Vector3 GetPosition(float time)
    {
        // Convert release angle to radians
        float angleInRadians = releaseAngle * Mathf.Deg2Rad;

        // Calculate horizontal and vertical components of initial velocity
        float v0x = initialVelocity * Mathf.Cos(angleInRadians);
        float v0y = initialVelocity * Mathf.Sin(angleInRadians);
        Debug.Log("V0x " + v0x);
        Debug.Log("V0y " + v0y);
        // Calculate position at time t
        float x = initialPosition.z + v0x * time;
        float y = initialPosition.y + v0y * time - 0.5f * gravity * Mathf.Pow(time, 2);

        // Return the calculated position
        return new Vector3(initialPosition.x, y, x);
    }

    public void InitBallPerameters(float _density)
    {
        density = _density;
    }
    public void Reset()
    {
        transform.SetParent(resetParent);
        transform.localPosition =ballResetPosition ;
        transform.localRotation = resetRotation;
        transform.localScale = resetScale;
        releaseAngle = 0;
        initialVelocity = 0;
        initialPosition = Vector3.zero;
        isReleased = false;
        releaseTime = 0;
        Camera.main.transform.localRotation = CameraresetRotation;
}

   
}
