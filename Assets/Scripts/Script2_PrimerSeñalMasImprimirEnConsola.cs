//using UnityEngine; // Le dice al compilador: Voy a utilizar las herramientas de Unity.

//[RequireComponent(typeof(AudioSource))] //Esto significa: Este script necesita un Audio Source.
// Si el GameObject no lo tiene, Unity lo agrega automáticamente.

//public class Script1_PrimerSonido : MonoBehaviour //Aquí estamos creando nuestro script.
//{
  //  private bool mensajeMostrado = false;

    //private void Start()
//{
    //Debug.Log("El script inició correctamente");
//}

    //private void OnAudioFilterRead(float[] data, int channels) //Esta es la función más importante.
    // No la llamamos nosotros. Unity la llama automáticamente.
    // Cada vez que el dispositivo de sonido necesita más muestras
    // Unity pregunta: "¿Qué números debo enviar ahora al parlante?"
    // Entonces ejecuta esta función.
    //chanel es si es mono=1, stereo=2
    //{
        //if (!mensajeMostrado)
        //{
            //mensajeMostrado = true;
            //Debug.Log("Unity está ejecutando OnAudioFilterRead");
        //}
        
        
        //for (int i = 0; i < data.Length; i++) //Recorremos todas las muestras hasta la N 
        //{
            //data[i] = 0f; // Aquí está ocurriendo todo. Estamos diciendo: 
            // La siguiente muestra vale cero. Después la siguiente también.
            // Después la siguiente. Después la siguiente.
            // Esta variable contiene un arreglo. Pero no cualquier arreglo. 
            // Contiene las muestras que Unity enviará al parlante. Por ejemplo:
            // 0.1, 0.2, 0.3, -0.4, 0.8
            // Cada posición es una muestra.
            // Exactamente como aprendiste en la Clase 2.

            
        //}
    //}
//}







using System; // System es un namespace o grupo que aloja dentro clases, 
// por ejemplo la clase Math, Random, String, Console, DateTime… 
// Esta linea nos permite escribir escribir Math y no System.Math
// aqui lo que le decimos es que vamos usar las herramientas y clases del grupo System
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


public class Script2_PrimerSeñalMasImprimirEnConsola: MonoBehaviour
{
    [Header("Variables del personaje")] // esta instruccion crea un encabezado o titulo en el inspector de Unity 
    // que dice "Variables del personaje" 
    // sirve mucho para orden 
    //siempre va antes de la variable que queremos mostrar en el inspector de Unity, en este caso volumen mio
    public float MiVariable = 1f; // esta instruccion crea una variable pública de tipo float llamada volumenmio 
    // y le asigna el valor 1f y la muestra en el inspector de Unity.

    [Header("Valores de la señal")] // esta instruccion crea un encabezado o titulo en el inspector de Unity
    public float UltimoValorGenerado; // Aqui dentro guardare el numero random que se genere y lo  mostrare en el inspector

    private System.Random RandomMio = new System.Random(); // Aqui se crea un OBJETO de la clase Random
    //que generará numeros aleatorios.

    private bool yaMostreLasMuestras = false; //OnAudioFilterRead() no se ejecuta una sola vez. 
    // Unity la llama una y otra vez mientras el audio está sonando.
    // Entonces con este bool sé si ya mostre los primeros 10 valores.

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i++)
        {
            float valor = (float)(RandomMio.NextDouble() * 0.4 - 0.2);
            data[i] = valor;

            UltimoValorGenerado = valor;   

            if(i>10 && yaMostreLasMuestras==false ) // con este if mostramos los primeros 10 valores en consola
            {
                Debug.Log("Primeras 10 muestras:\n" +
                    data[0] + "\n" +
                    data[1] + "\n" +
                    data[2] + "\n" +
                    data[3] + "\n" +
                    data[4] + "\n" +
                    data[5] + "\n" +
                    data[6] + "\n" +
                    data[7] + "\n" +
                    data[8] + "\n" +
                    data[9] ); 
                yaMostreLasMuestras = true;               
            }    
        }
    yaMostreLasMuestras = true;
    }
}
