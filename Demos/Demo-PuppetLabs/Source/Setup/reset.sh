#read -p 'Puppet Master VM Username (e.g.: admin@vmname.cloudapp.net): ' host
username=admin@contoso.com
password=Passw0rd!
host=adminuser@hmeydacmaster.cloudapp.net
pwd
ssh-keygen -f "~/.ssh/known_hosts" -R $host

ssh $host 'bash -s' << EOF
	# Create admin user
	cd /opt/puppet/share/puppet-dashboard
	sudo /opt/puppet/bin/bundle exec /opt/puppet/bin/rake -f /opt/puppet/share/console-auth/Rakefile db:create_user USERNAME=$username PASSWORD=$password ROLE='Admin'
	# Install Puppet prerequisites
	/opt/puppet/bin/puppet module install puppetlabs-registry
	/opt/puppet/bin/puppet module install joshcooper-powershell      

	# Create Group if not exists
	sudo /opt/puppet/bin/rake -f /opt/puppet/share/puppet-dashboard/Rakefile RAILS_ENV=production nodegroup:add['Windows Servers']

	# Remove classes from Group
	sudo /opt/puppet/bin/rake -f /opt/puppet/share/puppet-dashboard/Rakefile RAILS_ENV=production nodegroup:delclass['Windows Servers','microsoft-sysinternals']	
EOF

read -p 'Press [Enter] key to exit...'