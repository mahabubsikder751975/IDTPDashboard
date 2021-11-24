#!/usr/bin/python  
try:
   import time
except ImportError:
    print ("Cannot import time module - this is needed for this application.");
    print ("Exiting...")
    sys.exit();
  
try:
    import psutil
except ImportError:
    print ("Cannot import psutil module - this is needed for this application.");
    print ("Exiting...")
    sys.exit();
  
try:
    import re                 # Needed for regex
except ImportError:
    print ("Cannot import re module - this is needed for this application.");
    print ("Exiting...")
    sys.exit();

try:
    import datetime                 # Needed for regex
except ImportError:
    print ("Cannot import datetime module - this is needed for this application.");
    print ("Exiting...")
    sys.exit();

try:  
    import pyodbc
except ImportError:
    print ("Cannot import python ODBC - this is needed for this application.");
    print ("Exiting...")
    sys.exit();

# Importing socket library
try:  
    import socket
except ImportError:
    print ("Cannot import socket - this is needed for this application.");
    print ("Exiting...")
    sys.exit();


def bytes2human(n):
    # From sample script for psutils
    """
    >>> bytes2human(10000)
    '9.8 K'
    >>> bytes2human(100001221)
    '95.4 M'
    """
    symbols = ('K', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y')
    prefix = {}
    for i, s in enumerate(symbols):
        prefix[s] = 1 << (i + 1) * 10
    for s in reversed(symbols):
        if n >= prefix[s]:
            value = float(n) / prefix[s]
            return '%.2f %s' % (value, s)
    return '%.2f B' % (n)

def convert_to_gbit(value):
    return value/1024./1024./1024.

# end def
  
#
# Routine to add commas to a float string
#
def commify3(amount):
    amount = str(amount)
    amount = amount[::-1]
    amount = re.sub(r"(\d\d\d)(?=\d)(?!\d*\.)", r"\1,", amount)
    return amount[::-1]
# end def commify3(amount):

# Save statistics into the database
def savetodatabase():
        # to inserting data into sql server database table
    try:
        #print("Connecting db server")
        conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=18.142.121.40;'
                      'Database=IDTPReportDB;'
                      'UID=sa; PWD=Techvision123@;')
        #print("opening a connection cursor")
        cursor = conn.cursor()
        #print("Executing sql")
        #cursor.execute
        cursor.execute('''
        INSERT INTO MachinePerfData ([TransactionDate],[MachineName],[MachineIP],
        [CPU_Total],[CPU_Used],[CPU_Used_Percent],
        [Memory_Total],[Memory_Used],[Memory_Used_Percent],
        [Disk_Total],[Disk_Used],[Disk_Used_Percent],
        [Network_Total],[Network_Used],[Network_Used_Percent])
        VALUES
        (
        '''+"'"+str(datetime.datetime.now())+"'"+''',
        '''+"'"+host_name+"'"+''',
        '''+"'"+host_ip+"'"+''',
        '''+str(cpu_total_logical)+''',
        '''+str(cpu_usage)+''',
        '''+str(cpu_usage_percent)+''',
        '''+str(memory_total)+''',
        '''+str(memory_usage)+''',
        '''+str(memory_usage_percent)+''',
        '''+str(disk_total)+''',
        '''+str(disk_usage)+''',
        '''+str(disk_usage_percent)+''',
        '''+str(network_total)+''',
        '''+str(network_usage)+''',
        '''+str(network_usage_percent)+'''
        )''')

        conn.commit()
        #print("Transaction Commited")
 
    except pyodbc.Error as error:
        #print("Failed Connecting DB Server")
        print(error)
        return False
        # function returns false value
        # after data interruption
 
    else:
        #print("DB Connection is closing")
        conn.close()
        # closing the connection after the
        # communication with the server is completed
        return True



# ===================
# Main Python section
# ===================
  
