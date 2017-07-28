using UnityEngine;
using System.Collections;

/// <summary>
/// Moves the object it is attached to between two points
/// </summary>
public class MoveBetweenPoints : MonoBehaviour 
{
    // Egh will redo later N**
    public enum Axis {X, Y, Z, XY, XZ, YZ};
    public Axis axis;

    // Move Type
    public enum MoveType { Point2Point, Initial2Point, DistanceBased };
    public MoveType moveType;

    // Movement variables
    public float distance; // Distance to be moved
    public Vector3 startPoint, endPoint;
    public float speed; // Speed of movement
    private Vector3 sPoint, ePoint;
    private float distanceCovered;
    private float distanceFraction;
    private float startTime;
    //private float sModifier = 1f; // speed modifier
    //private Vector3 prevPos;

	// Initialise
	void Start () 
    {
        Time.timeScale = 0.5f;

        switch(moveType)
        {
            case MoveType.Point2Point: // Get the start and end points from the inspector
                startPoint = sPoint = startPoint;
                endPoint   = ePoint = endPoint;
                break;

            case MoveType.Initial2Point: // Get the start point from the initial position
                startPoint = sPoint = transform.position;
                endPoint   = ePoint = endPoint;
                break;

            case MoveType.DistanceBased: // Calculate the end point using the distance and axis variables
                startPoint = sPoint = transform.position;
                endPoint   = ePoint = transform.position + (Axis2Vector(axis) * distance);
                break;

            default: // Default to Distance based...
                startPoint = sPoint = transform.position;
                endPoint   = ePoint = transform.position + (Axis2Vector(axis) * distance);
                break;
        }

        startTime = Time.time;
	}

    // Update (Will be changed!)
    void Update()
    {
        distanceCovered = (Time.time - startTime) * speed;
        distanceFraction = distanceCovered / distance;


        // Interpolate between the two points
        transform.position = Vector3.Lerp(sPoint, ePoint, distanceFraction);

        // Reverse on End reached:
        if(transform.position == endPoint)
        {
            startTime = Time.time;
            sPoint = endPoint;
            ePoint = startPoint;
        }

        // Reverse on Start reached:
        if(transform.position == startPoint)
        {
            startTime = Time.time;
            sPoint = startPoint;
            ePoint = endPoint;
        }
    }

    /// <summary>
    ///  Convert the Axis enum to a Vector3 Object
    /// </summary>
    /// <returns></returns>
    private Vector3 Axis2Vector(Axis _axis)
    {
        Vector3 axisVec;

        switch(_axis)
        {
            case Axis.X:
                axisVec = new Vector3(1f, 0f, 0f);
                break;
            case Axis.Y:
                axisVec = new Vector3(0f, 1f, 0f);
                break;
            case Axis.Z:
                axisVec = new Vector3(0f, 0f, 1f);
                break;
            case Axis.XY: // Will not be used.
                axisVec = new Vector3(1f, 1f, 0f);
                break;
            case Axis.XZ:
                axisVec = new Vector3(1f, 0f, 1f);
                break;
            case Axis.YZ: // Will not be used.
                axisVec = new Vector3(0f, 1f, 1f);
                break;
            default:    // Default to 'X'
                axisVec = new Vector3(1f, 0f, 0f);
                break;
        }

        axisVec *= Mathf.Sign(distance);

        return axisVec; // Return the vector
    }

	
}
