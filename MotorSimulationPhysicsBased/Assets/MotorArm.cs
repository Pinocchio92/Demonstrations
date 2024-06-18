using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorArm : MonoBehaviour
{
    [SerializeField]
    float density = 2.70f;//  g/cm³;
    [SerializeField]
    float lengthOfCylinder = 200;//  mm;
    [SerializeField]
    float radiusOfCylinder = 7.5f;// mm;

    [SerializeField]
    float initialAngle = 0f;//  g/cm³;
    [SerializeField]
    float initialVelocity = 0f;//  g/cm³;

    public float CalculateMOI() // 1/3* mass * Length`2 
    {
        return (1f / 3f) * CalculateMass() * Mathf.Pow(lengthOfCylinder, 2);
    }
    float CalculateMass() // Volume * Density 
    {
        return GetVolume() * GetDensityinStdUnit(); ;
    }
    float GetVolume() // Pi r`2 H 
    {
        return Mathf.PI * Mathf.Pow(radiusOfCylinder, 2) * lengthOfCylinder;
    }
    float GetDensityinStdUnit()
    {
        return density * 1000f;
    }
    public void InitArmPerameters( float _density)
    {
        density = _density;
    }
}
