FROM mcr.microsoft.com/devcontainers/dotnet:9.0-bookworm

USER vscode

COPY .scripts/install_cli_tools.sh install_cli_tools.sh

RUN bash ./install_cli_tools.sh