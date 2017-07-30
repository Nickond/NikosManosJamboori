using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    public float speed = 10f;
    public int projectileDamage = 1;
    private float translation;
    private Vector3 direction;
    private Vector3 targetDirection;
    private Vector3 target;
    private Vector3 initPos;
    private PlayerController pc;
    private float timer;
    private bool destroyed = false;
    // Use this for initialization
    void Start()
    {
        translation = speed * Time.deltaTime;
        targetDirection = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (!destroyed)
        {
            //transform.Translate(translation * direction);
            //transform.position = Vector3.MoveTowards(transform.position, Vector3. target, translation);
            transform.position += translation * transform.forward;
            //transform.Translate(translation * transform.forward);

            timer += Time.deltaTime;
        }
        // Self destruct if 10 seconds pass
        if (timer > 100.0f)
            Destroy(gameObject);
    }
}