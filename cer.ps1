﻿# setup certificate properties including the commonName (DNSName) property for Chrome 58+
$certificate = New-SelfSignedCertificate `
    -Subject ts `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -NotBefore (Get-Date) `
    -NotAfter (Get-Date).AddYears(10) `
    -CertStoreLocation "cert:CurrentUser\My" `
    -FriendlyName "Certificate for .NET Core" `
    -HashAlgorithm SHA256 `
    -KeyUsage DigitalSignature, KeyEncipherment, DataEncipherment `
    -TextExtension @("2.5.29.17={text}DNS=localhost&DNS=*.local&IPAddress=127.0.0.1&IPAddress=::1")
$certificatePath = 'Cert:\CurrentUser\My\' + ($certificate.ThumbPrint) 
 
# create temporary certificate path
#$tmpPath = "C:\tmp"
#If(!(test-path $tmpPath))
#{
#New-Item -ItemType Directory -Force -Path $tmpPath
#}
 
# set certificate password here
$pfxPassword = ConvertTo-SecureString -String "123456" -Force -AsPlainText
$pfxFilePath = "cer.pfx"
$cerFilePath = "cer.cer"

# create pfx certificate
Export-PfxCertificate -Cert $certificatePath -FilePath $pfxFilePath -Password $pfxPassword
Export-Certificate -Cert $certificatePath -FilePath $cerFilePath
 
# import the pfx certificate
# Import-PfxCertificate -FilePath $pfxFilePath Cert:\LocalMachine\My -Password $pfxPassword -Exportable
 
# trust the certificate by importing the pfx certificate into your trusted root
# Import-Certificate -FilePath $cerFilePath -CertStoreLocation Cert:\CurrentUser\Root
 
# optionally delete the physical certificates (don’t delete the pfx file as you need to copy this to your app directory)
# Remove-Item $pfxFilePath
# Remove-Item $cerFilePath