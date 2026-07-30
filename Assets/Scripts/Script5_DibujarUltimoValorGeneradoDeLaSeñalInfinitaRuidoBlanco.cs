//              ╔═════════════════════════════════════════════════════════════════════════════════════╗
//              ║ Script5_DibujarUltimoValorGeneradoDeLaSeñalInfinitaRuidoBlanco                      ║                                                               ║
//              ╚═════════════════════════════════════════════════════════════════════════════════════╝


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
// Unity agregará automáticamente un AudioSource al GameObject.
#endregion
 

#region 2 Clase
//                  ╔════════════════════════════════════════════════════════════════════════════════╗
//                  ║    2. CLASE                                                                    ║
//                  ║    ImprimirEnConsolaPrimerSeñalInfinitaRuidoBlanco                             ║                                                               ║
//                  ╚════════════════════════════════════════════════════════════════════════════════╝

public class Script5_DibujarUltimoValorGeneradoDeLaSeñalInfinitaRuidoBlanco: MonoBehaviour  //creo mi clase que se llama como el script 
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

        [Header("Variables de prueba")] // esta instruccion crea un encabezado o titulo en el inspector de Unity 
        // que dice "Variables del personaje" 
        // sirve mucho para orden 
        //siempre va antes de la variable que queremos mostrar en el inspector de Unity, en este caso volumen mio
        public float MiVariable = 1f; // esta instruccion crea una variable pública de tipo float llamada MiVariable 
        // y le asigna el valor 1f y la muestra en el inspector de Unity.

        [Header("Variables exclusivas del código")] // esta instruccion crea un encabezado o titulo en el inspector de Unity

        public bool VariableReproducirSeñal = true; // variable booleana que nos servirá para reproducir o no la señal,
        // por defecto la inicializamos en true, para que se reproduzca la señal desde el inicio, 
        // pero luego la podemos cambiar a false para que deje de reproducirse la señal.

        public bool VariableYaMostreLasMuestras = false; //OnAudioFilterRead() no se ejecuta una sola vez. 
        // Unity la llama una y otra vez mientras el audio está sonando.
        // Entonces con este bool sé si ya mostre los primeros 10 valores.

        public float VariableUltimoValorGenerado; // Aqui dentro guardare el numero random que se genere y lo  mostrare en el inspector
        
            //                ╔══════════════════════════════════════════════════════════════════════════╗
            //                ║    2.2.1 VARIABLES desde Hierachy al codigo                              ║                                                               ║
            //                ╚══════════════════════════════════════════════════════════════════════════╝
                       
            [Header("Variables traidas desde Hierachy al código")]
            //referencias a GameObjects de la escena, para poder moverlos, escalarlos, rotarlos, etc. desde el código
            // referencias a LineRenderer de la escena, para poder dibujar lineas desde el código

            // Aqui no se crean gameobjects ni linerenderers, 
            // sino que se traen desde la escena hierachy como REFERENCIAS para poder manipularlos desde el código
            // lo que hagamos a estas referencias desde el código, se vera reflejado en la escena, 
            // porque son referencias a los objetos de la escena
            public GameObject VariableReferenciaGameObject_EsferaQueDibujaElUltimoValorGenerado;  // variable tipo gameobject 
            // esta variable es la copia o referencia del Gameobject_esfera del hierachy
            // la copiamos o referenciamos aqui en el codigo
            // para que cualquier cosa que modifiquemos desde codigo, en esta variable
            // se modifique en el gameobject del hierachy
            //En este caso la variableGameObject, es una esfera que queremos mover desde el código, 
            // en este caso la esfera que dibuja el ultimo valor generado

    #endregion

    #region 2.3 Métodos de juego 
   
        //2.3 Métodos de juego = (main thread)
        //               ╔══════════════════════════════════════════════════════════════════════════╗
        //               ║   2.3  Metodos  del juego (main thread)                                  ║                                                               ║
        //               ╚══════════════════════════════════════════════════════════════════════════╝

        #region 2.3.1. Start 
            //                ╔═════════════════════════════════════════════════════════════════════╗
            //                ║   2.3.1 Start                                                       ║                                                               ║
            //                ╚═════════════════════════════════════════════════════════════════════╝
            private void Start()
            {
                //VariableMiLineRenderer.positionCount = ConstanteCantidadDeMuestrasADibujar;
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

                #region 2.3.2.2. Dibujar ultimo 

                    //               ╔═════════════════════════════════════════════════════════════╗
                    //               ║  2.3.2.2. Dibujar ultimo valor generado                     ║
                    //               ║    No el ultimo de la señal sino                            ║
                    //               ║    el ultimo generado en tiempo real                        ║
                    //               ║    puede ser el 3°, el 5°, el antepenultimo                 ║    
                    //               ║          ¡¡¡ SI SE HACE AQUI !!!                            ║
                    //               ║    Explicacion                                              ║
                    //               ╚═════════════════════════════════════════════════════════════╝


                    // ¿Por qué generamos la señal en un método y la dibujamos en otro?
                    // Unity trabaja con dos hilos (Threads) principales para este caso:

                    // 1. Hilo del juego (Main Thread)
                    // Aquí se ejecutan los métodos relacionados con el juego y la parte gráfica, por ejemplo
                    //       Start(), Update(), LateUpdate() 
                    // Desde este hilo podemos:
                    //      Mover GameObjects, Cambiar posiciones (Transform), Dibujar gráficos (LineRenderer).
                    //      Leer entradas del teclado (Input), Modificar la interfaz de usuario.
                    

                    // 2. Hilo del audio (Audio Thread)
                    // Aquí se ejecuta el método: OnAudioFilterRead()
                    // Desde este hilo podemos:
                    //      Generar la señal de audio, Modificar las muestras del arreglo de audio.
                    //      Guardar valores de la señal en variables.
                    //      DESDE Este hilo NO se puede modificar objetos de la escena, como mover GameObjects o dibujar gráficos.
                    
                    // Ambos hilos trabajan al mismo tiempo
                    // El hilo del juego y el hilo del audio se ejecutan de forma independiente. 
                    // Ninguno espera a que el otro termine. Sin embargo, pueden compartir variables.
                    // Entonces, antes de programar, debemos preguntarnos:
                    // ¿Lo que quiero hacer pertenece al juego o al audio? 
                    // y en base a la respuesta hacerlo donde corresponda

                    // Por ejemplo, en nuestro proyecto:
                    // Generamos la señal en OnAudioFilterRead().
                    // Guardamos el ultimo valor generado en una variable compartida (UltimoValorGenerado)
                    // ese ultimo valor generado puede ser la muestra 5, la 10, la antepenultima, la del medio
                    // de una o de otra señal, pero es la ultima generada en tiempo real.
                    // Dibujamos ese ultimo valor generado moviendo la esfera en Update().
                    // Así respetamos la arquitectura de Unity y evitamos errores como:
                    // "SetPosition can only be called from the main thread."

                    if (VariableReferenciaGameObject_EsferaQueDibujaElUltimoValorGenerado != null)
                    // esta variable es nula cuando en el inspector no apunta a ningun objeto osea esta vacia la asignacion
                    // pero si en el inspector 
                    // la VariableGameObject_EsferaQueDibujaElUltimoValorGenerado 
                    // tiene enlazada a 
                    // GameObject_EsferaQueDibujaElUltimoValorGenerado
                    // Entonces ya no es nula , tiene un gameobject "dentro"
                    // y a ese gameobject yo puedo moverlo, escalarlo..etc.
                    {
                        VariableReferenciaGameObject_EsferaQueDibujaElUltimoValorGenerado.transform.position = new Vector3
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
            // metodo NATIVO de que SE EJECUTA INFINITAMENTE cada vez que Unity necesita un nuevo bloque de audio 
            // GENERADO DESDE CERO

            // ¿Porque se lee este metodo si nunca lo llamo?
            // porque Unity lee nuestro script en orden y ve que hay un audiosource que es obligatorio 
            // ese audiosource esta con playonawake por defecto siempre
            // entonces 
            // busca automaticamante un metodo que se llame OnAudioFilterRead() 
            // con este metodo se llena la VectorDeLaSeñal con valores de entre -1 y 1 si VariableReproducirSeñal=true
            //  o con ceros osea silencio si VariableReproducirSeñal=false
            // se ejecuta infinitamente
            // es decir genera la señal con  un vector vacio con 2048 muestras, las llena con ceros o valores aleatorios
            // la reproduce y vuelve a llamarlo...y asi..infinitamente 

            // float[] VectorDeLaSeñal es un parametro de entrada del método OnAudioFilterRead, 
            // float[] VectorDeLaSeñal es un arreglo de numeros flotantes, Representa las muestras de audio que el parlante reproducirá inmediatamente.
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

                #region 2.4.1.2. Silencio = ceros 
                    //                ╔═════════════════════════════════════════════════════════════╗
                    //                ║  2.4.1.2. Si se presiono la tecla espacio PARA              ║
                    //                ║              SILENCIAR sonido                               ║ 
                    //                ║              lleno señal de ceros                           ║                                                             
                    //                ╚═════════════════════════════════════════════════════════════╝

                    if (!VariableReproducirSeñal) //si variablereproducirseñal es falso, osea si no quiero que se diga sonando 
                    //sino que quiero silencio  
                    // La  VariableReproducirSeñal cambia cuando presiono la tecla espacio, 
                    // si la presione para silencia señal,, entonces lleno el vector de ceros y retorno
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

                #region 2.4.1.3.Sonido = numeros 
                    //                ╔═════════════════════════════════════════════════════════════╗
                    //                ║ 2.4.1.3. Si se presiono la tecla espacio PARA               ║  
                    //                ║           ACTIVAR sonido                                    ║
                    //                ║           lleno señal de numeros aleatorios                 ║
                    //                ╚═════════════════════════════════════════════════════════════╝

                    // La  VariableReproducirSeñal cambia cuando presiono la tecla espacio, 
                    // si la presione para activar el sonido señal,, entonces lleno el vector de numeros aleatorios  y retorno

                    for (int muestra = 0; muestra < VectorDeLaSeñal.Length; muestra++)  // si el valor que Unity nos dio para 
                    // VectorDeLaSeñal.Length es 2048, entonces este for se ejecutará 2048 veces
                    {
                        float NumeroAleatorio = (float)(ObjetoRandomMio.NextDouble()* 2 - 1); //Aquí realmente nace la señal en cada repeticion nace una muestra,aqui se genera un valor aleatorio entre -1 y 1 ejm: 0.9 el cual se multiplicara por 0.4
                        // ObjetoRandomMio.NextDouble() es un metodo de la clase System.Random, la cual usamos con el objeto ObjetoRandomMio
                        // que genera un numero aleatorio entre 0 incluido y 1.0 excluido
                        // pero nosotros UNITY necesita que nuestra muestra este entre -1 y 0 y 1
                        
                        //para simular el movimiento o posiciones del parlante
                        // con eso -1 es cerrado  o mínimo (hacia dentro)
                        // 0 es en el centro quieto
                        // y 1 es abierto o máximo (hacia afuera)

                        // Por esto es que le colocamos * 2 - 1
                        // asi si me RnadonMio.NextDouble me da el valor 0, entonces valor = 0*2-1 = -1
                        // asi si me RnadonMio.NextDouble me da el valor 0.5, entonces valor = 0.5*2-1 = 0          
                        // asi si me RnadonMio.NextDouble me da el valor 1, entonces valor = 1*2-1 = 1    

                        // de manera que al final obtendria numeros aleatorios entre -1 y 1

                        VectorDeLaSeñal[muestra] = NumeroAleatorio;  //   luego guardo   ese numero aleatorio en la ultima posicion de la señal
                        // y al voy construyendo valor a valor.


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

                        #region 2.4.1.3.2. Guardar Ultimo

                            //            ╔════════════════════════════════════════════════════════╗
                            //            ║  2.4.1.3.2. Guardar ultimo valor generado              ║
                            //            ╚════════════════════════════════════════════════════════╝

                            VariableUltimoValorGenerado = NumeroAleatorio; 
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
                            //                     ║               Explicacion                     ║
                            //                     ╚═══════════════════════════════════════════════╝


                            // ¿Por qué generamos la señal en un método y la dibujamos en otro?
                            // Unity trabaja con dos hilos (Threads) principales para este caso:

                            // 1. Hilo del juego (Main Thread)
                            // Aquí se ejecutan los métodos relacionados con el juego y la parte gráfica, por ejemplo
                            //       Start(), Update(), LateUpdate() 
                            // Desde este hilo podemos:
                            //      Mover GameObjects, Cambiar posiciones (Transform), Dibujar gráficos (LineRenderer).
                            //      Leer entradas del teclado (Input), Modificar la interfaz de usuario.
                            

                            // 2. Hilo del audio (Audio Thread)
                            // Aquí se ejecuta el método: OnAudioFilterRead()
                            // Desde este hilo podemos:
                            //      Generar la señal de audio, Modificar las muestras del arreglo de audio.
                            //      Guardar valores de la señal en variables.
                            //      DESDE Este hilo NO se puede modificar objetos de la escena, como mover GameObjects o dibujar gráficos.
                            
                            // Ambos hilos trabajan al mismo tiempo
                            // El hilo del juego y el hilo del audio se ejecutan de forma independiente. 
                            // Ninguno espera a que el otro termine. Sin embargo, pueden compartir variables.
                            // Entonces, antes de programar, debemos preguntarnos:
                            // ¿Lo que quiero hacer pertenece al juego o al audio? 
                            // y en base a la respuesta hacerlo donde corresponda

                            // Por ejemplo, en nuestro proyecto:
                            // Generamos la señal en OnAudioFilterRead().
                            // Guardamos el ultimo valor generado en una variable compartida (UltimoValorGenerado)
                            // ese ultimo valor generado puede ser la muestra 5, la 10, la antepenultima, la del medio
                            // de una o de otra señal, pero es la ultima generada en tiempo real.
                            // Dibujamos ese ultimo valor generado moviendo la esfera en Update().
                            // Así respetamos la arquitectura de Unity y evitamos errores como:
                            // "SetPosition can only be called from the main thread."
                        #endregion //endregion guardar ultimo 

                    }
                #endregion

            }

        #endregion OnAudioFilterRead 

    #endregion // cierre de metodos de audio 
}
#endregion //cierre de la region de la clase 

#region nombre

#endregion