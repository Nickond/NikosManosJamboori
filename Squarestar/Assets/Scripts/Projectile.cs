using UnityEngine;
using System.Collections;
using Mechanics;

public class Projectile : MonoBehaviour 
{
    // Data:
    private Vector3 direction;
    private Vector3 prevPos;
    private bool rotateTowardsMove;
    private float time;
    private float lifetime;
    private float speed;
    private float trueSpeed;
    private bool initialised;
    private Boost boost;
    private float boostDecay = 0.01f;

    public void Initialise(Vector3 _dir, float _speed, Boost _boost = null, float _lifetime = 20f, Material _mat = null)
    {
        direction = _dir;
        speed     = trueSpeed = _speed;
        lifetime  = _lifetime;
        boost     = _boost;

        if (boost != null)
            speed *= boost.Amount; // Double the speed

        if (_mat != null)
            GetComponent<MeshRenderer>().material = new Material(_mat);

        if (GetComponent<Rotate>() == null)
            rotateTowardsMove = true;

        initialised = true;
    }

	// Update
	void Update ()
    {
        if (initialised)
        {
            // Move
            transform.position += direction * speed * Time.deltaTime;

            // Rotations
            if (rotateTowardsMove)
                transform.rotation = Quaternion.LookRotation(transform.position - prevPos);

            prevPos = transform.position; // Store previous position

            // Boost:
            if(boost != null)
            {
                speed -= boost.DecayRate;
                if(speed <= trueSpeed)
                {
                    speed = trueSpeed;
                    boost = null;
                }
            }

            time += Time.deltaTime; // Advance time

            // Destroy if time is breached
            if (time >= lifetime)
                Destroy(gameObject);
        }
	}
}
