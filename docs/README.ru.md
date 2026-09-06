# 🌈 Caelestia Stylix Sync

**Автоматическая синхронизация цветовой схемы между Caelestia и Stylix в NixOS**

[![NixOS](https://img.shields.io/badge/NixOS-24.05-blue?style=flat-square&logo=nixos)](https://nixos.org)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com)
[![License: GPL v3](https://img.shields.io/badge/License-GPL%20v3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

---

## 📖 О проекте

**Caelestia Stylix Sync** — это фоновый сервис для NixOS, который автоматически отслеживает изменения темы в **Caelestia** и применяет их к **Stylix** без необходимости пересборки системы.

- ⚡ **Реагирует на изменения** — обновляет тему Stylix сразу после смены темы в Caelestia.
- 🧠 **Умный опрос** — проверяет файл `scheme.json` каждые N секунд (настраивается).
- 🎨 **Гибкая генерация** — поддерживает режимы `base` (стандартная палитра) и `term` (терминальные цвета `term0..term15`).
- 📦 **Интеграция с Nix** — декларативная настройка через единый Nix-модуль.
- 🚀 **Легковесный** — написан на C# с использованием .NET Generic Host и systemd.

---

## 🔧 Установка

### 1. Добавьте вход в ваш `flake.nix`:

```nix
{
  inputs = {
    # ... другие входы
    caelestia-stylix-sync = {
      url = "github:ReEloy228/caelestia-stylix-sync";
      inputs.nixpkgs.follows = "nixpkgs";
    };
  };
}
```

### 2. Включите модуль в вашей конфигурации:

```nix
{ config, pkgs, inputs, ... }:
{
  imports = [
    inputs.caelestia-stylix-sync.nixosModules.default
  ];

  services.caelestia-stylix-sync = {
    enable = true;
    user = "ваш_пользователь";   # обязательный параметр
    # остальные опции – по желанию
  };
}
```

### 3. Пересоберите систему:

```bash
sudo nixos-rebuild switch
```

---

## ⚙️ Настройка (опции модуля)

| Опция | Тип | По умолчанию | Описание |
|-------|-----|--------------|----------|
| `enable` | `bool` | `false` | Включить синхронизацию |
| `user` | `str` | **обязательно** | Пользователь, от имени которого запускается сервис |
| `themeFile` | `path` | `/etc/nixos/caelestia-theme.yaml` | Путь к файлу темы Stylix |
| `schemeFile` | `path` | `~/.local/state/caelestia/scheme.json` | Путь к файлу схемы Caelestia |
| `generateWith` | `enum` | `"base"` | Режим генерации: `base` (стандартная палитра) или `term` (терминальные цвета) |
| `logLevel` | `enum` | `"Information"` | Уровень логирования: `Trace`, `Debug`, `Information`, `Warning`, `Error`, `Critical` |
| `pollingInterval` | `int` | `2` | Интервал опроса файла схемы (в секундах) |

---

## 🎨 Пример конфигурации

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

## 🐧 Использование

После установки сервис запускается автоматически и работает в фоне. Вы можете управлять им стандартными systemd-командами:

```bash
# Статус
sudo systemctl status caelestia-stylix-sync

# Логи
sudo journalctl -u caelestia-stylix-sync -f

# Перезапуск
sudo systemctl restart caelestia-stylix-sync

# Остановка
sudo systemctl stop caelestia-stylix-sync
```

При смене темы в Caelestia (например, через `caelestia scheme set --random`) сервис автоматически обновит `themeFile`, и Stylix применит новую тему при следующей пересборке системы (или сразу, если у вас настроена автоматическая пересборка).

---

## 🤝 Вклад в проект

Приветствуются любые идеи, исправления и улучшения. Создавайте Issue или Pull Request — и мы вместе сделаем синхронизацию ещё лучше!

---

## 🙏 Благодарности

- [Caelestia](https://github.com/caelestia-dots) — за вдохновляющую экосистему тем.
- [Stylix](https://github.com/nix-community/stylix) — за мощный механизм тем в Nix.
- [NixOS](https://nixos.org) — за надёжную и воспроизводимую систему.

---

**Сделано с ❤️ для сообщества NixOS**

---
