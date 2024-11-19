![image](https://github.com/user-attachments/assets/2129fa31-ba0a-4e2e-a9f7-f1ea6e053eb2)# Tarea_9_FDV

#### Tarea: Configurar una escena simple en 3D con un objeto cubo que hará de player y varias esferas de color. Agregar un objeto AudioSource desde el menú GameObject → Audio. Seleccionar un clip de audio en algún paquete de la Asset Store de tu gusto y adjuntarlo a una esfera. El audio se debe reproducir en cuanto se carga la escena y en bucle.

Añadimos el cubo del jugador y las esferas. Añadimos un AudioSource a una de las esferas, marcamos las opciones "Loop" y "Play on Awake", y le añadimos un audio descargado de la Asset Store.

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Screenshots/FDV_9_Screenshot_a.png)

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Screenshots/FDV_9_Screenshot_b.png)



#### Tarea: En la escena anterior crea un objeto con una fuente de audio a la que le configures el efecto Doppler elevado y que se mueva a al pulsar la tecla m a una velocidad alta.

Añadimos una esfera con un script sencillo para moverla en dirección a la cámara.

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Screenshots/FDV_9_Screenshot_c.png)

Cambiamos Spacial Blend a 3D para tener efecto doppler.

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Screenshots/FDV_9_Screenshot_d.png)

Cambiando las opciones tenemos diferentes efectos:

- Incrementar el valor del parámetro Spread.  
Al modificar el Spread, cambia la dirección de la que viene el sonido. Si lo ponemos a 360 se invierte completamente, viniendo de derecha a izquierda en vez de de izquierda a derecha.

- Cambiar la configuración de Min Distance y Max Distance.  
Al aumentar Min Distance, el volumen del sonido aumenta cuando el objeto se acerca. Al disminuirlo, el volumen cuando se acerca también disminuye.
Al aumentar Max Distance, el efecto empieza desde más lejos y se extiende más. Al disminuirlo, el efecto empieza más cerca y se extiende menos.

- Cambiar la curva de Logarithmic Rolloff a Linear Rolloff.  
Con Logarithmic Rolloff, el efecto Doppler es más brusco y realista. Con Linear Rolloff, es uniforme.


#### Tarea: Configurar un mezclador de sonidos, aplica a uno de los grupo un filtro de echo y el resto de filtros libre. Configura cada grupo y masteriza el efecto final de los sonidos que estás mezclando. Explica los cambios que has logrado con tu mezclador.

Añadimos dos grupos. El primero tiene simplemente un filtro de echo.

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Screenshots/FDV_9_Screenshot_e.png)

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Screenshots/FDV_9_Screenshot_f.png)

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Videos/FDV_9_Video_a.mp4)

El segundo tiene tres filtros: un high pass, uno de distorsión y un reverb. Combinados, cambian la música para darle un efecto de baja fidelidad.

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Screenshots/FDV_9_Screenshot_g.png)

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Videos/FDV_9_Video_b.mp4)


## Scripting

#### Tarea: Implementar un script que al pulsar la tecla p accione el movimiento de una esfera en la escena y reproduzca un sonido en bucle hasta que se pulse la tecla s.

Creamos el siguiente script. El booleano m_Play sirve para guardar el estado del Audio Source. Cuando pulsamos p y m_Play es falso, suena el sonido y empieza a moverse. Cuando pulsamos s y m_Play es verdadero, detiene el sonido y el movimiento.

```
public class LoopSound : MonoBehaviour
{
    public float Speed = 50.0f;
    public Vector3 direction = new Vector3 (1.0f, 0.0f, 0.0f);
    [SerializeField] AudioSource m_AudioSource;
    private bool m_Play = false;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.loop = true;
        m_AudioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") && m_Play == false)
        {
            m_AudioSource.Play();
            m_Play = true;
        }
        if (Input.GetKeyDown("s") && m_Play == true)
        {
            m_AudioSource.Pause();
            m_Play = false;
        }
        if (m_Play == true)
        {
            transform.Translate(direction * Speed * Time.deltaTime);
        }
    }
}
```


#### Tarea: Implementar un script en el que el cubo-player al colisionar con las esferas active un sonido. Modificar el script anterior para que según la velocidad a la que se impacte, el cubo lance un sonido más fuerte o más débil.

Creamos el siguiente script. Tenemos un movimiento básico por físicas. En FixedUpdate, antes de la colisión, guardamos la velocidad del jugador. Cuando colisionamos con el obstáculo, reproducimos el sonido de impacto. El volumen es la velocidad del objeto multiplicado por un parámetro de escala. Lo acotamos entre 0 y 1 para evitar clipping del audio. Cambiamos el listener al jugador para escuchar mejor las colisiones.

