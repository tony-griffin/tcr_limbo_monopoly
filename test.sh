#!/usr/bin/env bash

dotnet test HaveFun/
if [ $? -eq 0 ]
then
    git add .
    git commit -am "automatically commit passing test"
else
    git reset --hard
    git clean -fd
fi