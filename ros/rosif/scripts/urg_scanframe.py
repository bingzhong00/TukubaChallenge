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
	dt.header.frame_id = "laser_rear"
	pub.publish(dt)

rospy.init_node("urg_frame_node")
pub = rospy.Publisher('/base_scan2', LaserScan, queue_size=1)
rospy.Subscriber('/scan_rear', LaserScan, cb_LaserScan)

rospy.loginfo("Start UrgScanFrameRear  ...")

if __name__ == '__main__':
	rospy.spin()