```
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float Speed = 5.0f, turnSpeed = 30.0f, collisionVolumeScale = 1.0f;
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip impact;
    [SerializeField] private Rigidbody m_Rigidbody;
    private Vector3 m_Input, playerVelocity;
    private float collisionSpeed, collisionVolume;

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        this.transform.LookAt(transform.position + m_Input * turnSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        playerVelocity = m_Input * Speed * Time.fixedDeltaTime;
        m_Rigidbody.MovePosition(transform.position + playerVelocity);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            
            collisionSpeed = playerVelocity.magnitude;            
            collisionVolume = Mathf.Clamp(collisionVolumeScale * collisionSpeed, 0f, 1f);
            m_AudioSource.PlayOneShot(impact, collisionVolume);
        }
    }

}
```


#### Tarea: Agregar un sonido de fondo a la escena que se esté reproduciendo continuamente desde que esta se carga. Usar un mezclador para los sonidos.

Agregamos un AudioSource a la escena, y ponemos un track de música. Ponemos el modo de audio en 2D para que se oiga de forma uniforme en la escena.

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Screenshots/FDV_9_Screenshot_h.png)

Configuramos un Audio Mixer similar al anterior, ajustando el volumen para poder escuchar el resto de efectos.

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Screenshots/FDV_9_Screenshot_i.png)


#### Tarea: Crear un script para simular el sonido que hace el cubo-player cuando está movimiento en contacto con el suelo (mecánica para reproducir sonidos de pasos).

Añadimos un sonido de pasos al AudioSource. Al script anterior le añadimos los booleanos m_IsPlaying y m_OnGround, para controlar cuándo se reproduce el sonido.

En OnCollisionEnter y OnCollisionExit controlamos que el jugador esté en contacto con el suelo.

```
void OnCollisionEnter(Collision collision)
{
    ...
    if(collision.gameObject.CompareTag("Floor"))
    {
        m_OnGround = true;
    }
}

    void OnCollisionExit(Collision collision)
{
    if(collision.gameObject.CompareTag("Floor"))
    {
        m_OnGround = false;
    }
}
```

Por último, en Update reproducimos el sonido cuando el Player se está moviendo en el suelo.

```
void Update()
{
    ...
    // Audio
    if(m_OnGround == true)
    {
        if (m_Input != Vector3.zero && m_IsPlaying == false)
        {
            m_AudioSource.Play();
            m_IsPlaying = true;
        }
        else
        {
            m_AudioSource.Pause();
            m_IsPlaying = false;
        }
    }
}
```

Al estar usando el mismo AudioSource para los pasos y los impactos, hay algo de clipping al reproducir los impactos.

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Videos/FDV_9_Video_c.mp4)


#### Tarea: añadir efectos de sonido a la escena 2D.

Creamos estos tres grupos de AudioMixer:

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Screenshots/FDV_9_Screenshot_j.png)

Añadimos un AudioSource al personaje, al que asignamos el AudioMixer de SFX. En el controlador del personaje, añadimos los siguientes efectos:

Para el salto:

```
    private void Jump()
    {
        rb2D.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
        audioSource.PlayOneShot(audioJump);
        isJumping = true;
    }
```

Para recolección de objetos y recuperar vida:

```
private void OnTriggerEnter2D(Collider2D trigger)
{
    // Collect power ups and increase the score
    if (trigger.gameObject.tag == "PowerUp")
    {
        ...
        audioSource.PlayOneShot(audioPowerUp);
    }

    if (trigger.gameObject.CompareTag("Healing"))
    {
        ...
        audioSource.PlayOneShot(audioHeal);
    }
```

Para perder vida

```
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ...
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ...
            audioSource.PlayOneShot(audioHurt);
        }
    }
```

Para el sonido de ambiente, creamos dos GameObjects vacíos con BoxCollider en modo trigger, que delimitarán las áreas. A cada una le ponemos un AudioSource con un sonido diferente, y a ambas les asignamos el AudioMixer de Ambience.


En el PlayerController, creamos un evento de delegado para controlar este cambio:

```
public delegate void OnAreaChange(string name);
public static event OnAreaChange onAreaChange;

...
private void OnTriggerEnter2D(Collider2D trigger)
{
    ...
    if (trigger.gameObject.layer == LayerMask.NameToLayer("AreaLimits"))
    {
        onAreaChange?.Invoke(trigger.gameObject.name);
    }
}
```

Añadimos el Script AreaBackgroundSound que escuchará el evento y comparará el nombre del objeto para activar o desactivar el AudioSource.

```
public class AreaBackgroundSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerController.onAreaChange += changeBackgroundSound;
    }


    void changeBackgroundSound(string gameObjectName)
    {
        Debug.Log("Activated");
        if ( gameObjectName == this.name)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
```

![](https://github.com/jsfabiani/Tarea_9_FDV/blob/main/Videos/FDV_9_Video_d.mp4)
