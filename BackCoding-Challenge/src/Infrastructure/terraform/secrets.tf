resource "aws_secretsmanager_secret" "app_secrets" {
  name = "${var.app_name}-secrets"
}

resource "aws_secretsmanager_secret_version" "app_secrets_version" {
  secret_id     = aws_secretsmanager_secret.app_secrets.id
  secret_string = jsonencode({
    JwtSettings__Key = var.jwt_secret,
    ConnectionStrings__DefaultConnection = "Host=${aws_db_instance.postgres.address};Database=${var.db_name};Username=${var.db_username};Password=${var.db_password}"
  })
}
