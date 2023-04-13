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