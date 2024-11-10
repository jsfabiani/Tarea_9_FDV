# Tarea_9_FDV

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
