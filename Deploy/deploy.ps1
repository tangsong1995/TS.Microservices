kubectl create configmap ts-microservices-webapi-config --from-file=ts-microservices-webapi/configs -o yaml --dry-run | kubectl apply -f - 
kubectl apply -f .\ts-microservices-webapi\ts-microservices-webapi.yaml

kubectl create configmap ts-microservices-mobile-apiaggregator-config --from-file=ts-microservices-mobile-apiaggregator/configs -o yaml --dry-run | kubectl apply -f - 
kubectl apply -f .\ts-microservices-mobile-apiaggregator\ts-microservices-mobile-apiaggregator.yaml

kubectl create configmap ts-microservices-config --from-env-file=env.txt -o yaml --dry-run | kubectl apply -f - 

kubectl create configmap ts-microservices-mobile-gateway-config --from-file=ts-microservices-mobile-gateway/configs -o yaml --dry-run | kubectl apply -f - 
kubectl apply -f .\ts-microservices-mobile-gateway\ts-microservices-mobile-gateway.yaml
kubectl apply -f .\ts-microservices-mobile-gateway\ts-microservices-mobile-gateway-ingress.yaml

kubectl create configmap ts-microservices-healthcheckshost-config --from-file=ts-microservices-healthcheckshost/configs -o yaml --dry-run | kubectl apply -f - 
kubectl apply -f .\ts-microservices-healthcheckshost\ts-microservices-healthcheckshost.yaml

"Any key to exit"  ;
Read-Host | Out-Null ;
Exit