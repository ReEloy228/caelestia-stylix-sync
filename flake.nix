{
  description = "Synchronize Caelestia theme to Stylix";
  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
  };
  outputs =
    {
      self,
      nixpkgs,
    }:
    let
      system = "x86_64-linux";
      pkgs = nixpkgs.legacyPackages.${system};
    in
    {
      nixosModules.default = import ./modules/caelestia-stylix-sync.nix;
      packages.${system}.caelestia-sync = pkgs.callPackage ./pkgs { };
    };
}
