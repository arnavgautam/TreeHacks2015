#read -p 'Puppet Master VM Username (e.g.: admin@vmname.cloudapp.net): ' host
username=admin@contoso.com
password=Passw0rd!
host=adminuser@hmeydacmaster.cloudapp.net
pwd
ssh-keygen -f "~/.ssh/known_hosts" -R $host

#scp -r /assets/microsoft-sysinternals adminuser@hmeydacmaster.cloudapp.net:/tmp

ssh $host 'bash -s' << EOF
	cd /opt/puppet/share/puppet-dashboard
	sudo /opt/puppet/bin/bundle exec /opt/puppet/bin/rake -f /opt/puppet/share/console-auth/Rakefile db:create_user USERNAME=$username PASSWORD=$password ROLE='Admin'
	/opt/puppet/bin/puppet module install puppetlabs-registry
	/opt/puppet/bin/puppet module install joshcooper-powershell                               
EOF

read -p 'Press [Enter] key to exit...'
