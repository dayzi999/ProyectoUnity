#  Emotional Lighting Plugin for Unity

**Gestión de ambientación emocional en videojuegos utilizando computación emocional (modelo de Plutchik)**  
Este plugin desarrollado en Unity permite modificar dinámicamente la iluminación de una escena de juego en función de emociones humanas. 
A través de una interfaz intuitiva, los diseñadores pueden combinar emociones primarias, crear perfiles emocionales y aplicarlos a objetos del juego para mejorar la inmersión narrativa.

---

## Características

- Iluminación dinámica basada en emociones (modelo de Plutchik)
- Interfaz visual con sliders y rueda emocional interactiva
- Guardado y carga de perfiles emocionales personalizados
- Activación de ambientaciones desde la lógica del juego (triggers, scripts, etc.)
- Aplicación directa de colores emocionales a cualquier objeto `Light`
- Guardado automático del último estado emocional

---

##  Estructura del proyecto

```
ProyectoUnity/
├── Assets/
│   ├── FurnishedCabin/              # Recursos gráficos: prefabs, texturas, escenas
│   │   ├── Animations/
│   │   ├── Documentation/
│   │   ├── Meshes/
│   │   ├── Prefabs/
│   │   ├── Scenes/
│   │   ├── Scripts/
│   │   └── Textures/
│   ├── Resources/                   # Archivos JSON de perfiles emocionales
│   ├── Scenes/                      # Escenas propias del proyecto
│   ├── Scripts/                     # Scripts funcionales del plugin (LightingController, etc.)
│   └── SUPER Character Controller/  # Paquete externo con controladores y assets
│       
├── documentacion/                   # Documentos adicionales o de soporte
```

---

##  Requisitos

- Unity 2022.3.4f1 o superior
- Visual Studio 2022 (opcional)
- Sistema operativo Windows 10 o compatible

---

##  Cómo usar

1. Clona o descarga este repositorio:
```bash
git clone https://github.com/dayzi999/ProyectoUnity
```

2. Abre tu proyecto de Unity y copia la carpeta `/Assets` dentro de tu proyecto.

3. En tu escena:
   - Añade el prefab de la UI a tu Canvas.
   - Agrega el script `LightingController.cs` a un GameObject vacío.
   - Asigna las referencias en el inspector (sliders, marcador, rueda emocional).
   - Agrega el componente `EmotionalLight` a cualquier objeto que contenga una `Light`.

4. Ejecuta y comienza a crear configuraciones emocionales para tus escenas.

---

## Ejemplo de uso in-game

Puedes activar un perfil emocional automáticamente en el juego desde cualquier evento. Ejemplo: al entrar a una zona triste.

```csharp
public class DoorTrigger : MonoBehaviour
{
    public LightingController lightingController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightingController.ActivateProfile("tristeza");
        }
    }
}
```

---

## Perfiles emocionales

Los perfiles se guardan en `emotion_profiles.json` y contienen:
- Intensidades de emociones primarias
- Color generado
- Posición del marcador en la rueda emocional

Estos perfiles se pueden reutilizar, compartir o activar desde scripts.

---

##  Documentación

- [Documento técnico](./documentacion/Documento-Técnico.pdf)  
- [Manual de usuario](./documentacion/MANUAL-DE-USUARIO.pdf)  
- [Capítulo de implementación](./documentacion/Capitulo-Implementación.pdf)

---

## Inspiración académica

Este plugin fue desarrollado como parte del trabajo de grado:

> **Título**: Desarrollo de un plugin para la gestión de ambientación en escenarios de videojuegos utilizando computación emocional  
> **Modelo emocional**: Plutchik (1980)  
> **Motor**: Unity  
> **Enfoque**: Immersive Design + Emotional Computing

---

##  Autor

**Deisy Viviana Ruiz Ochoa**  

[LinkedIn](https://github.com/dayzi999) · [Correo](ruizv2938@gmail.comm) · [Portafolio](https://dayzi999.github.io/Portafolio/)

---

