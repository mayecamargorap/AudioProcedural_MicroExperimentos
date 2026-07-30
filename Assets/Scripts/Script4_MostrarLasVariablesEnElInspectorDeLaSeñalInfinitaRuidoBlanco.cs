//                         ╔═════════════════════════════════════════════════════════════════╗
//                         ║  Script3_ImprimirEnConsolaPrimerSeñalInfinitaRuidoBlanco        ║                                                               ║
//                         ╚═════════════════════════════════════════════════════════════════╝


//                             ╔═════════════════════════════════════════════════════════╗
//                             ║          Encabezados, librerias y requerimientos        ║                                                               ║
//                             ╚═════════════════════════════════════════════════════════╝

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



//                             ╔═════════════════════════════════════════════════════════╗
//                             ║                           CLASE                         ║
//                             ║     ImprimirEnConsolaPrimerSeñalInfinitaRuidoBlanco     ║                                                               ║
//                             ╚═════════════════════════════════════════════════════════╝

public class Script4_MostrarLasVariablesEnElInspectorDeLaSeñalInfinitaRuidoBlanco: MonoBehaviour  //creo mi clase que se llama como el script 
// que HEREDA de MonoBehaviour, que es la clase base de todos los scripts de Unity.
// los metodos start(), Update(), OnAudioFilterRead(), OnEnable(), OnDisable()...etc
// osea aqui dentro de esta clase podemos escribir y usar todos los metodos de la clase Monobehaviour
{
    
//                                 ╔═════════════════════════════════════════════════╗
//                                 ║                  Objeto Random                  ║                                                               ║
//                                 ╚═════════════════════════════════════════════════╝
    private System.Random ObjetoRandomMio = new System.Random(); // Aqui se crea un OBJETO de la clase Random
    //que se llamará RnadonMio , el new es que asignara memoria dinamica al objeto 
    // y lo creara con el constructor por defecto  System.Random()
    // el cual generará numeros aleatorios.


//                                 ╔═════════════════════════════════════════════════╗
//                                 ║       VARIABLES  (Mostrarlas en inspector)      ║                                                               ║
//                                 ╚═════════════════════════════════════════════════╝

    [Header("Variables del Entorno")] // esta instruccion crea un encabezado o titulo en el inspector de Unity 
    // que dice "Variables del personaje" 
    // sirve mucho para orden 
    //siempre va antes de la variable que queremos mostrar en el inspector de Unity, en este caso volumen mio
    public float MiVariable = 1f; // esta instruccion crea una variable pública de tipo float llamada MiVariable 
    // y le asigna el valor 1f y la muestra en el inspector de Unity.

    [Header("Valores de la señal")] // esta instruccion crea un encabezado o titulo en el inspector de Unity

    public bool VariableReproducirSeñal = true; // variable booleana que nos servirá para reproducir o no la señal,
    // por defecto la inicializamos en true, para que se reproduzca la señal desde el inicio, 
    // pero luego la podemos cambiar a false para que deje de reproducirse la señal.

    public bool VariableYaMostreLasMuestras = false; //OnAudioFilterRead() no se ejecuta una sola vez. 
    // Unity la llama una y otra vez mientras el audio está sonando.
    // Entonces con este bool sé si ya mostre los primeros 10 valores.

    public float VariableUltimoValorGenerado; // Aqui dentro guardare el numero random que se genere y lo  mostrare en el inspector


//                                 ╔═════════════════════════════════════════════════╗
//                                 ║                      Metodos                    ║                                                               ║
//                                 ╚═════════════════════════════════════════════════╝
    private void Update() // metodo que se ejecuta una vez por frame, es decir 60 veces por segundo
    // Cada frame pregunta, si se presiono la tecla espacio para reproducir o no la señal
    {
        if (Input.GetKeyDown(KeyCode.Space)) // si se presiona la tecla espacio, entonces... 
        {
            VariableReproducirSeñal = !VariableReproducirSeñal; // cambiamos el valor de VariableReproducirSeñal a su contrario, 
            // si era true pasa a false, si era false pasa a true
        }
    }


//                                     ╔═════════════════════════════════════════╗
//                                     ║    Metodo que genera la señal INFINITA  ║                                                               ║
//                                     ╚═════════════════════════════════════════╝

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

//                                     ╔═════════════════════════════════════════╗
//                                     ║ Imprimir longitud y canales de la señal ║                                                               ║
//                                     ╚═════════════════════════════════════════╝

        Debug.Log(" Longitud de la señal N="+ VectorDeLaSeñal.Length + "  Canales de la señal=" + VariableCanalesDeLaSeñal); //imprime en consola la longitud de la señal y la cantidad de canales
        // para saber que valores me dio automaticamente Unity.

//                                     ╔═════════════════════════════════════════╗
//                                     ║Si se presiono la tecla espacio: Silencio║                                                               ║
//                                     ╚═════════════════════════════════════════╝

        if (!VariableReproducirSeñal) //si variablereproducirseñal es falso, osea si quiero silencio  
        // porque presioné la tecla espacio, entonces lleno el vector de ceros y retorno
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

//                                     ╔═════════════════════════════════════════╗
//                                     ║    Llenar de nros aleatorios la señal   ║
//                                     ╚═════════════════════════════════════════╝

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


//                                     ╔═════════════════════════════════════════╗
//                                     ║ Imprimir primeros 10 valores de la señal║
//                                     ╚═════════════════════════════════════════╝
            //imprimir en consola
            VariableUltimoValorGenerado = NumeroAleatorio;
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
        }
    }
}

