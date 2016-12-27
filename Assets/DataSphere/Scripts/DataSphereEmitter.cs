using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DataSphereEmitter : MonoBehaviour {

    public ParticleSystem system;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    //for (int i = 0; i < 10; ++i)
	    {
	        CreateDataPoint(Random.value, Random.Range(-1f, 1f));
	    }
	}

    public void CreateDataPoint(float rightAscension, float declination)
    {
        var dataPosition = new Vector3(Mathf.Sin(rightAscension * Mathf.PI) * Mathf.Cos(declination * Mathf.PI), Mathf.Sin(declination * Mathf.PI), Mathf.Cos(rightAscension * Mathf.PI) * Mathf.Cos(declination * Mathf.PI));
        system.Emit(new ParticleSystem.EmitParams() { position = dataPosition, startColor = new Color((1 + dataPosition.x) * 0.5f, (1 + dataPosition.y) * 0.5f, (1 + dataPosition.z) * 0.5f) }, 1);
    }
}
