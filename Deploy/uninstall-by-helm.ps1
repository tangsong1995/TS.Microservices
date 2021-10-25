helm uninstall ts-microservices-webapi  -n default
helm uninstall ts-microservices-mobile-gateway  -n default
helm uninstall ts-microservices-mobile-apiaggregator  -n default
helm uninstall ts-microservices-healthcheckshost  -n default

"Any key to exit"  ;
Read-Host | Out-Null ;
Exit