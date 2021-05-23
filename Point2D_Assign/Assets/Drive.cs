using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// A very simplistic car driving on the x-z plane.

public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public Transform fuel;
    

    bool Tflag = false;

    void Start()
    {

    }

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, translation, 0);

        // Rotate around our y-axis
        transform.Rotate(0, 0, -rotation);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CalculateDistance();
            CalculateAngle();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Tflag = !Tflag;
        }

        if (Tflag)
        {
            if (CalculateDistance()>5)
            {
                autoPilot();
            }
            
        }

    }
    void autoPilot()
    {
        float autospeed = 0.07f;
        CalculateAngle();
        this.transform.Translate(this.transform.up * autospeed, Space.World);  
    }
    float CalculateDistance()
    {
        float distancesq = Mathf.Pow(fuel.position.x - transform.position.x, 2) 
            + Mathf.Pow(fuel.position.y - transform.position.y, 2) 
            + Mathf.Pow(fuel.position.z - transform.position.z, 2);
        Debug.Log("Distance:" + Mathf.Sqrt(distancesq));
        Debug.Log("unityDistance:"+Vector3.Distance(fuel.position,transform.position));
        return distancesq;
    }
    void CalculateAngle()
    {
        Vector3 tF = this.transform.up;
        Vector3 fD = fuel.transform.position - this.transform.position;

        float numerator = tF.x * fD.x + tF.y * fD.y + tF.z * fD.z;
        float denominator = tF.magnitude * fD.magnitude;
        float angle = Mathf.Acos(numerator / denominator);

        Debug.Log("Angle: " + angle * Mathf.Rad2Deg);
        Debug.Log("Unity Angle: " + Vector3.Angle(tF, fD));
        Debug.DrawRay(this.transform.position, tF*10, Color.green, 2);
        Debug.DrawRay(this.transform.position, fD, Color.red, 2);
        int clockwise = Cross(tF, fD).z > 0 ? 1 : -1;
        this.transform.Rotate(0, 0, (clockwise * angle * Mathf.Rad2Deg)*0.02f);
        
    }
    Vector3 Cross(Vector3 v, Vector3 w)
    {
        float xMult = v.y * w.z - v.z * w.y;
        float yMult = v.z * w.x - v.x * w.z;
        float zMult = v.x * w.y - v.y - w.x;
        Vector3 crosspod = new Vector3(xMult, yMult, zMult);
        return crosspod;
    }
}