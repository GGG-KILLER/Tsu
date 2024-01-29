#! /usr/bin/env nix-shell
#! nix-shell -i bash -p 'with pkgs.dotnetCorePackages; combinePackages [ sdk_8_0 sdk_7_0 sdk_6_0 ]'
# shellcheck shell=bash
set -euo pipefail

[ -d packages ] && rm -r packages;

dotnet clean -c Debug -v quiet;
dotnet clean -c Release -v quiet;

dotnet restore;

dotnet build --configuration Release --no-restore -v quiet;

if ! dotnet pack --configuration Release --no-build --output packages -v quiet; then
    echo "Failed to package."
fi
