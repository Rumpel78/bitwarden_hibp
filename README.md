# bitwarden_hibp

Export Bitwarden passwords and check against [';--have i been pwned?](https://haveibeenpwned.com/)  
_This is a very naive implementation with no error checking etc._

## Usage:

* Export your passwords from [Bitwarden](https://bitwarden.com/) and save them as JSON
* Install dotnet from [Microsoft](https://dotnet.microsoft.com/download)
* Enter folder `dotnet_app`
* Execute `dotnet restore`
* Execute `dotnet run <export_filename.json>` where `<export_filename.json>` is your exported file

Example:

`dotnet run bitwarden_export_sample.json`

Example output:

>Checking 2 passwords  
>  
>2/2      Found: 2  
>Unsafe logins found:  
>  
>7877c6d0-ed3d-41b5-b7bc-a95600df32ef  
>Account1  
>Username1  
>123456  
>  
>8648a566-4bf8-4986-852d-a95600ddc6f8  
>Account2  
>Username2  
>Password2  