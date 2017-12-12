#!/usr/bin/env python

import rospy
import string
import math
import sys

#from time import time
from sensor_msgs.msg import Imu


rospy.init_node("timetest_node3")
#We only care about the most recent measurement, i.e. queue_size=1
pub = rospy.Publisher('imutest', Imu, queue_size=1)

imuMsg = Imu()
seq = 0

while not rospy.is_shutdown():
    imuMsg.header.stamp= rospy.Time.now()
    imuMsg.header.frame_id = 'base_imu_link'
    imuMsg.header.seq = seq
    seq = seq + 1
    pub.publish(imuMsg)
    rospy.loginfo("rospy.Time.now():%s", rospy.Time.now()) 
    rospy.sleep(0.1)

