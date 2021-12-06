# Supported Version 3.X
sudo apt install python3-pip
python3 -m pip --version
python3 -m ensurepip --default-pip
# python3.exe -m pip install --upgrade pip
python3 -m pip install wheel
python3 -m pip install psutil
sudo apt install unixodbc-dev
sudo apt install unixODBC unixODBC-devel
python3 -m pip install pyodbc 
# Debian 10
sudo su
apt-get install curl
curl https://packages.microsoft.com/config/debian/10/prod.list > /etc/apt/sources.list.d/mssql-release.list
exit
sudo apt-get update
sudo ACCEPT_EULA=Y apt-get install -y msodbcsql17