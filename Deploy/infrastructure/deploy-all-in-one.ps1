kubectl create configmap fluentd-config --from-file=fluentd -o yaml --dry-run | kubectl apply -f - 
kubectl apply -f .

"Any key to exit"  ;
Read-Host | Out-Null ;
Exit