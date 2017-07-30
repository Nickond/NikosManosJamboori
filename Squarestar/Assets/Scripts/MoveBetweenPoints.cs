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

    public bool pauseAtEnd;     // Pause at checkpoints
    public float pauseSeconds;  // Pause Amount
    private bool paused;        // Paused flag
    private float pausedTime;   // Time of the initial pause

    private Vector3 sPoint, ePoint;
    private float distanceCovered;
    private float distanceFraction;
    private float startTime;

    // Ugh...
    private bool pos = true;
    private float speedMod = 1f;

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
        // distanceCovered = (Time.time - startTime) * speed;
        // distanceFraction = distanceCovered / distance;

        // Paused
        if(paused && pauseAtEnd)
        {
            if(Time.time - pausedTime >= pauseSeconds)
                paused = false;
            else
                return;
        }

        // Interpolate between the two points
        transform.position += (ePoint - sPoint).normalized * speed * Time.deltaTime;
        // transform.position = Vector3.Lerp(sPoint, ePoint, distanceFraction);

        if(ChangeDirection())
        {
            if(pauseAtEnd)
            {
                pausedTime = Time.time;
                paused = true;
            }
        }
    }

    private bool ChangeDirection()
    {
        // Reverse on End reached:
        if (pos)
        {
            // if(transform.position == endPoint)
            if (PointReachedSingleAxisMove(endPoint))
            {
                startTime = Time.time;
                sPoint = endPoint;
                ePoint = startPoint;

                pos = false;

                return true;
            }
        }
        else if (!pos)
        {
            // Reverse on Start reached:
            // if(transform.position == startPoint)
            if (PointReachedSingleAxisMove(startPoint))
            {
                startTime = Time.time;
                sPoint = startPoint;
                ePoint = endPoint;

                pos = true;

                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// The object has reached the specified point.
    /// </summary>
    /// <returns></returns>
    private bool PointReachedSingleAxisMove(Vector3 _point)
    {
        float moveAxis = 0f;
        float pointAxis = 0f;
        // Cancer:
        switch(axis)
        {
            case Axis.X:
                moveAxis  = transform.position.x;
                pointAxis = _point.x;
                break;
            case Axis.Y:
                moveAxis  = transform.position.y;
                pointAxis = _point.y;
                break;
            case Axis.Z:
                moveAxis  = transform.position.z;
                pointAxis = _point.z;
                break;
        }

        // Debug.Log("M: " + moveAxis + " P: " + pointAxis);

        if (Mathf.Abs(moveAxis) >= Mathf.Abs(pointAxis))
        {
            return true;
        }

        return false;
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


        return axisVec; // Return the vector
    }

	
}
