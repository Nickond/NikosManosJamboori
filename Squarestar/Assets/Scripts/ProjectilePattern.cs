using UnityEngine;
using System.Collections;

public class ProjectilePattern : MonoBehaviour
{

    public Material[] ParticleMaterials;
    public GameObject pPrefab;
    public float RoF = 4; // Rate of fire
    public bool continuous; // Continuous fire flag 
    public float arc; // Arc in degrees
    public int pCount;
    public float pSpeed;
    private float current; // Current time
    private bool onCooldown;

    // Initialise
	void Start () 
    {
        //InvokeRepeating("Fire", 0f, RoF);
	}
	
	// Update
	void Update ()
    {
	    if(continuous)
        {
            if(!onCooldown) // Check the on cooldown flag
            {
                Fire();            // Fire a volley
                onCooldown = true; // set on cooldown
                current    = 0f;   // reset timer
            }

            current += Time.deltaTime; // Add frame time to current time

            if(current >= 1 / RoF)
            {
                // current exceeded the rate of fire, prepare to fire again
                onCooldown = false;
            }

        }
	}

    // Fire projectiles 2 
    public void Fire() 
    {
        float step        = arc / (pCount - 1); // divide the arc into "pCount" segments
        float startAngle  = -(arc * 0.5f);      // Calculate the first angle
        float endAngle    = arc * 0.5f;         // Calculate the last angle
        Vector3 targetDir = Vector3.zero;
        int matSelection  = Random.Range(0, ParticleMaterials.Length);

        for (float i = startAngle; i <= endAngle; i += step )
        {

            //Debug.Log("I " + i);

            targetDir = (Quaternion.Euler(0, i, 0) * transform.forward).normalized;

            //targetDir = targetDir - transform.position;
            // Create the empty object
            GameObject pObj = Instantiate(pPrefab, transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
            // Name the object
            pObj.name = "Projectile";

            // Initialise projectile
            pObj.GetComponent<Projectile>().Initialise(targetDir, pSpeed, 7.5f, ParticleMaterials[matSelection]);
            // Apply mat
            
        }
        
    }
}
