FROM mcr.microsoft.com/devcontainers/dotnet:9.0-bookworm

# Install wget and other necessary tools
RUN apt-get update && apt-get install -y \
    wget \
    && rm -rf /var/lib/apt/lists/*

# Download and install Go
RUN wget https://go.dev/dl/go1.23.3.linux-amd64.tar.gz \
    && rm -rf /usr/local/go \
    && tar -C /usr/local -xzf go1.23.3.linux-amd64.tar.gz \
    && rm go1.23.3.linux-amd64.tar.gz

# Set Go environment variables
ENV PATH=$PATH:/usr/local/go/bin

USER vscode

COPY .scripts/install_cli_tools.sh install_cli_tools.sh

RUN bash ./install_cli_tools.sh