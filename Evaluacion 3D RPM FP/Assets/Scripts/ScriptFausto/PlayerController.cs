using System;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 5f;
    public float sensibilidadRaton = 100f;
    public float gravedad = -9.81f; // Fuerza de gravedad estándar
    public float jumpHeight = 2f;


    private CharacterController controller;
    private Vector3 velocidadCaida; // Guarda la velocidad a la que cae
    private bool isGrounded;


    [Header("Animación")]
    private Animator animador;

    [Header("Shoot")]
    public Transform spawn;
    public GameObject bullet;
    public float shootForce = 1500f;
    public float shootRate = 0.5f;
    float shootRateTime = 0f;
    public bool shootActive = false;
    public GameObject gunHide;
    public GameObject gunText;
    public ScoreManager scoreManager;
    public GameObject lastCrab;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animador = GetComponent<Animator>();
        shootActive = false;
        gunHide.SetActive(false);
        gunText.SetActive(false);
    }

    void Update()
    {
        // 1. Rotación del cuerpo (Izquierda/Derecha)
        float ratonX = Input.GetAxis("Mouse X") * sensibilidadRaton * Time.deltaTime;
        transform.Rotate(Vector3.up * ratonX);

        // 2. Movimiento Horizontal (WASD)
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        // Calculamos la dirección y le decimos al CharacterController que se mueva
        Vector3 direccionMover = transform.right * inputX + transform.forward * inputZ;
        
        // La función .Move() es mágica: choca contra paredes sin temblar
        controller.Move(direccionMover.normalized * velocidad * Time.deltaTime);

        // 3. Aplicar Gravedad
        // Si tocamos el suelo, reseteamos la velocidad de caída para que no se acumule
        if (controller.isGrounded && velocidadCaida.y < 0)
        {
            velocidadCaida.y = -2f; // Un valor pequeño para mantenerlo pegado al piso
        }

        // Aplicamos la gravedad matemática y movemos el controlador hacia abajo
        velocidadCaida.y += gravedad * Time.deltaTime;
        controller.Move(velocidadCaida * Time.deltaTime);

        // 4. Control de Animación
        if (Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputZ) > 0.1f)
        {
            if (animador != null) animador.SetBool("seMueve", true);
        }
        else
        {
            if (animador != null) animador.SetBool("seMueve", false);
        }


        ScoreGoal();



        //SALTO
        // Comprobar si está en el suelo
        isGrounded = controller.isGrounded;

        if (isGrounded && velocidadCaida.y < 0)
        {
            velocidadCaida.y = -2f; // Mantener pegado al suelo
        }

        

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocidadCaida.y = Mathf.Sqrt(jumpHeight * -2f * gravedad);
        }

        // Aplicar gravedad
        velocidadCaida.y += gravedad * Time.deltaTime;
        controller.Move(velocidadCaida * Time.deltaTime);

        Shoot();
    }

    void Shoot()
    {
        if (shootActive == true && Input.GetMouseButtonDown(0) && Time.time > shootRateTime)
        {
            GameObject newBullet;
            
            newBullet = Instantiate(bullet, spawn.position, spawn.rotation);

            newBullet.GetComponent<Rigidbody>().AddForce(spawn.forward * shootForce);

            shootRateTime = Time.time;

            Destroy(newBullet, 5);
        }   
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ActiveGun"))
        {
            shootActive = true;
            gunHide.SetActive(true);
            gunText.SetActive(true);
        }

        if (other.CompareTag("NoActiveGun"))
        {
            shootActive = false;
            gunHide.SetActive(false);
            gunText.SetActive(false);
        }
    }

    void ScoreGoal()
    {
        if (scoreManager.score == 10)
        {
            lastCrab.SetActive(true);
        }

        else
        {
            lastCrab.SetActive(false);
        }
    }
}