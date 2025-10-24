# === AWS Secrets Manager para las variables sensibles ===

resource "aws_secretsmanager_secret" "app_secrets" {
  name = "${var.app_name}-secrets"

  # Evita que el secreto se borre por accidente al hacer terraform destroy
  lifecycle {
    prevent_destroy = true
  }
}

resource "aws_secretsmanager_secret_version" "app_secrets_version" {
  secret_id     = aws_secretsmanager_secret.app_secrets.id

  secret_string = jsonencode({
    JwtSettings__Key = var.jwt_secret
    JwtSettings__Issuer = var.jwt_issuer
    JwtSettings__Audience = var.jwt_audience
    JwtSettings__ExpirationMinutes = var.jwt_expiration
    ConnectionStrings__DefaultConnection = "Host=${aws_db_instance.postgres.address};Database=${var.db_name};Username=${var.db_username};Password=${var.db_password}"
  })
}
