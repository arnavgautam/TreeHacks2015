@echo off

echo Removing certificates...
echo.

certutil -delstore My "localhost"

certutil -delstore Root "Root Agency"

certutil -delstore My "IdentityTKStsCert"

certutil -delstore TrustedPeople "IdentityTKStsCert"

certutil -delstore My "identitytk_cloud_rp.cloudapp.net"

certutil -delstore -user My "identitytk_cloud_rp.cloudapp.net"

certutil -delstore -user TrustedPeople "identitytk_cloud_rp.cloudapp.net"

echo.
echo Clean up finished!
echo.