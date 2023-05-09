namespace MoogleEngine;
public class Motor
{
    CarpetaDeDatos Datos { get; set; }
    float factor_amortiguamiento;
    public Motor(float amortiguamiento)
    {
        Datos = new CarpetaDeDatos(Utiles.Direccion());
        factor_amortiguamiento = amortiguamiento;
    } 
    float[] ModeloVectorial(float[] IDFS, float[,] TFS)
    {
        float[] resultado = new float[TFS.GetLength(0)];
        for(int i = 0; i < TFS.GetLength(0); i++)
        {
            float numerador = 0f;
            float denominador1 = 0f;
            float denominador2 = 0f;
            for(int j = 0; j < TFS.GetLength(1); j++)
            {
                numerador += IDFS[j] * TFS[i,j];
                denominador1 += IDFS[j] * IDFS[j];
                denominador2 += TFS[i,j] * TFS[i,j];
            }
            if(denominador1 * denominador2 == 0)
                resultado[i] = 0f;
            else
                resultado[i] = (numerador * numerador) / (denominador1 * denominador2);
        }
        return resultado;
    }
    float[] Consulta_TF_IDF(string[] consulta)
    {
        Dictionary<string,int> tf = new Dictionary<string, int>();
        foreach(var palabra in consulta)
        {
            if(tf.Keys.Contains(palabra))
                tf[palabra]++;
            else
                tf[palabra] = 1;
        }
        float[] resultado = new float[tf.Keys.ToArray().Length];
        for(int i = 0; i < resultado.Length; i++)
        {
            resultado[i] = (factor_amortiguamiento + (1 - factor_amortiguamiento)*tf[tf.Keys.ToArray()[i]]) * Datos.IDF_De(tf.Keys.ToArray()[i]);
        }
        return resultado;
    }
    public SearchItem[] Busca(string consulta)
    {
        string[] consul = Utiles.ExtraerPalabras(consulta);
        float[] tf_idf_consulta = Consulta_TF_IDF(consul);
        consul = Utiles.PalabrasSinRepetir(consul);
        float[,] tf = Datos.TFS(consul);
        for(int i = 0; i < tf.GetLength(0); i++)
        {
            for(int j = 0; j < tf.GetLength(1); j++)
            {
                tf[i,j] *= Datos.IDF_De(consul[j]);
            }
        }
        float[] similitudes = ModeloVectorial(tf_idf_consulta,tf);
        List<SearchItem> resultados = new List<SearchItem>();
        List<string> relevantes = new List<string>();
        foreach(var palabra in consul)
            if(Datos.IDF_De(palabra) != 0)
                relevantes.Add(palabra);
            consul = relevantes.ToArray();
        for(int i = 0; i < similitudes.Length; i++)
            if(similitudes[i] != 0)
                resultados.Add(new SearchItem(Datos.Archivos[i].Nombre,Datos.Archivos[i].FragmentoCon(consul),similitudes[i]));
        return resultados.ToArray();
    }
    public SearchResult Consulta(string consulta)
    {
        string[] consul = Utiles.ExtraerPalabras(consulta);
        consul = Utiles.PalabrasSinRepetir(consul);
        SearchItem[] resultados = Busca(consulta);
        if((float)resultados.Length / (float)Datos.Archivos.Length < 0.2f)
        {
            foreach(var palabra in consul)
                if(Datos.IDF_De(palabra) == -1f)
                    consulta = consulta.Replace(palabra,Datos.EncuentraSimilar(palabra));
            return new SearchResult(Utiles.OrdenaPorSimilitud(resultados),consulta);
        }
        return new SearchResult(Utiles.OrdenaPorSimilitud(resultados));
    }
    Tuple<float[,],float[]> AplicaOperadores(float[,]tfs,float[]idfs,Dictionary<string,string>operadores,string[] palabras)
    {
        for(int i = 0; i < palabras.Length; i++)
        {
            if(operadores.Keys.Contains(palabras[i]))
            {
                if(operadores[palabras[i]].StartsWith("!"))
                {
                    bool done = false;
                    for(int j = 0; j < tfs.GetLength(0); j++)
                    {
                        if(tfs[j,i] > 0)
                        {
                            done = true;
                            for(int a = 0; a < tfs.GetLength(0); a++)
                                for(int b = 0; b < tfs.GetLength(1); b++)
                                    tfs[a,b] = 0;
                            break;
                        }
                    }
                    if(done)
                        break;
                }
                if(operadores[palabras[i]].StartsWith("^"))
                {
                    bool done = false;
                    for(int j = 0; j < tfs.GetLength(0); j++)
                    {
                        if(tfs[j,i] <= 0)
                        {
                            done = true;
                            for(int a = 0; a < tfs.GetLength(0); a++)
                                for(int b = 0; b < tfs.GetLength(1); b++)
                                    tfs[a,b] = 0;
                            break;
                        }
                    }
                    if(done)
                        break;
                }
                if(operadores[palabras[i]].StartsWith("*"))
                {
                    idfs[i] *= (operadores[palabras[i]].Length + 1);
                    break;
                }
            }
        }
        return new Tuple<float[,], float[]>(tfs,idfs);
    }
}