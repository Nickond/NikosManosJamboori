using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float walkSpeed = 2;
    public float runSpeed = 6;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    public GameObject playerProjectile;
    public Transform projectileStartPoint;
    public float projectileSpeed;
    Animator animator;
    private float totalTime = 0;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        bool running = !Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        float animationSpeedPercent = ((running) ? 1 : .5f) * inputDir.magnitude;
        animator.SetFloat("speed", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        bool shooting = Input.GetMouseButton(0);
        animator.SetBool("shoot", shooting);
        if (shooting)
        {
            // Cooldown
            totalTime += Time.deltaTime;
            //Debug.Log(totalTime);
            // Magic number
            if (totalTime >= 0.35714285714285714285714285714286)
            {
                // Spawn bullets
                GameObject newProjectile = Instantiate(playerProjectile, projectileStartPoint.position, transform.rotation) as GameObject;
                // Reset time
                totalTime = 0;
            }
        }

    }
}
