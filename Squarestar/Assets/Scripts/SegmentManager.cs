using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;

public class SegmentManager : MonoBehaviour {

    public Material[] indicatorMaterials;

    public List<Segment> segments = new List<Segment>();

    // X and Y bounds of the grid
    private int xBounds = 8;
    private int yBounds = 4;

	// Use this for initialization
	void Start () 
    {
        segments = GetComponentsInChildren<Segment>().ToList();
	}
	
    // Return the specified segment
    public Segment GetSegment(int x, int y)
    {

        if(x > xBounds || x == 0 || y > yBounds || y == 0)
        {
            Debug.LogWarning("Incorrect bounds requested: X: " + x + " Y: " + y);
            return null;
        }

        // Remove 1 from both coordinates
        x -= 1; 
        y -= 1;

        return segments[(x * yBounds) + y];
    }


    /// <summary>
    /// Returns a Row of segments
    /// </summary>
    /// <param name="_row"></param>
    /// <returns></returns>
    public Segment[] GetRow(int _rowIndex)
    {
        // Check the validity of the requested row
        if(_rowIndex > yBounds)
        {
            Debug.LogWarning("Row: " + _rowIndex + " does not exist!");
            return null;
        }

        Segment[] _row = new Segment[xBounds];

        for (int i = 0; i < xBounds; i++)
        {
            _row[i] = GetSegment(i + 1, _rowIndex);
        }

        return _row;
    }

    /// <summary>
    /// Returns a Column of segments
    /// </summary>
    /// <param name="_row"></param>
    /// <returns></returns>
    public Segment[] GetColumn(int _columnIndex)
    {
        // Check the validity of the requested column
        if (_columnIndex > xBounds)
        {
            Debug.LogWarning("Column: " + _columnIndex + " does not exist!");
            return null;
        }

        Segment[] _column = new Segment[yBounds];

        for (int i = 0; i < yBounds; i++)
        {
            _column[i] = GetSegment(_columnIndex, i + 1);
        }

        return _column;
    }



    int failsafe = 0;

    /// <summary>
    /// Returns a random segment (THIS WILL BE REDONE ONCE THE PLAYER IS IMPLEMENTED)  
    /// </summary>
    /// <returns></returns>
    public Segment GetRandomSegment()
    {
        if(failsafe > 100)
        {
            Debug.LogWarning("Failsafe breached");
            return null;
        }

        Segment seg = GetSegment(Random.Range(1, xBounds + 1), Random.Range(1, yBounds + 1));

        if (seg.Hidden)
        {
            failsafe = 0;
            return seg;
        }
        else
        {
            failsafe++;
            return GetRandomSegment();
        }
    }


    public int testX = 1;
    public int testY = 1;
    public Segment selected = null;

    public void TestSelection()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            //selected = GetRandomSegment();
            //selected = GetSegment(testX, testY);
            GetRandomSegment().Indicate((Elements)Random.Range(0, 3), 5f);
        }
    }

	// Update is called once per frame
	void Update () 
    {
        TestSelection();
	}
}
