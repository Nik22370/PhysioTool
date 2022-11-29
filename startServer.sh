#!/bin/bash

for path in Physiotool Physiotool/Physiotool.Application Physiotool/Physiotool.Webapi
do
    rm -rf $path/bin 2> /dev/null
    rm -rf $path/obj 2> /dev/null
    rm -rf $path/.vs 2> /dev/null
    rm -rf $path/.vscode 2> /dev/null
done

cd Physiotool
cd Physiotool.Webapi
dotnet watch run -c Debug
