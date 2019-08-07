
provider "aws" {
  region = "eu-west-3"
}

resource "aws_instance" "ChrisSmith-WPress" {
  ami                    = "ami-0bb80da74ca11153c"
  instance_type          = "t2.small"
  vpc_security_group_ids = ["${aws_security_group.instance.id}"]
  key_name = "ChrisSmith-test"
  tags = {
    Name = "ChrisSmith-WPress"
    Environment = "test"
  }
}

resource "aws_security_group" "instance" {
  name = "ChrisSmith-WPress-instance"

  ingress {
    from_port   = "${var.server_port}"
    to_port     = "${var.server_port}"
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = "22"
    to_port     = "22"
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_elb" "chrissmith-elb" {  
  name = "ChrisSmith-ELB"
  availability_zones = ["eu-west-3a","eu-west-3b","eu-west-3c"]
   
  listener {
    instance_port     = 80
    instance_protocol = "http"
    lb_port           = 80
    lb_protocol       = "http"
  }
  
  health_check {
    healthy_threshold = 2
    unhealthy_threshold = 2
    timeout = 3
    interval =30
    target = "HTTP:80/"
  }

  tags = {    
    Name    = "ChrisSmith-ELB"    
  }   
}

resource "aws_elb_attachment" "ChrisSmith-WP1" {
  elb      = "${aws_elb.chrissmith-elb.id}"
  instance = "${aws_instance.ChrisSmith-WPress.id}"
}
