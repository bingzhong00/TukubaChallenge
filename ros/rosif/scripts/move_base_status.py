#!/usr/bin/env python
# -*- coding: utf-8 -*-

import rospy
import string
import math
import sys

#from time import time
#from sensor_msgs.msg import LaserScan
#from geometry_msgs.msg import Twist
#from visualization_msgs.msg import Marker
from actionlib_msgs.msg import GoalStatusArray
from actionlib_msgs.msg import GoalStatus
from actionlib_msgs.msg import GoalID
from geometry_msgs.msg import Twist
from std_msgs.msg import *

goal_id = ""
goal_status = 3

def cmdvel_callback(data):
	global goal_id
	global movebase_cancel
	if goal_status == 1 and data.linear.x == 0.0 and abs(data.angular.z) > 0.0:
		#goalCancel = GoalID()
		#goalCancel.stamp = rospy.Time.now()
		#goalCancel.id = goal_id
		movebase_cancel.publish(goal_id)
		rospy.loginfo("move_base recovery cancel!")

def cb_GoalStatus(dt):
	global goal_id
	global goal_status
	global movebase_status
	if len(dt.status_list) > 0:
		endidx = len(dt.status_list) - 1
		goalStatus = dt.status_list[endidx]
		goal_id = goalStatus.goal_id
		goal_status = goalStatus.status
		rospy.loginfo("status  ..." + str(goalStatus.status) )
		movebase_status.publish(goalStatus.status)

rospy.init_node("move_base_status")
rospy.Subscriber("/move_base/status", GoalStatusArray, cb_GoalStatus)
rospy.Subscriber("/move_base/cmd_vel", Twist, cmdvel_callback)
movebase_cancel = rospy.Publisher("/move_base/cancel", GoalID, queue_size=10)
movebase_status = rospy.Publisher("/rosif/move_base/status", Int64, queue_size=10)

#rospy.loginfo("Start UrgScanReverse  ...")

if __name__ == '__main__':
	rospy.spin()
