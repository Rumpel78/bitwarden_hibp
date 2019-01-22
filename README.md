# bitwarden_hibp

Export Bitwarden passwords and check against 'Have I be pawnd'

## Usage:

* Export your passwords from [Bitwarden](https://bitwarden.com/) and save them as JSON
* Install dotnet from [Microsoft](https://dotnet.microsoft.com/download)
* Enter folder `dotnet_app`
* Execute `dotnet restore`
* Execute `dotnet run <export_filename.json>` where `<export_filename.json>` is your exported file

If you enter the directo

Example:

`dotnet run bitwarden_export_sample.json`
