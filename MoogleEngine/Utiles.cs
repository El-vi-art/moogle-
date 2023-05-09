namespace MoogleEngine;
public static class Utiles
{
    public static Dictionary<string, string> Operadores_Por_Palabras(string[] consulta)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        for (int i = 0; i < consulta.Length; i++)
        {
            if (consulta[i].Contains("*") || consulta[i].Contains("^") || consulta[i].Contains("!") || consulta[i].Contains("~"))
                result[consulta[i + 1]] = consulta[i];
        }
        return result;
    }
    public static string[] ConsultaSinOperadores(string[] consulta)
    {
        char[] operadores = new[] { '!', '~', '*', '^' };
        List<string> result = new List<string>();
        foreach(var palabra in consulta)
        {
            foreach(var op in operadores)
            {
                if(!palabra.Contains(op))
                {
                    result.Add(palabra);
                    break;
                }

            }
        }
        return result.ToArray();
    }
    public static string[] ExtraOperadores(string[] consulta)
    {
        char[] operadores = new[] { '!', '~', '*', '^' };
        List<string> result = new List<string>();
        for (int i = 0; i < consulta.Length; i++)
        {
            foreach (char op in operadores)
                if (consulta[i].StartsWith(op))
                {
                    for (int j = 0; j < consulta[i].Length; j++)
                    {
                        if (!operadores.Contains(consulta[i][j]))
                        {
                            result.Add(consulta[i].Substring(0, j));
                            result.Add(consulta[i].Substring(j));
                        }
                    }
                }
        }
        return result.ToArray();
    }
    public static string LimpiarTexto(string datos)//metodo que elimina los caracteres incomodos al leer los txt
    {
        char[] limpiar = { '\r', '\n', '(', ')', '*', '{', '}', '´', '`', ',', '.', ':', ';' };
        foreach (char c in limpiar)
        {
            datos.Replace(c, ' ');
        }
        return datos;
    }
    public static string Encuentra(string[] datos, string Palabra)
    {
        string resultado = "";
        float max_indice_coincidencias = 0f;
        //buscamos de entre todaas las palabras que existen en los datos la que mayor indice de coincidencias da
        foreach (string palabra in datos)
        {
            //cpmprobamos solo si tienen menos de 4 letras de diferencia
            if (Math.Abs(Palabra.Length - palabra.Length) < 4)
            {
                float coincidencias = MaximoIndiceDeCoincidencias(Palabra, palabra);
                if (coincidencias > max_indice_coincidencias)//si tiene un índice de coincidencia mayor
                {
                    resultado = palabra;
                    max_indice_coincidencias = coincidencias;//actualizamos la palabra a devolver
                }
            }
        }
        return resultado;
    }
    public static float MaximoIndiceDeCoincidencias(string palabra1, string palabra2, bool[] marcadas, int resto, float maximoindice)
    {
        if (resto == 0)
        {
            int contador = 0;//cantidad de coincidencias
            int posicion = 0;
            for (int i = 0; i < palabra2.Length; i++)
            {
                //por cada letra que coincida en posicion con la reordenacion dada de la segunda palabra
                //aumentan las coincidencias
                if (!marcadas[i] && palabra1[i + posicion] == palabra2[i])
                    contador++;
                if (marcadas[i])
                    posicion++;
            }
            //devolvemos el maximo entre el cociente de las coincidencias de ambas palabras y la longitud de la primera;
            //y el maximo indice de coincidencias obtenido anteriormente 
            return Math.Max((float)contador / (float)palabra1.Length, maximoindice);
        }
        //generamos todas las posibles palabras que se forman tachando letras de la segunda palabra
        //hasta que ambas palabras tengan la misma longitud
        for (int i = 0; i < marcadas.Length; i++)
        {
            marcadas[i] = true;
            maximoindice = MaximoIndiceDeCoincidencias(palabra1, palabra2, marcadas, resto - 1, maximoindice);
            marcadas[i] = false;
        }
        return maximoindice;
    }
    public static float MaximoIndiceDeCoincidencias(string palabra1, string palabra2)//metodo q devuelve el indice maximo de coincidencias entre dos palabras
    {
        //primero acomodamos las palabras de manera q la palabra mas larga sea el primer parametro
        string palabramaslarga = "";
        string palabramascorta = "";
        if (palabra1.Length > palabra2.Length)
        {
            palabramaslarga = palabra1;
            palabramascorta = palabra2;
        }
        else
        {
            palabramaslarga = palabra2;
            palabramascorta = palabra1;
        }
        //array que será usado posteriormente
        bool[] marcadas = new bool[palabramaslarga.Length];
        //convertimos ambas palabras a minusculas para facilitar la comparacion
        return MaximoIndiceDeCoincidencias(palabramaslarga.ToLower(), palabramascorta.ToLower(), marcadas, palabramaslarga.Length - palabramascorta.Length, 0f);
    }
    public static string[] ArchivosEnCarpeta(string Ruta)
    {
        return Directory.EnumerateFiles(Ruta).ToArray();
    }
    public static string Direccion()
    {
        //obtenemos la direccion desde donde esta corriendo la aplicacion
        string resultado = Directory.GetCurrentDirectory();
        resultado = resultado.Substring(0, resultado.LastIndexOf('\\') + 1);
        resultado += "Content";
        return resultado;
    }
    public static string[] ExtraerPalabras(string consulta)
    {
        consulta = consulta.ToLower();
        string[] resultado = consulta.Split(' ');
        List<string> final = new List<string>();
        foreach (var palabra in resultado)
            if (!String.IsNullOrEmpty(palabra) && !String.IsNullOrWhiteSpace(palabra))
                final.Add(palabra);
        return final.ToArray();
    }
    public static bool ConsultaValida(string consulta)
    {
        string[] guia = ExtraerPalabras(consulta);
        if (guia[0].StartsWith('~'))
            return false;
        if (guia[guia.Length - 1].EndsWith('!') || guia[guia.Length - 1].EndsWith('^') || guia[guia.Length - 1].EndsWith('*') || guia[guia.Length - 1].EndsWith('~'))
            return false;
        return true;
    }
    public static string[] PalabrasSinRepetir(string[] palabras)
    {
        List<string> resultado = new List<string>();
        foreach (var palabra in palabras)
            if (!resultado.Contains(palabra))
                resultado.Add(palabra);
        return resultado.ToArray();
    }
    public static SearchItem[] OrdenaPorSimilitud(SearchItem[] resultados)
    {
        for (int i = 0; i < resultados.Length; i++)
            for (int j = i; j < resultados.Length; j++)
                if (resultados[j].Score > resultados[i].Score)
                {
                    var temp = resultados[i];
                    resultados[i] = resultados[j];
                    resultados[j] = temp;
                }
        return resultados;
    }
}