#!/bin/sh
if [ "$#" != "1" ]
then 
    echo Apel: $0 \<fisier_fara_extensie\> && exit 1
fi

bison -d "$1.y" && echo Bison compilat || (echo bison nu a putut compila "$1.y" && exit 1)
flex "$1.l" && echo flex compilat || (echo flex a putut compila "$1.l" && exit 1)

g++ "$1.tab.c" lex.yy.c || (echo "Nu s-a putut compila cu g++" && exit 1)
rm lex.yy.c "$1.tab.c" "$1.tab.h"
echo gcc compilat
echo Running program ./a.out
echo
./a.out || (echo "Nu s-a putut genera fisierul nasm" && rm ./a.out && exit 1)
rm ./a.out
