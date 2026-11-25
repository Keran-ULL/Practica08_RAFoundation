# Práctica de Unity: Realidad aumentada con AR Foundation y ARCore

En esta práctica se construye una aplicación de realidad aumentada para Android con **AR Foundation** y **ARCore**, siguiendo el Codelab de Google *Create an AR game using Unity's AR Foundation*. La aplicación detecta el suelo con la cámara del móvil y coloca sobre él un coche que se conduce apuntando con el móvil y recoge paquetes. Después se amplía el juego con arañas que restan puntos y un marcador en pantalla.

📦 APKs: [coche.apk](APK/coche.apk) · [arana.apk](APK/arana.apk)

![Prueba de ejecución](Gif/ARGif.gif)

---

## Configuración del proyecto

- **Plantilla y paquetes:** proyecto **Universal 3D (URP)** con los paquetes *AR Foundation* y *Google ARCore XR Plugin*, y el paquete inicial del Codelab (`codelab-starter-package`).
- **XR Plug-in Management:** **ARCore** activado en Android. Los avisos se corrigen con *Project Validation → Fix All*.
- **Player (Android):** solo **OpenGLES3** (sin Vulkan), *Minimum API Level* 24, *Target API Level* 35, IL2CPP, ARM64 y *Active Input Handling* en *Both*, porque los scripts del Codelab leen el táctil con la clase `Input` antigua.
- **URP:** se añade la **AR Background Renderer Feature** a los renderers del proyecto (*Mobile_Renderer* y *PC_Renderer*). Sin ella, la imagen de la cámara se ve en negro.

---

## Ejercicio 1: Aplicación de RA con el Codelab

La escena tiene los componentes básicos de AR Foundation:

| Objeto | Función |
| --- | --- |
| **AR Session** | Controla la sesión de realidad aumentada; sin él no hay seguimiento del entorno |
| **XR Origin (Mobile AR)** | Convierte el seguimiento del móvil al sistema de coordenadas de Unity. Su cámara lleva *AR Camera Manager*, *AR Camera Background* y *Tracked Pose Driver* |
| **AR Plane Manager** | Detecta los planos horizontales y los dibuja con el prefab *Driving Surface Plane* |
| **AR Raycast Manager** | Lanza rayos desde la pantalla contra los planos detectados |
| **Driving Surface Manager** | Bloquea el plano sobre el que se coloca el coche y oculta los demás |

Los managers están en el **XR Origin**, como indica la arquitectura de AR Foundation. Sobre esta base se añaden los elementos del juego:

- **Retícula:** el script `ReticleBehaviour` lanza un raycast desde el centro de la pantalla y coloca la retícula sobre el plano. Se ha corregido el uso de `hits.Length` por `hits.Count`, porque `hits` es una `List`.
- **Coche:** al tocar la pantalla, `CarManager` crea el coche sobre la retícula, y el coche conduce hacia ella.
- **Paquetes:** `PackageSpawner` coloca un paquete al azar en el plano bloqueado cada vez que no hay ninguno.
- **Iluminación:** el script `LightEstimation` ajusta la luz de la escena a la luz real que estima la cámara.

📦 APK: [coche.apk](APK/coche.apk)

---

## Ejercicio 2: Paquetes, arañas y puntuación

El juego se amplía con una mecánica de puntos: cada paquete suma 1 punto y cada araña resta 1. Al tocar cualquiera de los dos, el objeto desaparece y su spawner coloca otro al azar en el plano bloqueado. La puntuación se muestra en pantalla.

- **Tags:** los paquetes tienen el tag **Paquete** y las arañas el tag **Arana**.
- **Araña:** se configura igual que el paquete. Es un prefab con la araña de la Asset Store (*Fuga Spiders*), un **Box Collider en modo trigger** y un **Rigidbody cinemático**, para que el coche la detecte al tocarla. Sus materiales se convierten a URP con el *Render Pipeline Converter*.
- **Spawner de arañas:** `AranaSpawner` funciona como el *Package Spawner*. Cuando hay un plano bloqueado y no existe ninguna araña, la coloca en un punto al azar dentro del contorno del plano, un poco elevada y lejos del coche.
- **Puntuación con eventos:** el script `PuntuacionCoche`, en el coche, detecta en `OnTriggerEnter` si ha tocado un paquete o una araña por su tag. Lanza el evento `OnPuntos` (delegado y evento estático) con +1 o −1 y destruye el objeto. El script `Marcador` está suscrito a ese evento y actualiza el texto de TextMeshPro, que está en un Canvas *Screen Space – Overlay*. Se suscribe en `Start` y se desuscribe en `OnDestroy`, siguiendo el patrón observador.

📄 Scripts: [PuntuacionCoche.cs](Scripts/PuntuacionCoche.cs) · [Marcador.cs](Scripts/Marcador.cs) · [AranaSpawner.cs](Scripts/AranaSpawner.cs)

📦 APK: [arana.apk](APK/arana.apk)

---

## APK

Las APK están compiladas para Android con ARCore (IL2CPP, ARM64, OpenGLES3). Necesitan un móvil compatible con ARCore y los *Servicios de Google Play para RA* instalados. Para probarlas hay que apuntar a un suelo o una mesa con textura y buena luz, mover el móvil despacio hasta que aparezca la superficie y tocar la pantalla para colocar el coche.
