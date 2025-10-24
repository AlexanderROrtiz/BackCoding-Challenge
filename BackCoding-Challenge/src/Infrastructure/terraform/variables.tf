variable "aws_region" {
  default = "us-east-1"
}

variable "aws_profile" {
  default = "default"
}

variable "app_name" {
  default = "backcoding-api"
}

variable "image_url" {
  description = "ECR image URL"
  default     = "913500087816.dkr.ecr.us-east-1.amazonaws.com/backcoding-api:latest"
}

variable "db_name" {
  default = "DB_test"
}

variable "db_username" {
  default = "postgres"
}

variable "db_password" {
  default = "postgres123"
}

variable "jwt_secret" {
  default = "MySuperSecretKeyForJwtTerraform"
}
