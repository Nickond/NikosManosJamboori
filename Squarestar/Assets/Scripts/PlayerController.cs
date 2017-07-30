using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
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
    private Quaternion aimingRotation;
    private Image targetImage;
    private Camera mainCamera;
    public AudioClip shot;
    AudioSource audioSource;
    void Start()
    {
        animator = GetComponent<Animator>();

        GameObject go = GameObject.Find("Canvas");
        if (!go)
            return;

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();

        targetImage = go.GetComponentInChildren<Image>();
    }

    void Update()
    {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        
        // Get the current rotation
        if (Input.GetMouseButtonDown(1))
        {
            aimingRotation = transform.rotation;
        }

        // Aiming, lock rotation
        bool aiming = Input.GetMouseButton(1);
        if (aiming)
        {
            transform.rotation = aimingRotation;
        }
        // Target
        targetImage.enabled = aiming;
        // Position target based on player rotation and position
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position + transform.forward * 4); //targetImage.rectTransform.
        targetImage.rectTransform.position = screenPos;
        Debug.Log(targetImage.rectTransform.offsetMax.y);
        if (inputDir != Vector2.zero && !aiming)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        bool running = !Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        //transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * currentSpeed * Time.deltaTime, Space.World);

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
                // Play shot sound
                AudioSource audio = GetComponent<AudioSource>();
                audioSource.PlayOneShot(shot, 0.7F);
                // Reset time
                totalTime = 0;
            }
        }
    }
}
