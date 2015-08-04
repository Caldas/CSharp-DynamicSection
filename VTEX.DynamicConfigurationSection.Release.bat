ECHO OFF
del nuget\*.nupkg /Q
cls
ECHO CRIANDO PACOTE NUGET...
ECHO ----------------------------
nuget pack ".\src\Public Libraries\VTEX.DynamicConfigurationSection\VTEX.Configuration.DynamicSection.csproj" -Build -Properties Configuration=Release -OutputDirectory nuget
ECHO ----------------------------
ECHO PACOTE NUGET CRIADO. PUBLICANDO O PACOTE NO MYGET...
ECHO ----------------------------
nuget push nuget\*.nupkg 
ECHO ----------------------------
ECHO PACOTE PUBLICADO COM SUCESSO
pause
del nuget\*.nupkg /Q