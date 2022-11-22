# Diplomarbeit PhysioTool

## Klonen des Repos

```
git clone https://github.com/Nik22370/PhysioTool.git
```

## Skripts

- *cleanSolution.cmd* (Windows) bzw. *cleanSolution.sh*: Löscht alle temporären Dateien von
  Visual Studio sowie den Ordner *node_modules*. Hilfreich bei Problemen.
- *startServer.cmd* (Windows) bzw. *startServer.sh*: Startet die ASP.NET Core API.

## Starten des ASP.NET Core Webservers

Führe in *Windows* die Datei *startServer.cmd* bzw. in macOS das Shellscript *startServer.sh* aus.
Es startet mit *dotnet watch run* die ASP.NET Core Applikation in *Physiotool.Webapi*.

## Deployment des Vue.js Frontends

Öffne das Client Projekt in *Physiotool.Client*. Mit *npm install* werden die Dependencies
in den Ordner *node_modules* geladen. Danach kann mit *npm run dev* der Dev Server gestartet
werden.

Soll ein Bundle erstellt werden, rufe *npm run build* auf. Es kopiert das Bundle in den wwwroot
Ordner des Webapi Projektes. Dann liefert der ASP.NET Core Server die App als Standarddokument
aus.

