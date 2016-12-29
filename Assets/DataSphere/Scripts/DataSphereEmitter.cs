using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class DataSphereEmitter : MonoBehaviour {

    public ParticleSystem system;

    public void DataReceived(DataPointViewModel dataPoint)
    {
        CreateDataPoint(dataPoint.rightAscension, dataPoint.declination, dataPoint.color);
    }

    private void CreateDataPoint(float rightAscension, float declination, Color color)
    {
        var dataPosition = new Vector3(Mathf.Sin(rightAscension * Mathf.PI) * Mathf.Cos(declination * Mathf.PI), Mathf.Sin(declination * Mathf.PI), Mathf.Cos(rightAscension * Mathf.PI) * Mathf.Cos(declination * Mathf.PI));
        system.Emit(new ParticleSystem.EmitParams() { position = dataPosition, startColor = color }, 1);
    }
}
