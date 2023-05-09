namespace MoogleEngine;


public static class Moogle
{
    public static Motor motor;
    public static bool initied = false;
    public static void Init()
    {  
        motor = new Motor(0.5f);
        initied = true;
    }
    public static SearchResult Query(string query) 
    {
        return motor.Consulta(query);
    }  
}
