//              ╔═════════════════════════════════════════════════════════════════════════════════════╗
//              ║ Script8_CrearSenalSenoYDibujarlaUnaVez                                              ║                                                               ║
//              ╚═════════════════════════════════════════════════════════════════════════════════════╝


    #region 1. Los 2 Hilos del juego explicacion
            
                    // ¿Por qué generamos la señal en un método y la dibujamos en otro?
                    // Unity trabaja con varios hilos, nosotros usaremos dos hilos (Threads) principales para este caso:

                    // 1. Hilo del juego  o "hilo grafico" o "hilo del front" (Main, graphic or front Thread)
                    // Aquí se ejecutan los métodos relacionados con el juego y la parte gráfica, por ejemplo
                    //       Start(), Update(), LateUpdate() 
                    // Desde este hilo podemos:
                    //      Mover GameObjects, Rotar y escalar (modificando su componente Transform.position por ejemplo)
                    //      Dibujar gráficos (LineRenderer).
                    //      Leer entradas del teclado, mouse y controle (Input), 
                    //      Modificar la interfaz de usuario.
                    
                    // 2. Hilo del audio o "hilo de la señal" o "hilo del back" (Audio, signal or back Thread)
                    // Aquí se ejecuta el método: OnAudioFilterRead()
                    // Desde este hilo podemos:
                    //      Generar la señal de audio, Modificar las muestras del arreglo de audio.
                    //      Guardar valores de la señal en variables.
                    //      DESDE Este hilo NO se puede modificar objetos de la escena, como mover GameObjects o dibujar gráficos.
                    
                    // Ambos hilos trabajan al mismo tiempo
                    // El hilo del juego o "hilo del front" y el hilo del audio o "hilo del back" se ejecutan de forma independiente. 
                    // Ninguno espera a que el otro termine. Sin embargo, pueden compartir variables.
                    // Entonces, antes de programar, debemos preguntarnos:
                    // ¿Lo que quiero hacer pertenece al juego o al audio? 
                    // y en base a la respuesta hacerlo donde corresponda

                    // Por ejemplo, en nuestro proyecto:

                    // Desde el hilo del audio o "hilo del back"
                    // Generamos la señal en OnAudioFilterRead(). 
                    // Guardamos el ultimo valor generado en tiempo real en una variable compartida (UltimoValorGenerado)
                    // ese ultimo valor generado puede ser la muestra 5, la 10, la antepenultima, la del medio
                    // de una o de otra señal, pero es la ultima generada en tiempo real.
                    // 
                    // Y Desde del hilo del main o "hilo del front"
                    // Movemos la esfera desde el metodo Update() al ultimo valor generado
                    // Así respetamos la arquitectura de Unity y evitamos errores como:
                    // "SetPosition can only be called from the main thread."


    #endregion

#region Encabezados, librerias y requerimientos
//                  ╔═════════════════════════════════════════════════════════════════════════════════╗
//                  ║         1 Encabezados, librerias y requerimientos                               ║                                                               ║
//                  ╚═════════════════════════════════════════════════════════════════════════════════╝

using System; // System es un namespace o grupo que aloja dentro clases, 
// por ejemplo la clase Math, Random, String, Console, DateTime… 
// Esta linea nos permite escribir escribir Math y no System.Math
// No estamos creando nada. Solo le estamos diciendo al compilador que vamos a utilizar las clases 
// que existen dentro del namespace System.

using UnityEngine; // Vamos a usar Unity, por eso ponemos esta linea. 
// Esta linea nos permite escribir MonoBehaviour y no UnityEngine.MonoBehaviour
// sino agregamos esta linea Unity no sabra que es GameObject, AudioSource,Transform, Vector3
// MonoBehaviour, TimeDebug, Camera
// aqui lo que le decimos es que vamos usar las herramientas del grupo UnityEngine

[RequireComponent(typeof(AudioSource))]  //Esta línea es un atributo o etiqueta, no es una instrucción
// Los atributos de Unity o etiquetas no son los mismos atributos de una clase.
// porque los atributos de clasen son caracteristicas como "color" de la clase pocillo
// mientras que los atributos o etiquetasde Unity son como notas que le damos a Unity para que haga cosas automáticamente.
// y a diferencia de los atributos de clase , los atributos  de Unity o etiquetas van entre [ ]
// En nuestro caso la nota o atributo que Unity lee es: Este script necesita obligatoriamente un componente AudioSource. 
// Si no existe, Unity lo agregará automáticamente
// esta instruccion siempre va antes de la linea "public class Script1_PrimerSeñal : MonoBehaviour" osea 
// antes de la clase.
//si arrastramos este script a un GameObject que no tenga un AudioSource, 
// Unity agregará automáticamente un AudioSource al GameObject, sobre el que arrastremos este codigo 
// ojo porque en el PROYECTO tenemos un LineRenderer, PERO NO esta en el GameObject_GeneradorDeAudioProcedural sino en el 
// GameObject_LineRendererOLapizDeLaSeñal por eso no lo ponemos como requisito aca porque el componente LineRenderer esta en otro objeto
// si lo ponemos aqui como requisito , se creara el componente audiosource y el componente linerenderer en el GameObject_GeneradorDeAudioProcedural
//
#endregion
 

