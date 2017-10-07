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
	rvsURG = LaserScan()
	dt.ranges = tuple(reversed( dt.ranges ))
	dt.intensities = tuple(reversed( dt.intensities ))
	pub.publish(dt)

rospy.init_node("urg_reverse_node")
pub = rospy.Publisher('/scan', LaserScan, queue_size=1)
rospy.Subscriber('/reverse/scan', LaserScan, cb_LaserScan)

rospy.loginfo("Start UrgScanReverse  ...")

if __name__ == '__main__':
	rospy.spin()
