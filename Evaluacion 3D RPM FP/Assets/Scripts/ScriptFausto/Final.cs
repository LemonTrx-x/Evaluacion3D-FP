using UnityEngine;
using UnityEngine.SceneManagement;

public class Final : MonoBehaviour
{
    [Header("Configuración de Interfaz y Escenas")]
    [Tooltip("Arrastra aquí el Panel o Canvas que contiene el mensaje final y el botón")]
    public GameObject panelFinal;
    
    [Tooltip("Escribe exactamente el nombre de la escena de tu menú principal")]
    public string nombreEscenaMenu = "MainMenu";

    void Start()
    {
        // Nos aseguramos de que el panel final esté oculto cuando empiece el nivel
        if (panelFinal != null)
        {
            panelFinal.SetActive(false);
        }
    }

    // Esta función es PÚBLICA para que tu script PlayerInteract pueda llamarla
    public void TerminarJuego()
    {
        if (panelFinal != null)
        {
            // 1. Mostrar la pantalla final (El "Gracias por jugar")
            panelFinal.SetActive(true);
            
            // 2. Pausar el tiempo del juego para que el jugador y el mundo se detengan
            Time.timeScale = 0f;
            
            // 3. Desbloquear y mostrar el cursor del ratón para poder hacer clic en el botón
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogWarning("¡Atención! No has asignado el Panel Final en el inspector del Cangrejo.");
        }
    }

    // Esta función es la que debes ponerle al evento "On Click ()" de tu botón en la UI
    public void IrAlMenuPrincipal()
    {
        // MUY IMPORTANTE: El tiempo debe volver a la normalidad (1) antes de cambiar de escena
        Time.timeScale = 1f; 
        
        // Cargar la escena del menú principal
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}
