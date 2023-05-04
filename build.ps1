Param(
    [parameter(Mandatory=$true)][string]$tag
)


docker build --no-cache  -f  .\App\TS.Microservices.WebApi\Dockerfile -t ts-microservices-webapi:$tag .
docker build --no-cache  -f  .\ApiGateways\TS.Microservices.Mobile.Gateway\Dockerfile -t ts-microservices-mobile-gateway:$tag .
docker build --no-cache  -f  .\ApiGateways\TS.Microservices.Mobile.ApiAggregator\Dockerfile -t ts-microservices-mobile-apiaggregator:$tag .
docker build --no-cache  -f  .\Monitor\TS.Microservices.HealthChecksHost\Dockerfile -t ts-microservices-healthcheckshost:$tag .

"Any key to exit"  ;
Read-Host | Out-Null ;
Exit
