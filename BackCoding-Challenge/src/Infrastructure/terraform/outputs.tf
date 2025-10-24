output "alb_dns_name" {
  value = aws_lb.api_alb.dns_name
}

output "ecs_cluster_name" {
  value = aws_ecs_cluster.main.name
}

output "rds_endpoint" {
  value = aws_db_instance.postgres.address
}

output "api_log_group" {
  value = aws_cloudwatch_log_group.ecs.name
}

output "secrets_arn" {
  value = aws_secretsmanager_secret.app_secrets.arn
}
