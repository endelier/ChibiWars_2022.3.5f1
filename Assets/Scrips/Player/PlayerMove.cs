using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMove : MonoBehaviour
{
    //Player
    private CharacterController characterController;//controlador de personaje-se referencia en el estar

    [Header("Camera")]
    //Camara
    public Transform cam;

    //Movimiento
    [HideInInspector] public float x;
    [HideInInspector] public float z;

    //Caminar
    [HideInInspector] public Vector3 direction = Vector3.zero;//esto lo usa el codigo Aim
    [HideInInspector] public Vector3 angleRotationShoot = Vector3.zero;
    [HideInInspector] public float speed = 8f;

    //Correr
    [HideInInspector] public float sprintSpeedMultiplier = 2f;
    private float sprintSpeed = 1f;

    //Salto
    private float sphereRadius = 0.35f; //esfera que detecta si esta en el suelo
    [HideInInspector] public float jumpHeight = 7f;
    private int hopCounter = 1;
    private bool isJumping = false;
    private bool isFalling = false;

    //Disparo
    [HideInInspector] public float rotationSpeed = 0.4f;
    private float rotationSpeedShoot = 2f;

    //Gravedad
    [HideInInspector] public float gravity = -45f;
    Vector3 velocity;

    [Header("Extras")]
    //Extras
    public Transform groundCheck;
    public LayerMask groudMask;
    private Animator animator;//Animator de personaje-se referencia en el estar
    bool isGrounded;

    public TargetObject target;

    void Awake()
    {
        cam = FindObjectOfType<UniversalAdditionalCameraData>().transform;
    }

    void Start()
    {

        characterController = GetComponent<CharacterController>();//se referencia el controlador
        animator = GetComponent<Animator>();//Se refencia el animator
    }

    void Update()
    {
        CharacterMove();
        CharacterJump();
        RotationShoot();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            target.SetPriority(0);
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            target.SetPriority(1);
        }
    }

    private void CharacterMove()
    {

        //teclas
        x = Input.GetAxis("Horizontal");//a los lados
        z = Input.GetAxis("Vertical");//al frente

        //obtenniendo angulo hacia donde ve la camara
        Vector3 cameZ = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;//
        Vector3 cameX = new Vector3(cam.transform.right.x, 0, cam.transform.right.z);

        //traspasando el angulo de la camara al jugador
        Vector3 playerFoward = Vector3.Scale(cameZ, new Vector3(1, 0, 1)).normalized;
        Vector3 playerRight = Vector3.Scale(cameX, new Vector3(1, 0, 1)).normalized;

        //vector de la direccion  de movimiento del jugador
        direction = z * playerFoward * speed + x * playerRight * speed;//pasando la informacion de direccion a un vector

        //vector de la rotacion hacia donde mira al disparar
        angleRotationShoot = playerFoward * speed;

        if (direction.magnitude > 0.1f)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        //rota a donde mira Z - //vector que gira, a que direccion gira, en que velocidad, y cual es el maximo
        characterController.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(characterController.transform.forward, direction, rotationSpeed, 0f));//rotando al jugador

        //para sistema de apuntado cambiar el direction en rotar por playerForeward
        if (isGrounded)
        {
            CharacterRun();
        }
        characterController.Move(direction * Time.deltaTime * sprintSpeed);//moviendo al jugador

    }

    private void CharacterRun()
    {

        if (Input.GetKey(KeyCode.LeftShift) && direction.magnitude >= 0.5f)
        {
            animator.SetBool("Run", true);
            sprintSpeed = sprintSpeedMultiplier;
        }
        else
        {
            sprintSpeed = 1;
            animator.SetBool("Run", false);
        }
    }

    private void CharacterJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groudMask);

        //Jump, on Air
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            if (Input.GetKeyUp(KeyCode.Space))
            {
                animator.SetBool("Jump", true);
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                isJumping = true;
            }
            if (isJumping)
            {
                animator.SetBool("onAir", true);
                isFalling = true;
            }
        }

        //double jump
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && hopCounter == 1)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.2f * gravity);
            animator.SetBool("DoubleJump", true);
            hopCounter -= 1;
        }

        //Desactiva saltando
        if (velocity.magnitude < 6f)
        {
            animator.SetBool("Jump", false);
        }

        //Fall false
        if (velocity.magnitude > 15f)
        {
            animator.SetBool("Fall", false);
        }

        //Fall true
        if (velocity.magnitude < 15f && isFalling && isGrounded)
        {
            animator.SetBool("Fall", true);
            animator.SetBool("DoubleJump", false);
            animator.SetBool("onAir", false);
            isFalling = false;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        //Reset double Jump
        if (isGrounded && !isFalling && hopCounter == 0)
        {
            hopCounter += 1;
        }
    }

    private void RotationShoot()
    {

        //Disparar sin mira
        //si aplasta click izquierdo y si magnitud de mover es 0 - rota a donde mira la camara
        if (Input.GetMouseButtonDown(0) && direction.magnitude == 0f && Time.timeScale != 0)
        {
            characterController.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(characterController.transform.forward, angleRotationShoot, rotationSpeedShoot, 0f));//rotando al jugador

        }

        //Poner la mira
        //si aplasta click derecho y si magnitud de mover es 0 - rota a donde mira la camara
        if (Input.GetMouseButton(1) && direction.magnitude == 0f && Time.timeScale != 0)
        {
            characterController.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(characterController.transform.forward, angleRotationShoot, rotationSpeedShoot, 0f));//rotando al jugador

        }
    }
}
