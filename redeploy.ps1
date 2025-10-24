# ==============================
# Redeploy automático BackCoding API
# ==============================
# para ajecutar en la carpeta donde esta el archivo   ./redeploy.ps1
# Variables de entorno (ajústar si cambian)
param(
    [string]$AWS_ACCOUNT_ID = "913500087816",
    [string]$AWS_REGION = "us-east-1",
    [string]$APP_NAME = "backcoding-api"
)

# ------------------------------
# 1️⃣ Variables y configuración
# ------------------------------
$ECR_REPO = "$AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$APP_NAME"
$ECS_CLUSTER = "${APP_NAME}-cluster"
$ECS_SERVICE = "${APP_NAME}-service"
$TerraformDir = "./src/Infrastructure/terraform"

Write-Host "====================================================="
Write-Host " Redeploy automático para $APP_NAME en $AWS_REGION"
Write-Host "====================================================="

# ------------------------------
# 2️⃣ Construcción de imagen Docker
# ------------------------------
Write-Host "`n  Construyendo imagen Docker..."
docker build -t $APP_NAME .

if ($LASTEXITCODE -ne 0) { Write-Error " Error al construir la imagen Docker"; exit 1 }

# ------------------------------
# 3️⃣ Etiquetado y push a ECR
# ------------------------------
Write-Host "`n  Etiquetando imagen..."
docker tag $APP_NAME "$ECR_REPO:latest"

Write-Host " Subiendo imagen al ECR..."
docker push "$ECR_REPO:latest"

if ($LASTEXITCODE -ne 0) { Write-Error " Error al subir imagen al ECR"; exit 1 }

Write-Host " Imagen subida correctamente."

# ------------------------------
# 4️⃣ Aplicar Terraform (Infraestructura)
# ------------------------------
Write-Host "`n Aplicando Terraform..."
cd $TerraformDir
terraform init -input=false
terraform plan -out=tfplan
terraform apply -auto-approve tfplan

if ($LASTEXITCODE -ne 0) { Write-Error " Error al aplicar Terraform"; exit 1 }

# ------------------------------
# 5️⃣ Forzar redeploy en ECS
# ------------------------------
Write-Host "`n Forzando nuevo deployment en ECS..."
aws ecs update-service `
  --cluster $ECS_CLUSTER `
  --service $ECS_SERVICE `
  --force-new-deployment `
  --region $AWS_REGION | Out-Null

if ($LASTEXITCODE -ne 0) { Write-Error " Error al forzar redeploy ECS"; exit 1 }

# ------------------------------
# 6️⃣ Esperar hasta que el servicio esté Healthy
# ------------------------------
Write-Host "`n Esperando que el ECS esté Healthy..."
$timeout = (Get-Date).AddMinutes(10)
do {
    $status = aws ecs describe-services `
        --cluster $ECS_CLUSTER `
        --services $ECS_SERVICE `
        --region $AWS_REGION `
        --query "services[0].deployments[0].rolloutState" `
        --output text

    if ($status -eq "COMPLETED") {
        Write-Host " ECS está desplegado correctamente y en estado Healthy."
        break
    }
    elseif ((Get-Date) -gt $timeout) {
        Write-Error " Timeout: el servicio ECS no alcanzó estado Healthy en 10 minutos."
        exit 1
    }

    Write-Host " Aún desplegando... estado actual: $status"
    Start-Sleep -Seconds 30
} while ($true)

# ------------------------------
# Mostrar la URL del Load Balancer
# ------------------------------
Write-Host "`n Obteniendo URL del Load Balancer..."
$albUrl = terraform output -raw alb_dns_name
Write-Host "`n Despliegue completado correctamente."
Write-Host " URL de acceso: http://$albUrl/swagger"