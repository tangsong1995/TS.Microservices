helm repo update
helm install prometheus-operator stable/prometheus-operator --version 8.10.0 --values .\prometheus-operator\values.yaml  --namespace kube-system