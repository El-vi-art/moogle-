namespace MoogleEngine;
public class DatosArchivo
{
    //Nombre del archivo
    public string Nombre { get; private set; }
    //Ruta al archivo
    public string Ruta { get; private set; }
    //Cantidad de veces que se repite la palabra en el texto del archivo
    float MaximaFrecuencia {get; set;}
    Dictionary<string,int> FrecuenciaPorPalabras { get; set; }
    public DatosArchivo(string Ruta)
    {
        //abrimos el archivo para leerlo
        StreamReader lector = new StreamReader(Ruta);
        this.Ruta = Ruta;
        string[] temp = Ruta.Split('\\');
        //extraemos el nombre del archivo
        Nombre = temp[temp.Length - 1].Substring(0,temp[temp.Length - 1].IndexOf('.'));
        //leemos el archivo y lo pasamos a minusculas
        string content = lector.ReadToEnd().ToLower();
        //eliminamos los caracteres que dificultan la busqueda
        content = Utiles.LimpiarTexto(content);
        //lo separamos por los espacios en blanco
        temp = content.Split(' ');
        MaximaFrecuencia = 0;
        //contamos cuantas veces aparece cada palabra en el texto
        FrecuenciaPorPalabras = new Dictionary<string, int>();
        foreach( var palabra in temp)
        {
            if(!String.IsNullOrWhiteSpace(palabra) && !String.IsNullOrEmpty(palabra))
            {
                if(FrecuenciaPorPalabras.Keys.Contains(palabra))
                    FrecuenciaPorPalabras[palabra]++;
                else
                    FrecuenciaPorPalabras[palabra] = 1;
                if (FrecuenciaPorPalabras[palabra]>MaximaFrecuencia)
                    MaximaFrecuencia = FrecuenciaPorPalabras[palabra];
            }
        }
    }
    //tomamos como valor float la frecuencia de cada palabra
    public float FrecuenciaDe(string palabra)
    {
        if(!FrecuenciaPorPalabras.Keys.Contains(palabra))
            return 0;//si esta no existe no se devuelve ningun valor
        return FrecuenciaPorPalabras[palabra] / MaximaFrecuencia;
    }
    public string Encuentra(string palabra)
    {
        return Utiles.Encuentra(FrecuenciaPorPalabras.Keys.ToArray(),palabra);
    }
    public string[] Palabras
    {
        get
        {
            return FrecuenciaPorPalabras.Keys.ToArray();
        }
    }
    public string FragmentoCon(string[] palabras)
    {
        StreamReader lector = new StreamReader(Ruta);
        string content = lector.ReadToEnd().ToLower();
        int posicion = -1;
        foreach(var palabra in palabras)
        {
            posicion = content.IndexOf(palabra);
            if(posicion > 0)
            {
                int temp = content.IndexOf(' ',Math.Min(posicion + 500,content.Length - posicion)) - posicion;
                if(temp > 0)
                    return content.Substring(posicion,temp);
                return content.Substring(posicion,Math.Min(500,content.Length - posicion));
            }
        }
        return content.Substring(0,content.IndexOf(' ',500));
    }
}