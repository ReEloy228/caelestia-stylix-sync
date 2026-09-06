{
  config,
  pkgs,
  lib,
  ...
}:
with lib;
let
  cfg = config.services.caelestia-stylix-sync;
  group = "caelestia-stylix-sync";
  themeFile = cfg.themeFile;
  settingsFile = "/etc/caelestia-stylix-sync/settings.yaml";
in
{
  options.services.caelestia-stylix-sync = {
    enable = mkEnableOption "Caelestia to Stylix sync";

    user = mkOption {
      type = types.str;
      description = "User to run the service as";
    };

    themeFile = mkOption {
      type = types.path;
      default = "/etc/nixos/caelestia-theme.yaml";
      description = "Theme file path";
    };

    schemeFile = mkOption {
      type = types.path;
      default = "${config.users.users.${cfg.user}.home}/.local/state/caelestia/scheme.json";
      description = "Scheme file path (absolute path)";
    };

    generateWith = mkOption {
      type = types.enum [
        "base"
        "term"
      ];
      default = "base";
      description = "Use base16 colors or terminal palette (term0..term15)";
    };

    logLevel = mkOption {
      type = types.enum [
        "Trace"
        "Debug"
        "Information"
        "Warning"
        "Error"
        "Critical"
      ];
      default = "Information";
      description = "Logging level for the service";
    };

    pollingInterval = mkOption {
      type = types.ints.positive;
      default = 2;
      description = "Polling interval in seconds for checking scheme changes";
    };
  };

  config = mkIf cfg.enable {
    users.groups.${group} = { };
    users.users.${cfg.user}.extraGroups = [ group ];

    system.activationScripts.createCaelestiaTheme = {
      text = ''
        if [ ! -f ${themeFile} ]; then
          mkdir -p "$(dirname ${themeFile})"
          touch ${themeFile}
          chown root:${group} ${themeFile}
          chmod 664 ${themeFile}
          echo "# Managed by caelestia-stylix-sync" > ${themeFile}
        fi
      '';
      deps = [ ];
    };

    environment.etc."caelestia-stylix-sync/settings.yaml" = {
      text = ''
        themeFilePath: ${themeFile}
        schemeFilePath: ${cfg.schemeFile}
        pollingIntervalSeconds: ${toString cfg.pollingInterval}
        logLevel: ${cfg.logLevel}
        generateWith: ${cfg.generateWith}
      '';
      mode = "0644";
      user = "root";
      group = group;
    };

    systemd.services.caelestia-stylix-sync = {
      description = "Caelestia → Stylix theme synchronizer";
      after = [ "network.target" ];
      wantedBy = [ "multi-user.target" ];
      environment = {
        CAELESTIA_SYNC_CONFIG = settingsFile;
      };
      serviceConfig = {
        ExecStart = "${pkgs.caelestia-sync}/bin/caelestia-sync";
        User = cfg.user;
        Group = group;
        Restart = "always";
        RestartSec = 5;
      };
    };
  };
}
