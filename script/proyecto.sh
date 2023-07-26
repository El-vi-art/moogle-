#!/usr/bin/bash

# Run: Ejecuta y compila el proyecto Moogle!
Run()
{
    gnome-terminal --quiet -- dotnet watch run --project MoogleServer
}

# Report: compilar y generar el pdf del proyecto latex relativo al informe
Report()
{
    pdflatex -output-directory=informe informe/Proyecto Moogle!.tex
    echo "Archivo latex referente al informe, compilado"
}

# Slides:compilar y generar el pdf del proyecto latex relativo a la presentacion
Slides()
{
    pdflatex -output-directory=presentacion presentacion/Proyecto Moogle!-1.tex
    echo "Archivo latex referente a la presentacion, compilado"
}

# show_report: Ejecutar un programa para la visualizacion del informe
show_report()
{
    dir=informe/Proyecto Moogle!.pdf
        if test -f $dir;then
            abrirPDF "$1" "$dir"
        else
            report
            abrirPDF "$1" "$dir"
        fi
}

# show_slides: Ejecutar un programa para la visualizacion de la presentacion
show_slides()
{
    dir=presentacion/Proyecto Moogle!-1.pdf

    if test -f $dir; then
        abrirPDF "$1" "$dir"
    else 
        slides
        abrirPDF "$1" "$dir"
    fi
}
abrirPDF()
{
   if [ "$1" = $"" ] then
        xdg-open $2
    else
        $1 $2
    fi
}

# clean: Eliminacion de archivos innecesarios generados con la compilacion
clean()
{
    find . -type f -name '*.aux' -delete
    find . -type f -name '.synctex.gz' -delete
    find . -type f -name '*.log' -delete
    find . -type f -name '*.out' -delete
    find . -type f -name '*.pdf' -delete
    find . -type f -name '*.fdb_latexmk' -delete
    find . -type f -name '*.fls' -delete
    find . -type f -name '*.nav' -delete
    find . -type f -name '*.snm' -delete
    find . -type f -name '*.toc' -delete
    find . -type f -name '*.vrb' -delete
    echo "archivos innecesarios eliminados"
}
echo "
Comandos: 

run: Ejecuta y compila el proyecto Moogle!.

report: compilar y generar el pdf del proyecto latex relativo al informe.

slides: compilar y generar el pdf del proyecto latex relativo a la presentacion.

show_report: Ejecutar un programa para visualizar el informe. Especifique si lo desea el programa a utilizar.

show_slides: Ejecutar un programa para visualizar la presentacion. Especifique si lo desea el programa a utilizar.
    "

cd ..
help

while read input ; 
do 
    command=$(echo "$input" | cut -d' ' -f1)
    arg=$(echo "$input" | cut -d' ' -f2) 

    # echo $command
    # echo $arg

    case $command in 
        run)
            run
            ;;
        
        report)
            report
            ;;
        
        slides)
            slides
            ;;
        
        show_report)
            if [ "$command" == "$arg" ]; then
                show_report
            else
                show_report "$arg"
            fi
            ;;
        
        show_slides)
            if [ "$command" == "$arg" ]; then
                show_slides
            else
                show_slides "$arg"
            fi
            ;;
        
        clean)
            clean
            ;;
        
        help)
            help
            ;;

        clear)
            clear
            ;;

        exit)
            exit
            ;;

        *)
            echo "invalid command"
    esac

done