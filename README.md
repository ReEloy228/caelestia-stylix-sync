---

# 🌈 Caelestia Stylix Sync

**Automatic color scheme synchronization between Caelestia and Stylix on NixOS**

[![NixOS](https://img.shields.io/badge/NixOS-24.05-blue?style=flat-square&logo=nixos)](https://nixos.org)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com)
[![License: GPL v3](https://img.shields.io/badge/License-GPL%20v3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

> 📖 [Read in Russian](./docs/README.ru.md)

---

## 📖 About

**Caelestia Stylix Sync** is a background service for NixOS that automatically tracks theme changes in **Caelestia** and applies them to **Stylix** without requiring a system rebuild.

- ⚡ **Reacts to changes** — updates Stylix theme immediately after Caelestia theme switch.
- 🧠 **Smart polling** — checks `scheme.json` every N seconds (configurable).
- 🎨 **Flexible generation** — supports `base` (standard palette) and `term` (terminal colors `term0..term15`) modes.
- 📦 **Nix integration** — declarative configuration via a single Nix module.
- 🚀 **Lightweight** — written in C# using .NET Generic Host and systemd.

---

## 🔧 Installation

### 1. Add the input to your `flake.nix`:

```nix
{
  inputs = {
    # ... other inputs
    caelestia-stylix-sync = {
      url = "github:ReEloy228/caelestia-stylix-sync";
      inputs.nixpkgs.follows = "nixpkgs";
    };
  };
}
```

### 2. Enable the module in your configuration:

```nix
{ config, pkgs, inputs, ... }:
{
  imports = [
    inputs.caelestia-stylix-sync.nixosModules.default
  ];

  services.caelestia-stylix-sync = {
    enable = true;
    user = "your_username";   # required
    # other options are optional
  };
}
```

### 3. Rebuild your system:

```bash
sudo nixos-rebuild switch
```

---

## ⚙️ Configuration options

| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `enable` | `bool` | `false` | Enable synchronization |
| `user` | `str` | **required** | User to run the service as |
| `themeFile` | `path` | `/etc/nixos/caelestia-theme.yaml` | Path to Stylix theme file |
| `schemeFile` | `path` | `~/.local/state/caelestia/scheme.json` | Path to Caelestia scheme file |
| `generateWith` | `enum` | `"base"` | Generation mode: `base` (standard palette) or `term` (terminal colors) |
| `logLevel` | `enum` | `"Information"` | Logging level: `Trace`, `Debug`, `Information`, `Warning`, `Error`, `Critical` |
| `pollingInterval` | `int` | `2` | Polling interval in seconds |

---

## 🎨 Example configuration

```nix
services.caelestia-stylix-sync = {
  enable = true;
  user = "user";
  themeFile = "/etc/nixos/my-theme.yaml";
  schemeFile = "/home/user/.local/state/caelestia/scheme.json";
  generateWith = "term";
  logLevel = "Debug";
  pollingInterval = 3;
};
```

---

## 🐧 Usage

After installation, the service starts automatically and runs in the background. You can manage it with standard systemd commands:

```bash
# Status
sudo systemctl status caelestia-stylix-sync

# Logs
sudo journalctl -u caelestia-stylix-sync -f

# Restart
sudo systemctl restart caelestia-stylix-sync

# Stop
sudo systemctl stop caelestia-stylix-sync
```

When you change the theme in Caelestia (e.g., via `caelestia scheme set --random`), the service automatically updates `themeFile`, and Stylix will apply the new theme on the next system rebuild (or immediately if you have auto-rebuild configured).

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! Feel free to open an issue or submit a pull request.

---


## 🙏 Acknowledgments

- [Caelestia](https://github.com/caelestia-dots) — for the inspiring theme ecosystem.
- [Stylix](https://github.com/nix-community/stylix) — for the powerful theming in Nix.
- [NixOS](https://nixos.org) — for the reliable and reproducible system.

---

**Made with ❤️ for the NixOS community**

---
