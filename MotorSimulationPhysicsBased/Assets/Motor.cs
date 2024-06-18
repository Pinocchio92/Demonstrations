using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{

    [SerializeField]
    Transform rotor;
    [SerializeField]
    Transform ballAnchor;
    [SerializeField]
    Ball ball;
    [SerializeField]
    MotorArm arm;
    //////////////////////
    [SerializeField]
    float maxSpeed; //Rad/S
    [SerializeField]
    float torque = 20; // nm

    Quaternion rotorResetRotation;

    float AngularAcc = 0;
    bool performSimulation = false;
    private void Start()
    {
        rotorResetRotation = rotor.transform.localRotation;
        currentAngle = 0;
        currentAngularVelocity = 0;
    }   
    private float currentAngle;
    private float _currentAngularVelocity =0;
    private float currentAngularVelocity
    {
        get => _currentAngularVelocity;
        set
        {
            if ((value > maxSpeed))
            {
                _currentAngularVelocity = maxSpeed;
            }
            else
                _currentAngularVelocity = value;
        }
    }
    float time = 0f;
    void Update()
    {
        if (performSimulation)
        {
            float time = 0f;
       
            // Calculate angular acceleration
           // float angularAcceleration = torque / (arm.CalculateMOI() + ball.CalculateMOI());

            // Update angular velocity
            currentAngularVelocity += AngularAcc * Time.deltaTime;

            // Update rotation angle using Euler method
           currentAngle +=  currentAngularVelocity * Time.deltaTime;

            // Apply rotation to object
            rotor.transform.localRotation = Quaternion.Euler(0f, currentAngle * Mathf.Rad2Deg, 0f);

            // Increment time
            time += Time.deltaTime;
       
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReleaseBall();
            }
          
        }
    }
    float AngularAcceeration() // Torque/ inirtia of the body attached
    {
       return torque / (arm.CalculateMOI() + ball.CalculateMOI());
    }
    float CalculateTangentialVelocity()
    {
       return currentAngularVelocity * ball.offsetFromPivot;//20/.015mm

    }
    float CalculateReleaseAngle()
    {
        return Vector3.SignedAngle(Vector3.forward, ballAnchor.transform.forward , Vector3.left);//calcute clockwise angle between z forwar and tanget 
    }
    public void PerformSimulation(float _torque , float _maxSpeed)
    {
        torque = _torque;
        maxSpeed = _maxSpeed;
        AngularAcc = AngularAcceeration();
        performSimulation = true;
    }
    public void Reset()
    {
        performSimulation = false;
        currentAngularVelocity = 0;
        currentAngle = 0;
        rotor.transform.localRotation =  rotorResetRotation ;
    }
    public void ReleaseBall()
    {
        CalculateTangentialVelocity();
        ball.ReleaseBall(CalculateTangentialVelocity(), CalculateReleaseAngle());
    }
}
