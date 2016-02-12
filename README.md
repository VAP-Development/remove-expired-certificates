# remove-expired-certificates
A .NET console app to remove expired certificates from a certificate store

##Usage

###Test
Doesn't actualy remove any certificates but tells you what it would remove

``` remove-expired-certificates.exe ```

###Live
Will remove any expired certificates it finds under the default store (WebHosting or Personal if WebHosting doesn't exist)

``` remove-expired-certificates.exe --live```

###Live Specified Cert Store
Will remove any expired certificates it finds under the "MyCertStore" store

``` remove-expired-certificates.exe --live --store MyCertStore```
