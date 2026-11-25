using UnityEngine;

public class PuntuacionCoche : MonoBehaviour
{
    public delegate void PuntosHandler(int puntos);
    public static event PuntosHandler OnPuntos;

    private void OnTriggerEnter(Collider other)
    {
        Transform t = other.transform;
        while (t != null && !t.CompareTag("Paquete") && !t.CompareTag("Arana"))
            t = t.parent;

        if (t == null) return;   

        if (t.CompareTag("Paquete"))
        {
            OnPuntos?.Invoke(1);
        }
        else
        {
            OnPuntos?.Invoke(-1);
        }

        Destroy(t.gameObject);   
    }
}
