# Xref Viewer

## Requirements

- [MelonLoader 0.5.4+](https://melonwiki.xyz/)

## Description

The project is now based on a mod which extracts a WPF (.NET 4.7.2) application on startup and transfers data via tcp sockets between processes. WPF performs much faster and better than WinForms running inside a mod.

This mod utilizes xref scanning methods from [Il2CppAssemblyUnhollower](https://github.com/knah/Il2CppAssemblyUnhollower) and provides user input based scans at runtime controlled by a external WPF application. It can be very helpful for modders to find methods via xref scanning to have persistence against game updates.

## Example Video


## Credits

The repository includes files of the following packages used for the WPF application:

- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) (Covered by [MIT  license](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md))
- [AdonisUI](https://github.com/benruehl/adonis-ui) (Covered by [MIT  license](https://github.com/benruehl/adonis-ui/blob/master/LICENSE))

# WinForms Version (Deprecated)

## Usage

| Command | Description | Arguments | Argument Description
|-|-|-|-|
|xref|Scan a method or a type|-t typename<br>-m methodname<br>-s<br>-c<br>-l|-t Defines the type<br>-m Defines the method from the type<br>-s Print only strings of the method<br>-c Use methodname as part of name<br>-l Allow large scan results
|dump|Writes console to file|-f filepath|-f Defines the destination file
|clear|Clears the console window|
|help|Prints all commands

### Screenshot

![Screenshot](https://i.imgur.com/DvcnjeY.png)