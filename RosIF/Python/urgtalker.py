#!/usr/bin/env python

import rospy
import string
import math
import sys

#from time import time
from sensor_msgs.msg import LaserScan
#from geometry_msgs.msg import Twist
#from visualization_msgs.msg import Marker

degrees2rad = math.pi/180.0
rsvURG = LaserScan()

def cb_LaserScan(dt):
	global rsvURG
	rsvURG = dt	

rospy.init_node("urgtalker_node")
#We only care about the most recent measurement, i.e. queue_size=1
pub = rospy.Publisher('/torosif/scan', LaserScan, queue_size=1)
#srv = Server(imuConfig, reconfig_callback)  # define dynamic_reconfigure callback
#diag_pub = rospy.Publisher('diagnostics', DiagnosticArray, queue_size=1)
#diag_pub_time = rospy.get_time();
sub = rospy.Subscriber('/last', LaserScan, cb_LaserScan)

seq=0
rospy.loginfo("Start UrgScan to Rosif ...")

while not rospy.is_shutdown():

    pub.publish(rsvURG)
    rospy.sleep(0.1)
