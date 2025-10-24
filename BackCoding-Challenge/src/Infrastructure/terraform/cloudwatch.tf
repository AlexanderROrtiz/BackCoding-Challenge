resource "aws_cloudwatch_log_group" "ecs" {
  name              = "/BackCoding/Logs"
  retention_in_days = 7
}
