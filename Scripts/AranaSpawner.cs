using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;

public class AranaSpawner : MonoBehaviour
{
    public DrivingSurfaceManager drivingSurfaceManager;   // el del Codelab
    public GameObject aranaPrefab;                        // prefab con tag "Arana"
    public float distanciaMinimaAlCoche = 0.3f;           // para que no aparezca encima del coche
    public float alturaExtra = 0.02f;                     // la sube un poco para que no quede bajo el plano

    private GameObject arana;

    void Update()
    {
        if (drivingSurfaceManager == null || aranaPrefab == null) return;

        ARPlane plano = drivingSurfaceManager.LockedPlane;
        if (plano != null && arana == null)
        {
            Spawn(plano);
        }
    }

    private void Spawn(ARPlane plano)
    {
        CarBehaviour coche = FindFirstObjectByType<CarBehaviour>();

        Vector3 posicion = PuntoAleatorio(plano);
        for (int i = 0; i < 10 && coche != null; i++)
        {
            if (Vector3.Distance(posicion, coche.transform.position) >= distanciaMinimaAlCoche) break;
            posicion = PuntoAleatorio(plano);
        }

        posicion += plano.transform.up * alturaExtra;

        Quaternion giro = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        arana = Instantiate(aranaPrefab, posicion, giro);
    }

    // Punto al azar dentro del contorno del plano
    private Vector3 PuntoAleatorio(ARPlane plano)
    {
        Vector2 centro = plano.centerInPlaneSpace;
        Vector2 ext = plano.extents;
        NativeArray<Vector2> borde = plano.boundary;

        for (int i = 0; i < 20; i++)
        {
            Vector2 p = centro + new Vector2(Random.Range(-ext.x, ext.x), Random.Range(-ext.y, ext.y));
            if (DentroDelPoligono(p, borde))
                return plano.transform.TransformPoint(new Vector3(p.x, 0f, p.y));
        }
        return plano.center;
    }

    private bool DentroDelPoligono(Vector2 p, NativeArray<Vector2> poligono)
    {
        bool dentro = false;
        for (int i = 0, j = poligono.Length - 1; i < poligono.Length; j = i++)
        {
            Vector2 a = poligono[i];
            Vector2 b = poligono[j];
            if ((a.y > p.y) != (b.y > p.y) &&
                p.x < (b.x - a.x) * (p.y - a.y) / (b.y - a.y) + a.x)
                dentro = !dentro;
        }
        return dentro;
    }
}
