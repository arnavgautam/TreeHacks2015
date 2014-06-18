host=$1
username=$2
password=$3
clear
echo "Copying microsoft-sysinternals module to Puppet Master..."
scp -r ./Assets/microsoft-sysinternals $host:/tmp
echo "Done"
ssh $host 'bash -s' << EOF
	# Copy module to Puppet Modules
	sudo cp /tmp/microsoft-sysinternals /etc/puppetlabs/puppet/modules --force -r

	echo "Creating Console Admin User..."
	cd /opt/puppet/share/puppet-dashboard
	sudo /opt/puppet/bin/bundle exec /opt/puppet/bin/rake -f /opt/puppet/share/console-auth/Rakefile db:create_user USERNAME=$username PASSWORD=$password ROLE='Admin'
	echo "Done"
	echo "Installing Puppet prerequisites..."
	/opt/puppet/bin/puppet module install puppetlabs-registry
	/opt/puppet/bin/puppet module install joshcooper-powershell      

	echo "Creating Group if not exists..."
	sudo /opt/puppet/bin/rake -f /opt/puppet/share/puppet-dashboard/Rakefile RAILS_ENV=production nodegroup:add['Windows Servers']
	echo "Done"
	
	echo "Registering class..."
	sudo /opt/puppet/bin/rake -f /opt/puppet/share/puppet-dashboard/Rakefile RAILS_ENV=production nodeclass:add['microsoft-sysinternals','skip']
	echo "Done"
	
	echo "Removing classes from Group..."
	sudo /opt/puppet/bin/rake -f /opt/puppet/share/puppet-dashboard/Rakefile RAILS_ENV=production nodegroup:delclass['Windows Servers','microsoft-sysinternals']	
	echo "Done"
EOF

read -p 'Press [Enter] key to exit...'