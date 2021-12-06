# RHEL 
# Supported Version 3.X
sudo yum install python3-pip
sudo python3 -m pip --version
sudo yum install unixodbc-dev
sudo yum install unixODBC unixODBC-devel
sudo yum install epel-release
sudo yum install gcc-c++

sudo yum install python3-devel
sudo pip3 install pyodbc

# Red Hat Enterprise Server 8 and Oracle Linux 8
sudo su
# Red Hat Enterprise Server 8 and Oracle Linux 8
curl https://packages.microsoft.com/config/rhel/8/prod.repo > /etc/yum.repos.d/mssql-release.repo
 exit
sudo yum remove unixODBC-utf16 unixODBC-utf16-devel #to avoid conflicts
sudo ACCEPT_EULA=Y yum install -y msodbcsql17
