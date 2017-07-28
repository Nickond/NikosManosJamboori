using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
    // Data:
    private Vector3 direction;
    private Vector3 prevPos;
    private bool rotateTowardsMove;
    private float time;
    private float lifetime;
    private float speed;
    private bool initialised;

    public void Initialise(Vector3 _dir, float _speed, float _lifetime = 20f, Material _mat = null)
    {
        direction = _dir;
        speed     = _speed;
        lifetime  = _lifetime;

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

            time += Time.deltaTime; // Advance time

            // Destroy if time is breached
            if (time >= lifetime)
                Destroy(gameObject);
        }
	}
}
