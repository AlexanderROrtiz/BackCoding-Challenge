resource "aws_ecs_cluster" "main" {
  name = "${var.app_name}-cluster"
}

# === Security Group del ECS ===
resource "aws_security_group" "ecs_sg" {
  name   = "${var.app_name}-ecs-sg"
  vpc_id = aws_vpc.main.id

  ingress {
    from_port       = 8080
    to_port         = 8080
    protocol        = "tcp"
    security_groups = [aws_security_group.alb_sg.id]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = { Name = "${var.app_name}-ecs-sg" }
}

# === Task Definition ===
resource "aws_ecs_task_definition" "api_task" {
  family                   = "${var.app_name}-task"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = "256"
  memory                   = "512"

  # --- Contenedor principal ---
  container_definitions = jsonencode([
    {
      name      = "backcoding"
      image     = var.image_url
      essential = true
      portMappings = [{ containerPort = 8080 }]

      # Aquí lees secretos directamente desde Secrets Manager
      secrets = [
        {
          name      = "JwtSettings__Key"
          valueFrom = "${aws_secretsmanager_secret.app_secrets.arn}:JwtSettings__Key::"
        },
        {
          name      = "JwtSettings__Issuer"
          valueFrom = "${aws_secretsmanager_secret.app_secrets.arn}:JwtSettings__Issuer::"
        },
        {
          name      = "JwtSettings__Audience"
          valueFrom = "${aws_secretsmanager_secret.app_secrets.arn}:JwtSettings__Audience::"
        },
        {
          name      = "JwtSettings__ExpirationMinutes"
          valueFrom = "${aws_secretsmanager_secret.app_secrets.arn}:JwtSettings__ExpirationMinutes::"
        },
        {
          name      = "ConnectionStrings__DefaultConnection"
          valueFrom = "${aws_secretsmanager_secret.app_secrets.arn}:ConnectionStrings__DefaultConnection::"
        }
      ]

      environment = [
        { name = "ASPNETCORE_ENVIRONMENT", value = "Production" }
      ]

      logConfiguration = {
        logDriver = "awslogs"
        options = {
          "awslogs-group"         = aws_cloudwatch_log_group.ecs.name
          "awslogs-region"        = var.aws_region
          "awslogs-stream-prefix" = "ecs"
        }
      }
    }
  ])

  execution_role_arn = aws_iam_role.ecs_execution_role.arn
  task_role_arn      = aws_iam_role.ecs_execution_role.arn
}

# === ECS Service ===
resource "aws_ecs_service" "api_service" {
  name            = "${var.app_name}-service"
  cluster         = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.api_task.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets          = [aws_subnet.public_a.id, aws_subnet.public_b.id]
    assign_public_ip = true
    security_groups  = [aws_security_group.ecs_sg.id]
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.api_tg.arn
    container_name   = "backcoding"
    container_port   = 8080
  }

  depends_on = [aws_lb_listener.http]
}
