output "Wordpress_public_ip" {
  value = "${aws_instance.ChrisSmith-WPress.public_ip}"
#  description "ssh login like ssh-i ~/.ssh/id_rsa bitnami@public_ip"
}

output "ELB_public_dns_name" {
  value = "${aws_elb.chrissmith-elb.dns_name}"
  description = "Only http Open"
}
