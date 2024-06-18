using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Simulator : MonoBehaviour
{
    [SerializeField]
    Motor motor;
    [SerializeField]
    Ball ball;
    [SerializeField]
    MotorArm arm;
    [SerializeField]
    GameObject SimPanel;
    [SerializeField]
    GameObject VisPanel;

    public TMP_InputField t_Torque;
    public TMP_InputField t_MaxSpeed;
    public TMP_InputField t_ReleaseAngle;
    public TMP_InputField t_MotorElapsedTime;

    ///////TMP_InputField////////////////////////////////////
    public TMP_InputField t_ArmDensity;

    ///////TMP_InputField//////////////////////////////
    public TMP_InputField t_BallDensity;

    ///////Output//////////////////////////////////////
    public TMP_InputField t_DistanceTravel;
    public TMP_InputField t_MotorTimeToMaxSpeed;
    public TMP_InputField t_FlightTime;

    private float Torque;
    private float MaxSpeed;
    private float ReleaseAngle;
    private float MotorElapsedTime;
    //////////////////////////////////
    private float ArmDensity;
    private float BallDensity;




    // Start is called before the first frame update
    void Start()
    {
        SimPanel.SetActive(true);
        VisPanel.SetActive(false);
    }
    public void SimulateValues()
    {
        GetInput();
        ball.InitBallPerameters(BallDensity);
        arm.InitArmPerameters(ArmDensity);
        CalculateSimulation();
    }

    public void PerformSimulationButtonClick()
    {
        GetInput();
        SimPanel.SetActive(false);
        VisPanel.SetActive(true);
        ball.InitBallPerameters(BallDensity);
        arm.InitArmPerameters(ArmDensity);
        motor.PerformSimulation(Torque, MaxSpeed);
    }
    public void LaunchBallButtonClick()
    {
        motor.ReleaseBall();
    }
    public void ResetButtonClick()
    {
        SimPanel.SetActive(true);
        VisPanel.SetActive(false);
        motor.Reset();
        ball.Reset();
    }
    public void  CalculateSimulation()
    {
        float AngularAcc = Torque / (arm.CalculateMOI() + ball.CalculateMOI());
        float currentAngularVelocity = AngularAcc * MotorElapsedTime;

        float ballReleaseAngleRad = DegToRad(ReleaseAngle);
        float launchVelocity = currentAngularVelocity * ball.offsetFromPivot;
        // Using projectile motion equations to calculate distance
        float launchVelocityX = launchVelocity * Mathf.Cos(ballReleaseAngleRad);
        float launchVelocityY = launchVelocity * Mathf.Sin(ballReleaseAngleRad);

        // Time of flight calculation
        float timeOfFlight = (2 * launchVelocityY) / ball.gravity;

        // Horizontal distance calculation
        float travelDistance = launchVelocityX * timeOfFlight;

        float timeToMaxSpeed = ((arm.CalculateMOI() + ball.CalculateMOI()) * MaxSpeed) / Torque;

        //output update
        t_DistanceTravel.text = travelDistance.ToString();
        t_FlightTime.text = timeOfFlight.ToString();
        t_MotorTimeToMaxSpeed.text = timeToMaxSpeed.ToString();
    }
    private float DegToRad(float degrees)
    {
        return degrees * Mathf.PI / 180f;
    }
    void GetInput()
    {
        Torque = float.Parse(t_Torque.text);
        MaxSpeed = float.Parse(t_MaxSpeed.text);
        ReleaseAngle = float.Parse(t_ReleaseAngle.text);
        MotorElapsedTime = float.Parse(t_MotorElapsedTime.text);
        /////////////////
        ArmDensity = float.Parse(t_ArmDensity.text);
        //////////////
        BallDensity = float.Parse(t_BallDensity.text);
    }
}
