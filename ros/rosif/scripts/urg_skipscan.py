#!/usr/bin/env python
# -*- coding: utf-8 -*-

import rospy
import string
import math
import sys

#from time import time
from sensor_msgs.msg import LaserScan
#from geometry_msgs.msg import Twist
#from visualization_msgs.msg import Marker

def cb_LaserScan(dt):
	global pub
	#rvsURG = LaserScan()
	dt.angle_increment = dt.angle_increment * 2
	dt.ranges = dt.ranges[0::2]
	dt.intensities = dt.intensities[0::2]
	pub.publish(dt)

rospy.init_node("urg_skip_node")
pub = rospy.Publisher('/skip_scan', LaserScan, queue_size=1)
rospy.Subscriber('/scan', LaserScan, cb_LaserScan)

rospy.loginfo("Start UrgScanSkip  ...")

if __name__ == '__main__':
	rospy.spin()
