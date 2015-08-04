ECHO OFF
del nuget\*.nupkg /Q
cls
ECHO CRIANDO PACOTE NUGET...
ECHO ----------------------------
nuget pack ".\Public Libraries\VTEX.DynamicConfigurationSection\VTEX.Configuration.DynamicSection.csproj" -Build -Properties Configuration=Release -OutputDirectory nuget
ECHO ----------------------------
pause