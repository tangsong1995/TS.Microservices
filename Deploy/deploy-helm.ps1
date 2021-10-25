kubectl create configmap ts-microservices-webapi-config --from-file=ts-microservices-webapi/configs -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap ts-microservices-mobile-gateway-config --from-file=ts-microservices-mobile-gateway/configs -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap ts-microservices-config --from-env-file=env.txt -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap ts-microservices-mobile-apiaggregator-config --from-file=ts-microservices-mobile-apiaggregator/configs -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap ts-microservices-healthcheckshost-config --from-file=ts-microservices-healthcheckshost/configs -o yaml --dry-run | kubectl apply -f - 

helm upgrade ts-microservices-webapi .\charts\ts-microservices-webapi -n default
helm upgrade ts-microservices-mobile-gateway .\charts\ts-microservices-mobile-gateway -n default
helm upgrade ts-microservices-mobile-apiaggregator .\charts\ts-microservices-mobile-apiaggregator -n default
helm install ts-microservices-healthcheckshost  .\charts\ts-microservices-healthcheckshost -n default

"Any key to exit"  ;
Read-Host | Out-Null ;
Exit