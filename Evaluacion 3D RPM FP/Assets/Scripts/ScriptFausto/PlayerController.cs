using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 5f;
    public float sensibilidadRaton = 100f;
    public float gravedad = -9.81f; // Fuerza de gravedad estándar

    private CharacterController controller;
    private Vector3 velocidadCaida; // Guarda la velocidad a la que cae

    [Header("Animación")]
    private Animator animador;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animador = GetComponent<Animator>();
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
    }
}