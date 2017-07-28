using UnityEngine;
using System.Collections;

public class Bolt : MonoBehaviour
{
    private Vector3 target; // Target of the bolt (a segment)
    private Vector3 start;
    private Vector3 currentPos;
    private Vector3 centre;
    private Vector3 startCentre;
    private Vector3 endCentre;
    private Vector3 prevPos;

    public GameObject boltObject;
    public ParticleSystem segmentImpact;

    private float startTime;
    private float currentTime;
    private float duration = 3.5f;
    private float arcHeight = 5f;
    private bool primed = false;
    private bool collided = false;

    // Use this for initialization
	void Start ()
    {  
        // REEEEEEEEEEEEEEEEE
	}
	
    /// <summary>
    /// Initialise the bolt
    /// </summary>
    /// <param name="_target"></param>
    public void Initialise(Vector3 _target)
    {
        start       = transform.position;
        currentPos  = start;
        startTime   = Time.time;
        target      = _target; // Set the target
        centre      = (transform.position + _target) * 0.5f; // Calculate the centre
        startCentre = transform.position - centre;
        endCentre   = target - centre;
        arcHeight   = Random.Range(2.5f, 5f); // Randomize arc height

        primed = true;
    }

	// Update is called once per frame
	void Update () 
    {
        if (primed && !collided)
        {
            //transform.position += transform.forward * 2 * Time.deltaTime;
            //transform.position = Vector3.Slerp(startCentre, endCentre, (Time.time - startTime) / 5f);

            // Current time
            currentTime = (Time.time - startTime) / duration;

            // Lerp the current position
            currentPos = Vector3.Lerp(start, target, currentTime);

            // Apply arc
            currentPos.y += arcHeight * Mathf.Sin(Mathf.Clamp01(currentTime) * Mathf.PI);

            // Apply the new position to the transform
            transform.position = currentPos;

            // Rotate the bolt towards movement
            boltObject.transform.parent.rotation = Quaternion.LookRotation(transform.position - prevPos);

            // Store previous position
            prevPos = transform.position;
        }
	}
    
    /// <summary>
    /// Collisions
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        if(!collided)
        {
            // Check for collision with a segment
            if(col.gameObject.layer == LayerMask.NameToLayer("Segment"))
            {
                collided = true;     // collision flag
                //boltObject.GetComponent<MeshRenderer>().enabled = false;
                Destroy(boltObject); // Destroy the bolt object
                
                // Snap to position
                transform.position = new Vector3(col.gameObject.transform.position.x,
                                                 transform.position.y,
                                                 col.gameObject.transform.position.z - 0.25f);
                // Play explosion
                segmentImpact.Play();
            }
        }
    }


    // Setters / Getters
    #region properties
    public float Duration
    {
        get { return duration; }
    }
    #endregion
}