#region 2 Clase
//                  ╔════════════════════════════════════════════════════════════════════════════════╗
//                  ║    2. CLASE                                                                    ║
//                  ║    ImprimirEnConsolaPrimerSeñalInfinitaRuidoBlanco                             ║                                                               ║
//                  ╚════════════════════════════════════════════════════════════════════════════════╝

public class Script8_CrearSenalSenoYDibujarlaUnaVez: MonoBehaviour  //creo mi clase que se llama como el script 
// que HEREDA de MonoBehaviour, que es la clase base de todos los scripts de Unity.
// los metodos start(), Update(), OnAudioFilterRead(), OnEnable(), OnDisable()...etc
// osea aqui dentro de esta clase podemos escribir y usar todos los metodos de la clase Monobehaviour
{
    
    #region 2.1 Crear objeto random
        //               ╔═══════════════════════════════════════════════════════════════════════════╗
        //               ║    2.1. Crear Objeto Random                                               ║                                                               ║
        //               ╚═══════════════════════════════════════════════════════════════════════════╝
        private System.Random ObjetoRandomMio = new System.Random(); // Aqui se crea un OBJETO de la clase Random
        //que se llamará RnadonMio , el new es que asignara memoria dinamica al objeto 
        // y lo creara con el constructor por defecto  System.Random()
        // el cual generará numeros aleatorios.
    #endregion
    
    #region 2.2 Variables
        //                ╔══════════════════════════════════════════════════════════════════════════╗
        //                ║    2.2. VARIABLES  (Mostrarlas en inspector)                             ║                                                               ║
        //                ╚══════════════════════════════════════════════════════════════════════════╝

          #region 2.2.1 Del código

            //                ╔══════════════════════════════════════════════════════════════════════════╗
            //                ║    2.2.1 VARIABLES exclusivas del código                                 ║   
            //                ║          se llaman VariableTal....                                       ║                                                              ║
            //                ╚══════════════════════════════════════════════════════════════════════════╝
            // Estas variables pertenecen al script. No representan ningún objeto de la escena. 
            // Guardan información, parámetros o estados que utiliza el algoritmo.
            // Son variables que pertenecen al algoritmo y almacenan datos, parámetros o estados necesarios para generar y controlar la señal.

            [Header("Variables exclusivas del código")] // esta instruccion crea un encabezado o titulo en el inspector de Unity

            public bool VariableReproducirSeñal = true; // variable booleana que nos servirá para reproducir o no la señal,
            // por defecto la inicializamos en true, para que se reproduzca la señal desde el inicio, 
            // pero luego la podemos cambiar a false para que deje de reproducirse la señal.
            // eso se cambia o actualiza en el update, en base si la tecla "espacio" esta presionada.

            public bool VariableYaMostreLasMuestras = false; //OnAudioFilterRead() no se ejecuta una sola vez. 
            // Unity la llama una y otra vez mientras el audio está sonando.
            // Entonces con este bool sé si ya mostre los primeros 10 valores.
            // y si ya los mostró que no los vuelva a mostrar.
  
            public int VariableCantidadPropuestaDeMuestrasADibujar = 200; // aqui declato y inicializo de una vez la Cantidad de muestras de la señal 
            // que queremos representar gráficamente. "Ejm: Solo voy a dibujar las primeras 200 muestras."

            public int VariableCantidadRealDeMuestrasADibujar=0; 
            // esta variable se creo para que en caso de que nuestra señal tenga menor cantidad de muestras de las que nos proponemos dibujar, 
            // no nos bote error, sino que escoja el menor valor de los dos, en este caso seria que dibuje la señal original.
            // por ejemplo proponemos que dibuje 200 muestras, pero nuestra señal solo tiene 100 muestras
            // entonces la cantidad real que dibujara sera 100
            public bool VariableYaSeDibujoLaSeñal = false; //variable booleana para controlar que la señal se dibuje una sola vez y que no se haga 
            //infinitamente, sino la usamos al momento de dibujar la señal...entonces se dibujaran infinitas señales sobrescribiendose.

                //                ╔══════════════════════════════════════════════════════════════════════════╗
                //                ║    2.2.1.1. VARIABLES exclusivas del código                              ║   
                //                ║             para dibujar señal SENO                                      ║                                                              ║
                //                ╚══════════════════════════════════════════════════════════════════════════╝

            [Header("Variables para señal seno")] // esta instruccion crea un encabezado o titulo en el inspector de Unity
            public float VariableFrecuenciaOCiclosFenHz = 440f;
            public int VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo = 44100;

            
         #endregion

         #region 2.2.1 Referencias desde Hierachy

            //                ╔══════════════════════════════════════════════════════════════════════════╗
            //                ║    2.2.1 VARIABLES desde Hierachy al codigo                              ║     
            //                ║          se llaman VariableReferenciaAlGameobject_Tal....                ║                                                             ║
            //                ╚══════════════════════════════════════════════════════════════════════════╝
            // Estas variables NO crean objetos.
            // Simplemente contienen una referencia a un objeto que ya existe en la escena. 
            
            // Aqui no se crean gameobjects ni linerenderers, 
            // sino que se traen desde la escena del panel de hierachy como REFERENCIAS para poder manipularlos desde el código
            // lo que hagamos a estas referencias desde el código, se vera reflejado en la escena, 
            // porque son referencias a los objetos de la escena
            // Son variables que contienen referencias a objetos que ya existen en el Hierarchy, para poder acceder a ellos 
            // y manipularlos desde el código.

            [Header("Variables traidas desde Hierachy al código")] // esta instruccion crea un encabezado o titulo en el inspector de Unity
            //referencias a GameObjects de la escena, para poder moverlos, escalarlos, rotarlos, etc. desde el código
            // referencias a LineRenderer de la escena, para poder dibujar lineas desde el código

            public GameObject VariableReferenciaAlGameObject_EsferaQueDibujaElUltimoValorGenerado;  // variable tipo gameobject 
            // esta variable es la copia o referencia del Gameobject_esfera del hierachy
            // la copiamos o referenciamos aqui en el codigo
            // para que cualquier cosa que modifiquemos desde codigo, en esta VariableReferencia
            // se modifique en el gameobject del hierachy
            // En este caso la VariableReferenciaAlGameObject, es una esfera que queremos mover desde el código,
            // CUYA FUNCION ES MOVERSE EN PANTALLA en y DE ACUERDO AL PUNTO DE LA SEÑAL QUE REPRESENTE  
            // en este caso la esfera se posiciona en tiempo real en las coordenadas del ultimo valor generado
            // ese valor generado y "alojado en un vector" corresponde al valor de y

            public GameObject VariableReferenciaAlGameObject_EsferaQueDibujaTodaLaSeñal;  // variable tipo gameobject 
            // esta variable es la copia o referencia del Gameobject_esfera del hierachy
            // la copiamos o referenciamos aqui en el codigo
            // para que cualquier cosa que modifiquemos desde codigo, en esta VariableReferencia
            // se modifique en el gameobject del hierachy
            // En este caso la VariableReferenciaAlGameObject, es una esfera que queremos mover desde el código,
            // CUYA FUNCION ES MOVERSE EN PANTALLA (X,Y,Z)DE ACUERDO AL PUNTO DE LA SEÑAL QUE REPRESENTE  
            // en este caso la esfera se posiciona en tiempo real en las coordenadas del ultimo valor generado
            // ese valor generado y "alojado en un vector" corresponde al valor de y
            // y el valor de x es el numero de la muestra 

            public LineRenderer VariableReferenciaAlGameObject_LineRendererOLapizDeLaSeñal; // variable tipo gameobject linerenderer , yo lo llamare "lapiz"
            // esta variable es la copia o referencia del gameobject LineRenderer del Hierachy
            // la copiamos o referenciamos aqui en el codigo 
            // Para que cualquier cosa que modifiquemos desde codigo, en esta variable 
            // se modifique en el gameobject linerendereer del hierachy

            // Un LineRenderer es un gameobject de unity CUYA FUNCION ES DIBUJAR UNA LINEA (LA SEÑAL) uniendo una serie de puntos en el espacio 3d.
            // en este caso la variable linerenderer, es como un lapiz especial que dibuja lineas, 
            // osea dibuja la señal completa de ruido blanco, o seno, o la señal que sea.
            // Tiene muchas propiedades, Por ejemplo: color, grosor, cantidad de puntos, posición de cada punto

            // para dibujar neceesita una lista de posiciones por ejemplo punto0, punto1, punto2...
            // cada punto tiene (x,y,z)



         #region 2.2.1 Puente entre hilos

            //                ╔══════════════════════════════════════════════════════════════════════════╗
            //                ║    2.2.1 VARIABLES de comunicacion o puente entre                        ║            
            //                ║          el hilo main o "hilo de front"                                  ║
            //                ║           y el hilo de audio o "hilo del back"                           ║                                                               ║
            //                ╚══════════════════════════════════════════════════════════════════════════╝
            
            // variables que sirven de puente entre las variables del código y las referencias a los gameobjects de la escena

            public float VariableUltimoValorGenerado; // Aqui dentro guardare el ultimo numero random que se genere en tiempo real 
            // recordemos que se generan muchisimos, aqui no es el ultimo final y ya no se generan mas 
            // sino el ultimo generado en tiempo real, despues siguen generandose mas 
            // y lo  mostrare en el inspector
            //esta variable se llena o se incializa en el metodo OnAudioFilterRead() que corresponde al hilo de audio o "hilo del back" 
            // y se usa en el update() que corresponde al hilo de main o "hilo de front" para mover la esfera a esa altura 
            // y dibujar solo el ultimo valor generado en tiempo real

            private float[] VectorCopiaDeLaSeñal; // Este vector se creo para ahi copiar la señal original y asi usarla para dibujar 
            // Ese VectorCopia será utilizado por Update() para alojar una copia de la señal y dibujar la gráfica.
            // No dibujamos directamente el arreglo VectorDeLaSeñal porque ese vector 
            // pertenece al hilo o thread de audio y desde el hilo de audio no podemos dibujar
            // Entonces lo que hacemos es copiar n muestras del VectorDeLaSeñal  en la copia que es global 
            // y asi ya podremos dibujarlo desde el hilo del main , mas especificamente en el update()
            // Este vector se llena o se inicializa en el metodod OnAudioFilterRead() que corresponde al hilo de audio o "hilo del back"
            // y se usa en el update() que corresponde al hilo de main o "hilo de front" para mover la esfera a esas alturas que tenemos en el vector.
                        
            public bool VariableYaCopieLaPrimeraSeñal = false;
            //Cuado se empieza a generar la copia de la señal , debemos controlar si ya la copio o no
            // si ya se copio se puede dibujar sino no
            // sino se ha copiado continua copiandose 
            // Donde se copia la señal es el metodod OnAudioFilterRead()
            // Esta variable se actuliza, se llena o se inicializa en el metodod OnAudioFilterRead() que corresponde al hilo de audio o "hilo del back"
            // y se usa en el update() que corresponde al hilo de main o "hilo de front" para mover identificar si la copia ya fue hecha y si ya fue hech
            // proceder a dibujar la copia de la señal.
            

         #endregion


         #endregion

    #endregion   

   

    #region 2.3 Métodos de juego
   
        //2.3 Métodos de juego = (main thread) = Hilo del main = Hilo del juego
        //               ╔══════════════════════════════════════════════════════════════════════════╗
        //               ║   2.3  Metodos  del juego (main thread)                                  ║                                                               ║
        //               ╚══════════════════════════════════════════════════════════════════════════╝

        #region 2.3.1. Start 
            //                ╔═════════════════════════════════════════════════════════════════════╗
            //                ║   2.3.1 Start                                                       ║                                                               ║
            //                ╚═════════════════════════════════════════════════════════════════════╝
            private void Start()
            {
                //                 ╔════════════════════════════════════════════════════════════════╗
                //                 ║ 2.3.1.1 Inicializar el tamaño de la variable copia de la señal ║                                                               ║
                //                 ╚════════════════════════════════════════════════════════════════╝
                
                VectorCopiaDeLaSeñal = new float[VariableCantidadPropuestaDeMuestrasADibujar]; 
                // inicializamos el. vector, Con esto reservas memoria para guardar las primeras muestras.

                VariableReferenciaAlGameObject_LineRendererOLapizDeLaSeñal.startWidth = 0.05f  ; //grosor de la linea de dibujo
                VariableReferenciaAlGameObject_LineRendererOLapizDeLaSeñal.endWidth = 0.05f;
            }   
        #endregion

        #region 2.3.2. Update 
            //                 ╔════════════════════════════════════════════════════════════════════╗
            //                 ║   2.3.2. Update                                                    ║                                                               ║
            //                 ╚════════════════════════════════════════════════════════════════════╝
            
            private void Update() // metodo que se ejecuta una vez por frame, es decir 60 veces por segundo
            // Cada frame pregunta, si se presiono la tecla espacio para reproducir o no la señal
            {
                #region 2.3.2.1. ¿Tecla presion?
                    //               ╔══════════════════════════════════════════════════════════════╗
                    //               ║ 2.3.2.1. ¿ La tecla espacio fue presionada ?                 ║
                    //               ╚══════════════════════════════════════════════════════════════╝    
                        
                    if (Input.GetKeyDown(KeyCode.Space)) // si se presiona la tecla espacio, entonces... 
                    {
                        VariableReproducirSeñal = !VariableReproducirSeñal; // cambiamos el valor de VariableReproducirSeñal a su contrario, 
                        // si era true pasa a false, si era false pasa a true
                    }
                        
                #endregion

                #region 2.3.2.2. Dibujar ultimo ∞

                    //               ╔═════════════════════════════════════════════════════════════╗
                    //               ║  2.3.2.2. Dibujar ultimo valor generado                     ║
                    //               ║    No el ultimo de la señal sino                            ║
                    //               ║    el ultimo generado en tiempo real                        ║
                    //               ║  puede ser el 3°, el 5°, el antepenultimo                   ║
                    //               ║    como mi compu tiene 60 FPS,                              ║
                    //               ║ y update() se ejecuta en cada frame                         ║
                    //               ║ entonces update se ejecuta 60 veces por segundo             ║
                    //               ║ osea dibujamos 60 muestras por segundo                      ║ 
                    //               ║                                                             ║    
                    //               ║          ¡¡¡ SI SE HACE AQUI !!!                            ║
                    //               ║    Explicacion al inicio del programa                       ║
                    //               ╚═════════════════════════════════════════════════════════════╝

                    if (VariableReferenciaAlGameObject_EsferaQueDibujaElUltimoValorGenerado != null)
                    // esta variable es nula cuando en el inspector no apunta a ningun objeto osea esta vacia la asignacion
                    // pero si en el inspector 
                    // la VariableGameObject_EsferaQueDibujaElUltimoValorGenerado 
                    // tiene enlazada a 
                    // GameObject_EsferaQueDibujaElUltimoValorGenerado
                    // Entonces ya no es nula , tiene un gameobject "dentro"
                    // y a ese gameobject yo puedo moverlo, escalarlo..etc.
                    {
                        VariableReferenciaAlGameObject_EsferaQueDibujaElUltimoValorGenerado.transform.position = new Vector3
                        // en su posicion le genero un nuevo vector de 3, donde "y" va a ser igual al ultimovalorgenerado
                        // que representa la altura
                        // como esta VariableGameObject
                        (
                            0,
                            VariableUltimoValorGenerado,
                            0
                        );
                    }

               
                #endregion // endregion de Dibujar ultimo valor generado

                #region 2.3.2.3.Dibujar Señal
                     
                    //               ╔══════════════════════════════════════════════════════════════╗
                    //               ║ 2.3.2.3. Dibujar señal completa                              ║
                    //               ╚══════════════════════════════════════════════════════════════╝
                            
                    //               ╔══════════════════════════════════════════════════════════════╗
                    //               ║  Dibujar la linea de la señal                                ║
                    //               ╚══════════════════════════════════════════════════════════════╝  
                    
                    if ((VariableReferenciaAlGameObject_LineRendererOLapizDeLaSeñal != null) 
                    && (VariableYaSeDibujoLaSeñal == false)
                    && (VariableYaCopieLaPrimeraSeñal == true))
                    // si tenemos enlazado un gameobject refernciado desde el inspector y si la no se ha dibujado la señal entonces la dibujamos una vez 
                    // Esto no detiene la generación infinita de ruido blanco., OnAudioFilterRead() puede seguir haciendo:
                    // señal 1 → señal 2 → señal 3 → señal 4 → ...
                    // pero el LineRenderer solamente toma la copia de la primera señal que decidimos dibujar y después deja de actualizarse.
                    {
                        VariableReferenciaAlGameObject_LineRendererOLapizDeLaSeñal.positionCount =VariableCantidadRealDeMuestrasADibujar;
                        // positionCount NO dibuja la línea.
                        // Solo indica cuántos puntos tendrá la linea que creara el linerenderer 
                        // en nuestro caso positionCount= VariableCantidadRealDeMuestrasADibujar
                        // Después SetPosition() asignará la posición de cada uno.                        
                        for (int numerodemuestra = 0; numerodemuestra < VariableCantidadRealDeMuestrasADibujar; numerodemuestra++)
                        {
                            VariableReferenciaAlGameObject_LineRendererOLapizDeLaSeñal.SetPosition //ponga un punto en...
                            ( numerodemuestra, new Vector3  (  numerodemuestra * 0.03f, VectorCopiaDeLaSeñal[numerodemuestra],0  )); // ....las coordenadas de este vector 3
                            // en x pongo el indice numerodemuestra 0,1,,2,3,4,5,...
                            //      Si no multiplicaras por 0.05, los puntos quedarían a una unidad de distancia 
                            //      entre sí y la gráfica sería muy ancha.
                            // en y pongo el valor correspondiente a la posicion [numerodemuestra] en el VectorCopiaDeLaSeñal
                            //      esa seria la altura de la grafica
                            // en z no pongo nada , osea no le doy profundiad

                            // Ejm
                            // si por ejemplo el numerodemuestra es la 3° quedaria
                            // ( 3 * 0.05f, valor de la señal en este punto ejn 0.67, 0)
                            // (    1.5 ,                       0.67                , 0
                            // y asi lo hace con cada una de las muestras de la señal hasta dibujar todos los puntos

                            //               ╔══════════════════════════════════════════════════════════════╗
                            //               ║  Muevo la esfera a lo largo de la señal mientras se dibuja   ║
                            //               ╚══════════════════════════════════════════════════════════════╝ 
                            // Esto se hace para que parezca que la esfera se mueve a lo largo de la señal mientras se dibuja,
                            
                            if (VariableReferenciaAlGameObject_EsferaQueDibujaTodaLaSeñal != null)
                            // esta variable es nula cuando en el inspector no apunta a ningun objeto osea esta vacia la asignacion
                            // pero si en el inspector 
                            // la VariableReferenciaGameObject_EsferaQueDibujaTodaLaSeñal        
                            // tiene enlazada a 
                            // VariableReferenciaGameObject_EsferaQueDibujaTodaLaSeñal    
                            // Entonces ya no es nula , tiene un gameobject "dentro"
                            // y a ese gameobject yo puedo moverlo, escalarlo..etc.
                            {
                                VariableReferenciaAlGameObject_EsferaQueDibujaTodaLaSeñal.transform.position = new Vector3
                                // en su posicion le genero un nuevo vector de 3, donde "y" va a ser igual al ultimovalorgenerado
                                // que representa la altura
                                // como esta VariableGameObject
                                (
                                    numerodemuestra * 0.03f,
                                    VectorCopiaDeLaSeñal[numerodemuestra],
                                    0
                                );
                            }
                        }
                    // Ya terminamos de dibujar la primera señal
                    VariableYaSeDibujoLaSeñal = true;
                    }
                #endregion // endregion de Dibujar señal                    
            }      
        #endregion // endregion del update

    #endregion // endregion de los metodos de juego


    #region 2.4 Métodos de audio
        // 2.4 Métodos de audio = (audio thread)

        //                 ╔═════════════════════════════════════════════════════════════════════════╗
        //                 ║  2.4. Metodos  del audio (audio thread)                                 ║                                                               ║
        //                 ╚═════════════════════════════════════════════════════════════════════════╝

        #region 2.4.1. OnAudioFilterRead 

            //                   ╔═══════════════════════════════════════════════════════════════════╗
            //                   ║ 2.4.1. Metodo que genera la señal INFINITA                        ║                                                               ║
            //                   ╚═══════════════════════════════════════════════════════════════════╝

            private void OnAudioFilterRead(float[] VectorDeLaSeñal, int VariableCanalesDeLaSeñal) 
            // metodo NATIVO de Unity que SE EJECUTA INFINITAMENTE cada vez que Unity necesita un nuevo bloque de audio 
            // GENERADO DESDE CERO
            // float[] VectorDeLaSeñal, int VariableCanalesDeLaSeñal se crean dentro de este metodo, afuera no existen

            // ¿Porque se lee este metodo si nunca lo llamo?
            // porque Unity lee nuestro script en orden y ve que hay un audiosource que es obligatorio 
            // ese audiosource esta con playonawake por defecto siempre
            // entonces 
            // busca automaticamante un metodo que se llame OnAudioFilterRead() 
            // con este metodo se llena el VectorDeLaSeñal con valores de entre -1 y 1 si VariableReproducirSeñal=true
            //  o con ceros osea silencio si VariableReproducirSeñal=false
            // se ejecuta infinitamente
            // es decir genera la señal con  un vector vacio con 2048 muestras, las llena con ceros o valores aleatorios
            // la reproduce y vuelve a llamarlo...y asi..infinitamente 

            // float[] VectorDeLaSeñal es un parametro de entrada del método OnAudioFilterRead, 
            // float[] VectorDeLaSeñal es un arreglo de numeros flotantes, Representa las muestras de audio 
            // que el parlante reproducirá inmediatamente.
            // Es decir --> VectorDeLaSeñal = [ ?, ?, ?, ?, ?, ?, ?, ? ]

            //int VariableCanalesDeLaSeñal, si es 1 es mono, 2 =estereo
            // En audio estéreo, VectorDeLaSeñal no viene separado así:
            // canal izquierdo: [0.2,0.1,-0.2]
            // canal derecho: [0.3,0.4,-0.1]
            // Normalmente viene intercalado: VectorDeLaSeñal= [ L0, R0, L1, R1, L2, R2]
            // Entonces si:  channels = 2 y tienes: data.Length = 6
            // realmente tienes: 3 muestras izquierda, mas 3 muestras derecha
            // porque: 6 valores / 2 canales = 3 muestras por canal

            // si no especifico la longitud de VectorDeLaSeñal, Unity me la asigna automáticamente, ejm 2048
            // si no especifico la cantidad de VariableCanalesDeLaSeñal, Unity me la asigna automáticamente, ejm 2
            // Esos valores se modifican en Edit → Project Settings → Audio, pero dejarlos como Unity los saca
                    
            {

                #region 2.4.1.1. Imprimir longitud 
                    //                ╔═════════════════════════════════════════════════════════════╗
                    //                ║ 2.4.1.1.Imprimir longitud y canales de la señal             ║                                                               ║
                    //                ╚═════════════════════════════════════════════════════════════╝

                    Debug.Log(" Longitud de la señal N="+ VectorDeLaSeñal.Length + "  Canales de la señal=" + VariableCanalesDeLaSeñal); //imprime en consola la longitud de la señal y la cantidad de canales
                    // para saber que valores me dio automaticamente Unity.
                #endregion

                #region 2.4.1.2. Cantidad Real
                // pero se dibuja en update
                    //                ╔═════════════════════════════════════════════════════════════╗
                    //                ║ 2.4.1.2. Calcular CantidadRealDeMuestrasADibujar            ║                                                               ║
                    //                ╚═════════════════════════════════════════════════════════════╝

                    VariableCantidadRealDeMuestrasADibujar = Mathf.Min(VariableCantidadPropuestaDeMuestrasADibujar, VectorDeLaSeñal.Length);
                    // Por precaucion limito que la cantidad de muestras a copiar sea:
                    // -  o el minimo que yo determine en el codigo ejm 200 muestras
                    // -  ó si la señal es menor ejm 100 muestras que dibuje unicamente las 100
                    // eso para prevenir errores futuros.
                    // por eso los parametros son 2, y ya Unity esocgerá el menor de los dos  

                    // Explicacion de que quiere decir math.min osea minimo 200 si la señal es de mas muestras 
                    // o si la señal tiene menos de 200 muestras ejm : 100 
                    // CantidadDeMuestrasACopiar=100
                #endregion  

                #region 2.4.1.3. Llenar ceros 
                    //                ╔═════════════════════════════════════════════════════════════╗
                    //                ║  2.4.1.2. Si se presiono la tecla espacio PARA              ║
                    //                ║              SILENCIAR sonido                               ║ 
                    //                ║              lleno señal de ceros                           ║
                    //                ║              ceros = silencio                                                                                          
                    //                ╚═════════════════════════════════════════════════════════════╝

                    if (!VariableReproducirSeñal) 
                    // La  VariableReproducirSeñal cambia cuando presiono la tecla espacio
                    // y se actualiza o cambia en el update.
                    // //si variablereproducirseñal es falso, osea si no quiero que siga sonando 
                    //sino que quiero silencio  
                     
                    // si presione espacio para silenciar la señal,, entonces lleno el vector de ceros y retorno
                    {
                        for (int muestra = 0; muestra < VectorDeLaSeñal.Length; muestra++)
                        {
                            VectorDeLaSeñal[muestra] = 0f;
                        }

                        return; // no ejecute nada de lo que hay despues  y salga del metodo OnAudioFilterRead, 
                        // porque ya llene el vector con ceros
                        //obviamente unity enseguida volvera a llamar el metodo, porque este metodo se llama infinitamente
                        // generara otro vector vacio
                        // y como VariableReproducirSeñal sigue siendo false, lo llenara de ceros , lo reproducirá (silencio)y retornara otra vez
                        // volvera a llamar el metodo, generara otro vector vacio lo llenara de ceros , lo reproducirá (silencio)y retornara otra vez
                        // y asi infinitamente hasta que presione la tecla espacio otra vez y VariableReproducirSeñal pase a true
                        // entonces volvera a llamar el metodo, generara otro vector vacio lo llenara de numeros aleatorios entre -1 y 1 , lo reproducirá y lo llamara otra vez...
                        // no ejecutara el if sino el for que esta despues 
                    }
                #endregion

                #region 2.4.1.3.Llenar Sonido
                    //                ╔═════════════════════════════════════════════════════════════╗
                    //                ║ 2.4.1.3. Si se presiono la tecla espacio PARA               ║  
                    //                ║           ACTIVAR sonido                                    ║
                    //                ║           lleno señal de numeros aleatorios                 ║
                    //                ║           lleno con numeros                                 ║
                    //                ║           numeros= sonido                                   ║
                    //                ╚═════════════════════════════════════════════════════════════╝

                    // La  VariableReproducirSeñal cambia cuando presiono la tecla espacio, 
                    // si la presione espacio para activar el sonido de la señal
                    // entonces lleno el vector de numeros aleatorios

                    for (int muestra = 0; muestra < VectorDeLaSeñal.Length; muestra++)  // si el valor que Unity nos dio para 
                    // VectorDeLaSeñal.Length es 2048, entonces este for se ejecutará 2048 veces
                    {
                    //float NumeroAleatorio = (float)(ObjetoRandomMio.NextDouble()* 2 - 1); 
                        // Aquí realmente nace la señal en cada repeticion nace una muestra,
                        // Aqui se genera un valor aleatorio entre -1 y 1 ejm: 0.9 el cual se multiplicara por 0.4
                        // 
                        // NextDouble() es un metodo de la clase System.Random
                        // la cual usamos con el objeto ObjetoRandomMio que yo creé
                        // que genera un numero aleatorio entre 0 incluido y 1.0 excluido
                        // pero UNITY necesita que nuestra muestra este entre -1 y 0 y 1
                        
                        //para simular el movimiento o posiciones del parlante
                        // con eso -1 es cerrado  o mínimo (hacia dentro)
                        // 0 es en el centro quieto
                        // y 1 es abierto o máximo (hacia afuera)

                        // Por esto es que le colocamos * 2 - 1
                        // asi si me RnadonMio.NextDouble me da el valor 0, entonces valor = 0*2-1 = -1
                        // asi si me RnadonMio.NextDouble me da el valor 0.5, entonces valor = 0.5*2-1 = 0          
                        // asi si me RnadonMio.NextDouble me da el valor 1, entonces valor = 1*2-1 = 1    

                        // de manera que al final obtendria numeros aleatorios entre -1 y 1

                        //VectorDeLaSeñal[muestra] = NumeroAleatorio;  //   guardo   ese numero aleatorio en la ultima 
                        // posicion libre de la señal
                        // y asi la voy construyendo valor a valor.

                        float NumeroSeno = Mathf.Sin( 2f * Mathf.PI * VariableFrecuenciaOCiclosFenHz * muestra / VariableFrecuenciaDeMuestreoFsenMuestrasPorSegundo );

                        VectorDeLaSeñal[muestra] = NumeroSeno;


                        #region 2.4.1.3.1. Imprimir 10
                            //            ╔═════════════════════════════════════════════════════════╗
                            //            ║ 2.4.1.3.1. Imprimir primeros 10 valores de la señal     ║
                            //            ╚═════════════════════════════════════════════════════════╝
                            //imprimir en consola

                            if(muestra>10 && VariableYaMostreLasMuestras==false ) // con este if mostramos los primeros 10 valores en consola
                            {
                                Debug.Log(" Primeras 10 muestras:\n" +
                                    VectorDeLaSeñal[0].ToString("F2") + ", " + VectorDeLaSeñal[1].ToString("F2") + ", " +
                                    VectorDeLaSeñal[2].ToString("F2") + ", " + VectorDeLaSeñal[3].ToString("F2") + ", " +
                                    VectorDeLaSeñal[4].ToString("F2") + ", " + VectorDeLaSeñal[5].ToString("F2") + ", " +
                                    VectorDeLaSeñal[6].ToString("F2") + ", " + VectorDeLaSeñal[7].ToString("F2") + ", " +
                                    VectorDeLaSeñal[8].ToString("F2") + ", " + VectorDeLaSeñal[9].ToString("F2") ); 
                                    // ToString("F2") es para que solo imprima 2 decimales en consola.
                                VariableYaMostreLasMuestras = true;               
                            } 
                        #endregion  

                        #region 2.4.1.3.2 Guardar Ultimo

                            //            ╔════════════════════════════════════════════════════════╗
                            //            ║  2.4.1.3.2. Guardar ultimo valor generado              ║
                            //            ╚════════════════════════════════════════════════════════╝

                            VariableUltimoValorGenerado = NumeroSeno; 
                            // Esta variable esta dentro de un ciclo for y ese ciclo for dentro de un metodo OnAudioFilterRead
                            // El ciclo for lo que hace es llenar el VectorDeLaSeñal de inicio a fin y salir 
                            // al salir esta variable queda con el ultimo valor generado de esa señal

                            // luego Unity vuelve y llama al metodo OnAudioFilterRead
                            // genera un nuevo vector vacio, lo llena con el for y otra vez esta variable queda con el ultimo valor de la segunda señal

                            // luego Unity vuelve y llama al metodo OnAudioFilterRead
                            // genera un nuevo vector vacio, lo llena con el for y otra vez esta variable queda con el ultimo valor de la tercera señal

                            // es decir esta variablr se sobreescribe cada que se llama al metodo.
                            
                            //                     ╔═══════════════════════════════════════════════╗
                            //                     ║       Dibujar ultimo valor generado           ║
                            //                     ║       No el ultimo de la señal sino           ║
                            //                     ║    el ultimo generado en tiempo real          ║
                            //                     ║puede ser el 3°, el 5°, el antepenultimo       ║  
                            //                     ║   ¡¡¡ NO SE HACE AQUI SINO EN UPDATE()        ║
                            //                     ║       Explicacion al inicio del progr         ║
                            //                     ╚═══════════════════════════════════════════════╝


                        #endregion //endregion guardar ultimo 

                        #region 2.4.1.3.3 Copiar señal
                            //            ╔═════════════════════════════════════════════════════════╗
                            //            ║ 2.4.1.3.3. Copiar señal                                 ║
                            //            ╚═════════════════════════════════════════════════════════╝

                            if (muestra < VariableCantidadRealDeMuestrasADibujar&&VariableYaCopieLaPrimeraSeñal == false) // Copiamos únicamente las primeras muestras.
                            // Esa copia será utilizada posteriormente por Update()
                            // // para dibujar la gráfica en pantalla.

                            {
                            VectorCopiaDeLaSeñal[muestra] = NumeroSeno  ;
                            }

                            // cuando salga de este for cambio el valor de VariableYaCopieLaPrimeraSeñal a true 
                            // porque cuando salga de este for ya habre copiado la señal
                            // y estara lista para ser dibujada en el update...
                            // alla configuro si quiero que se dibuje una vez o infinitamente 
                        #endregion
                   
                    }

                #region 2.4.1.3.3.1 ConfirmoCopia

                    //            ╔═════════════════════════════════════════════════════════╗
                    //            ║ 2.4.1.3.3.1 Confirmo copia                              ║
                    //            ╚═════════════════════════════════════════════════════════╝

                     if (VariableYaCopieLaPrimeraSeñal == false)
                    {
                        VariableYaCopieLaPrimeraSeñal = true;
                    }

                #endregion

                #endregion

            }

        #endregion OnAudioFilterRead 

    #endregion // cierre de metodos de audio 

}
#endregion //cierre de la region de la clase 

#region nombre

#endregion