if __name__ == '__main__':    
    while True:       
        # infinite loop, as we are monetering
        # the network connection till the machine runs

        #Host Name & IP
        host_name = socket.gethostname()
        host_ip = socket.gethostbyname(host_name)
        #print("Hostname :  ",host_name)
        #print("IP : ",host_ip)

        #CPU Statistics
    
        # gives a physical CPU count
        cpu_total_Core=psutil.cpu_count(logical=False)
        #print(" Total Core CPU: %15s " % cpu_total_Core)

        # gives a logical CPU count
        cpu_total_logical=psutil.cpu_count()
        #print(" Total Logical CPU: %15s " % cpu_total_logical)

        l1, l2, l3 = psutil.getloadavg()
        cpu_usage = (l3/psutil.cpu_count()) * 100
        
        #print(" Total CPU Usage: %15s " % cpu_usage)

        # gives a single float value  for Overall CPU
        cpu_usage_percent=psutil.cpu_percent(interval=None)
        #print(" Total CPU Usage Percent: %15s %s" % (cpu_usage_percent, "%"))
        
        # gives a float value for each CPU
        cpu_all_percent=psutil.cpu_percent(interval=None, percpu=True) 

        #for i, value in enumerate(cpu_all_percent):
        #    print("CPU#: %2i , Percent Used: %.2f %s" % (i,value,"%") )


        # View system memory
        # gives an object with many fields
        mem = psutil.virtual_memory()

        #System total memory
        memory_total = convert_to_gbit(mem.total)   
        #print(" Total Memory: %15s " % memory_total)
        
        #System has been used
        memory_usage=convert_to_gbit(mem.used)
        #print (memory_usage)
        #print(" Total Memory Used: %15s " % memory_usage) 

        # you can have the percentage of used RAM
        memory_usage_percent=psutil.virtual_memory().percent
        #print(" Total Memory Used Percent: %.2f %s" % (memory_usage_percent, "%"))

        #System free memory
        #print(" Total Memory Free: %15s " % bytes2human(mem.free))

        # you can calculate percentage of free memory
        memory_free_percent=psutil.virtual_memory().free * 100.00 / psutil.virtual_memory().total
        #print(" Total Memory Free Percent: %.2f %s" % (memory_free_percent, "%"))
        
        #Disks 

        #total, used and free
        diskcounts=psutil.disk_usage('/')
        
        #total disk space
        disk_total=convert_to_gbit(diskcounts.total)
        # print(" Total Disk Space: %15s " % disk_total)
        disk_usage = convert_to_gbit(diskcounts.used)
        # print(" Total Disk Used: %15s " % disk_usage)
        disk_free=(diskcounts.free)
        # print(" Total Disk Free: %15s " % disk_free)
        disk_usage_percent=diskcounts.percent
        # print(" Total Disk Percent: %15s %s" % (disk_usage_percent,"%"))

        #Read disk parameters
        diskio=psutil.disk_io_counters()
        diskio_total_write=(diskio.write_bytes)
        # print(" Total Disk Write: %15s " % diskio_total_write)
        diskio_total_read=(diskio.read_bytes)
        # print(" Total Disk Read: %15s " % diskio_total_read)      

        #Network Statistics

        # Before
        tot_before = psutil.net_io_counters()
        pnic_before = psutil.net_io_counters(pernic=True)
    
        # sleep some interval so we can compute rates
        interval = 0.2;
        time.sleep(interval)
        
        tot_after = psutil.net_io_counters()
        pnic_after = psutil.net_io_counters(pernic=True)
        
        network_total=convert_to_gbit(tot_after.bytes_sent+tot_after.bytes_recv)
        # print ("Total Network Bandwidth: %15s" % network_total)        
        # print ("Total Network Bandwidth Usage: %15s" % network_usage)
        # start output:
        # print ("Network Statistics total bytes:");
        # print ("   sent: %-10s" % (bytes2human(tot_after.bytes_sent)));
        # print ("   recv: %-10s" % (bytes2human(tot_after.bytes_recv)));
        
        network_usage= '%.6f' % ((tot_after.bytes_recv - 
        tot_before.bytes_recv)/1024/1024/1024)


        network_usage_percent =  ((tot_after.bytes_recv - 
        tot_before.bytes_recv)/(tot_after.bytes_sent+tot_after.bytes_recv)*100)

        #print(network_usage_percent)
        #print ("Total Network Bandwidth Percent: %.10f" % (network_usage_percent/1024/1024/1024))           
        #print ("Total Network Bandwidth Percent: %.2f" % (network_usage_percent))


        nic_names = list(pnic_after.keys())
        nic_names.sort(key=lambda x: sum(pnic_after[x]), reverse=True)
        # print ("Interface:")
        for name in nic_names:
            stats_before = pnic_before[name]
            stats_after = pnic_after[name]

            # print (name);
            # print ("   network Bytes-sent: %15s (total)  %15s  (Per-Sec)" %
            #     (bytes2human(stats_after.bytes_sent), 
            #     bytes2human(stats_after.bytes_sent - 
            #     stats_before.bytes_sent) + '/s'  ) );
            # print ("   network Bytes-recv: %15s (total)  %15s  (Per-Sec)" %
            #     (bytes2human(stats_after.bytes_recv), 
            #     bytes2human(stats_after.bytes_recv - 
            #     stats_before.bytes_recv) + '/s'  ) );
            # print ("   network pkts-sent:  %15s (total)  %15s  (Per-Sec)" %
            #     (commify3(stats_after.packets_sent), 
            #     commify3(stats_after.packets_sent - 
            #     stats_before.packets_sent) + '/s'  ) );
            # print ("   network pkts-recv:  %15s (total)  %15s  (Per-Sec)" %
            #     (commify3(stats_after.packets_recv), 
            #     commify3(stats_after.packets_recv - 
            #     stats_before.packets_recv) + '/s'  ) );
        # end for

        savetodatabase()

# end main



    



