sudo yum install cronie
sudo echo "*/5 * * * * python3 /home/orion/GetServerPerfData.py" | sudo tee -a /var/spool/cron/orion
crontab -l
sudo service crond stop
sudo service crond start
