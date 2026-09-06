{ pkgs, ... }:
pkgs.buildDotnetModule {
  pname = "caelestia-sync";
  version = "0.1.0";
  src = ./src;

  projectFile = "CaelestiaStylixSync.csproj";
  nugetDeps = ./deps.json;

  selfContainedBuild = true;
  runtime = "linux-x64";
  dotnet-sdk = pkgs.dotnet-sdk_10;

  enableParallelBuilding = true;

  postFixup = ''
    mv $out/bin/CaelestiaStylixSync $out/bin/caelestia-sync
  '';

  meta = with pkgs.lib; {
    description = "Synchronize Caelestia theme to Stylix";
    platforms = platforms.linux;
  };
}
