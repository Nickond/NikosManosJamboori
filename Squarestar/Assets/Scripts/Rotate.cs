using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{

    public float amount;

    public bool x = true;
    public bool y = true;
    public bool z = true;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        float rot = amount * Time.deltaTime;
        transform.Rotate(new Vector3(rot * BoolToInt(x), 
                                     rot * BoolToInt(y), 
                                     rot * BoolToInt(z)));
	}

    private int BoolToInt(bool b)
    {
        int i = 0;

        if (b)
            i = 1;

        return i;
    }
}
