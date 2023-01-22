# PlatformService
Platform microservice

## Create and publish contacts package
```powershell
$version="0.0.18"
$local_packages_path="D:\Dev\NugetPackages"
$baget_key="KEY HERE"

dotnet pack src\PlatformContracts --configuration Release -p:PackageVersion=$version -o $local_packages_path

dotnet nuget push $local_packages_path\PlatformContracts.$version.nupkg --api-key $baget_key --source baget
```

## Build the docker image
```powershell

docker build -t platform.service:$version .
```

## Run the docker image on local machine
```powershell
docker run -it --rm -p 5231:5231 --name platformservice platform.service:$version
```

## Push the docker on Local Container Registry
```powershell

$crname="container-registry.docker.local:5000"
docker tag platform.service:$version "$crname/platform.service:$version"

docker push "$crname/platform.service:$version"
```

## Deploy to kubernetes
```powershell

$namespace="platform"
kubectl create namespace $namespace

kubectl apply -f .\kubernetes\deployment.yaml -n $namespace

kubectl apply -f .\kubernetes\node-port-service.yaml -n $namespace

kubectl apply -f .\kubernetes\cluster-ip-service.yaml -n $namespace


#get deployments 
kubectl get deployments -n $namespace

# restart deployment 
kubectl rollout restart deployment platform-deployment -n $namespace

# list pods in namespace
kubectl get pods -n $namespace -w

# list services in namespace
kubectl get services -n $namespace
``` 

