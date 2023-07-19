Víctor Manuel Castillo Tamayo                         Grupo: C-111.
Proyecto de Programación, primer semestre año 2023.
                                                                        Moogle!
Esta aplicación web Moogle!, desarrollada con .NET core 6.0, en el lenguaje C#, tiene la funcionalidad de buscar una palabra o frase (query) insertada por el usuario, en cierto grupo de documentos .txt, y mostrar el resultado en su interfaz.
Instrucciones para la utilización de Moogle!:
- Abrir una terminal en la carpeta del proyecto.
- En Linux: 
               Correr: “make dev”.
- En Windows:
               Correr: “dotnet watch run --project MoogleServer”.
  
La búsqueda devolverá los documentos donde se encuentre el “query” insertado por el usuario y una porción de este, mostrando donde fue encontrado .
El usuario puede editar o sustituir los documentos contenidos en la carpeta “Content”, siempre y cuando se respete la condición de estar en formato .txt .
