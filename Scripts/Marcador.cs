using UnityEngine;
using TMPro;

public class Marcador : MonoBehaviour
{
    public TextMeshProUGUI texto;
    private int puntuacion = 0;

    void Start()
    {
        PuntuacionCoche.OnPuntos += SumarPuntos;
        ActualizarTexto();
    }

    void OnDestroy()
    {
        PuntuacionCoche.OnPuntos -= SumarPuntos;
    }

    private void SumarPuntos(int puntos)
    {
        puntuacion += puntos;
        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        if (texto != null)
            texto.text = "Puntos: " + puntuacion;
    }
}
