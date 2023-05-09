namespace MoogleEngine;
public class CarpetaDeDatos
{
    //informacion por cada archivo
    public DatosArchivo[] Archivos { get; set; }
    //idf de las palabras
    Dictionary<string,float> IDFS { get; set;}
    public CarpetaDeDatos(string ruta)
    {
        //obtenemos la ruta de cada archivo existente en la ruta dada
        string[] archivos = Utiles.ArchivosEnCarpeta(ruta);
        //instanciamos cada ruta en un archivo
        Archivos = new DatosArchivo[archivos.Length];
        for(int i =0; i < archivos.Length; i++)
            Archivos[i] = new DatosArchivo(archivos[i]);
        //computamos el idf de cada palabra en el conjunto de documentos
        IDFS = new Dictionary<string, float>();
        foreach(var archivo in Archivos)
        {
            foreach(var palabra in archivo.Palabras)
            {
                if(IDFS.Keys.Contains(palabra))
                {
                    if(IDFS[palabra] != Archivos.Length)
                        IDFS[palabra]++;
                    if(IDFS[palabra] != Archivos.Length && (float)IDFS[palabra] / (float)Archivos.Length > 0.7f)
                        IDFS[palabra] = Archivos.Length;
                }
                else
                    IDFS[palabra] = 1;
            }
        }
        //aplicamos la formula del idf
        foreach(var palabra in IDFS.Keys)
            IDFS[palabra] = (float)Math.Log10(Archivos.Length / IDFS[palabra]);
        var a = IDFS["el"];
    }
    public float[,] TFS(string[] Palabras)
    {
        float[,] resultado = new float[Archivos.Length,Palabras.Length];
        for(int i = 0; i < Archivos.Length; i++)
        {
            for(int j = 0; j < Palabras.Length; j++)
            {
                resultado[i,j] = Archivos[i].FrecuenciaDe(Palabras[j]);
            }
        }
        return resultado;
    }
    public float IDF_De(string palabra)
    {
        if(IDFS.Keys.Contains(palabra))
            return IDFS[palabra];
        return -1;
    }
    public string EncuentraSimilar(string palabra)
    {
        string resultado = "";
        float coincidencias = 0f;
        foreach(var archivo in Archivos)
        {
            string temp = archivo.Encuentra(palabra);
            float indice_de_coincidencias = Utiles.MaximoIndiceDeCoincidencias(temp,palabra);
            if(indice_de_coincidencias > coincidencias)
            {
                coincidencias = indice_de_coincidencias;
                resultado = temp;
            }
        }
        return resultado;
    }
}