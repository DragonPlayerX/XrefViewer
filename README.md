# Xref Viewer

## Requirements

- [MelonLoader 0.4.x](https://melonwiki.xyz/)

## Description

This mod utilizes the Xref scanning methods from [Il2CppAssemblyUnhollower](https://github.com/knah/Il2CppAssemblyUnhollower) and provides user input based scans at runtime controlled by a simple functional WinForms window.

## Features

- Scan a method for **UsedBy** and **Using** (also prints strings)
- Print only strings from containing methods
- Scan all methods from a given type

---

## Usage

| Command | Description | Arguments | Argument Description
|-|-|-|-|
|xref|Scan a method or a type|-t typename<br>-m methodname<br>-s<br>-c<br>-l|-t Defines the type<br>-m Defines the method from the type<br>-s Print only strings of the method<br>-c Use methodname as part of name<br>-l Allow large scan results
|dump|Writes current console contents to a file|-f filepath|-f Defines the destination file
|clear|Clears the console window|
|help|Print all commands and their description

### Screenshot

![Screenshot](https://i.imgur.com/DvcnjeY.png)