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


public class Script1_CrearPrimerSeñalInfinitaRuidoBlanco: MonoBehaviour  //creo mi clase que se llama como el script 
// que HEREDA de MonoBehaviour, que es la clase base de todos los scripts de Unity.
// los metodos start(), Update(), OnAudioFilterRead(), OnEnable(), OnDisable()...etc
// osea aqui dentro de esta clase podemos escribir y usar todos los metodos de la clase Monobehaviour
{
    
    private System.Random RandomMio = new System.Random(); // Aqui se crea un OBJETO de la clase Random
    //que se llamará RnadonMio , el new es que asignara memoria dinamica al objeto 
    // y lo creara con el constructor por defecto  System.Random()
    // el cual generará numeros aleatorios.

    private void OnAudioFilterRead(float[] SeñalEnFormaDeVector, int CanalesDeLaSeñal) 
    // metodo NATIVO de que se ejecuta cada vez que Unity necesita un nuevo bloque de audio

    // ¿Porque se lee este metodo si nunca lo llamo?
    // porque Unity lee nuestro script en orden y ve que hay un audiosource que es obligatorio 
    // ese audiosource esta con playonawake por defecto siempre
    // entonces 
    // busca automaticamante un metodo que se llame OnAudioFilterRead() 
    // con este metodo se llena la SeñalEnFormaDeVector con valores de entre -1 y 1
    // y lo ejecuta cada vez que necesita un nuevo bloque de audio
    // es decir genera las 2048 muestras, y vuelve a llamarlo...y asi..infinitamente 

    // float[] SeñalEnFormaDeVector es un parametro de entrada del método OnAudioFilterRead, 
    // float[] SeñalEnFormaDeVector es un arreglo de numeros flotantes, Representa las muestras de audio que el parlante reproducirá inmediatamente.
    // Es decir --> SeñalEnFormaDeVector = [ ?, ?, ?, ?, ?, ?, ?, ? ]

    //int CanalesDeLaSeñal, si es 1 es mono, 2 =estereo
    // En audio estéreo, SeñalEnFormaDeVector no viene separado así:
    // canal izquierdo: [0.2,0.1,-0.2]
    // canal derecho: [0.3,0.4,-0.1]
    // Normalmente viene intercalado: SeñalEnFormaDeVector= [ L0, R0, L1, R1, L2, R2]
    // Entonces si:  channels = 2 y tienes: data.Length = 6
    // realmente tienes: 3 muestras izquierda, mas 3 muestras derecha
    // porque: 6 valores / 2 canales = 3 muestras por canal

    // si no especifico la longitud de SeñalEnFormaDeVector, Unity me la asigna automáticamente, ejm 2048
    // si no especifico la cantidad de CanalesDeLaSeñal, Unity me la asigna automáticamente, ejm 2
    // Esos valores se modifican en Edit → Project Settings → Audio, pero dejarlos como Unity los saca
            
    {
        Debug.Log("Longitud de la señal N="+ SeñalEnFormaDeVector.Length + "  Canales de la señal=" + CanalesDeLaSeñal); //imprime en consola la longitud de la señal y la cantidad de canales
        // para saber que valores me dio automaticamente Unity.

        for (int i = 0; i < SeñalEnFormaDeVector.Length; i++)  // si el valor que Unity nos dio para 
        // SeñalEnFormaDeVector.Length es 2048, entonces este for se ejecutará 2048 veces
        {
            float NumeroAleatorio = (float)(RandomMio.NextDouble()* 2 - 1); //Aquí realmente nace la señal en cada repeticion nace una muestra,aqui se genera un valor aleatorio entre -1 y 1 ejm: 0.9 el cual se multiplicara por 0.4
            // RandomMio.NextDouble() es un metodo de la clase System.Random, la cual usamos con el objeto RandonMio
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

            SeñalEnFormaDeVector[i] = NumeroAleatorio;  //   luego guardo   ese numero aleatorio en la ultima posicion de la señal
            // y al voy construyendo valor a valor.
        }
    }
}
