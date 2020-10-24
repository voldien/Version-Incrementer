## Unity Version Incrementer Setting Provider

A setting provider for adding automatic version incrementor of the project. Where it can be configured both manually and automatically. Where automatic mode will provide a setting to when the project gets incremented.

## Installation

### Simple Download
[Latest Unity Packages](../../releases/latest)

### Unity Package Manager (UPM)

> You will need to have git installed and set in your system PATH.

Find `Packages/manifest.json` in your project and add the following:
```json
{
  "dependencies": {
    "com.linuxsenpai.versionincrementer": "https://github.com/voldien/Version-Incrementer.git",
    "...": "..."
  }
}
```
