## Install additional apt packages
sudo apt-get update && \
    sudo apt-get install -y dos2unix libsecret-1-0

## Configure git
git config --global core.autocrlf input

## Enable local HTTPS for .NET
dotnet dev-certs https --trust

## Restore .NET packages and build the default solution
dotnet restore && dotnet build

# Install Azure Static Web Apps CLI.
npm install -g @azure/static-web-apps-cli

# Install Azure Dev CLI
curl -fsSL https://aka.ms/install-azd.sh | bash

# Install Azure Functions Core Tools
curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
sudo sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-$(lsb_release -cs)-prod $(lsb_release -cs) main" > /etc/apt/sources.list.d/dotnetdev.list'
sudo apt-get update
sudo apt-get install azure-functions-core-tools-4
