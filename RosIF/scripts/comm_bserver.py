#!/usr/bin/env python
# -*- coding: utf-8 -*-

import rospy
import socket
from geometry_msgs.msg import Twist
from geometry_msgs.msg import Vector3
from geometry_msgs.msg import PoseStamped
from tf.transformations import quaternion_from_euler

import bserver

bsrv = bserver.Bserver()
bsrv.start()
bsrv.set_handle_throttle(0.0, 0.0)

rospy.init_node('rosif_bserver')


def odom_callback(data):
	global bsrv
	bsrv.set_encorderdt(data.x,data.y)


def baselink_callback(data):
	global bsrv
	bsrv.set_globalxyr( data.linear.x, data.linear.y, data.angular.z )

def cmdvelmovebase_callback(data):
	global bsrv
	bsrv.set_movebase( data.linear.x, data.linear.y, data.angular.z )

def run():
	pub = rospy.Publisher('/cmd_vel', Twist, queue_size=40)
	pubGoal = rospy.Publisher('/move_base_simple/goal', PoseStamped, queue_size=40)
	rospy.Subscriber("/sh2_encLR", Vector3, odom_callback)
	rospy.Subscriber("/rosif/base_link", Twist, baselink_callback)
	rospy.Subscriber("/cmd_vec_movebase", Twist, cmdvelmovebase_callback)

	rospy.loginfo("Start bserver")

	rate = rospy.Rate(40.0)
	cmd = Twist()
	seq = 0
	
	while not rospy.is_shutdown():
		ht = bsrv.get_handle_throttle()
		
		#self.bsrv.set_encorderdt(self.controller.tire[0],self.controller.tire[1])
		#self.bsrv.set_globalxyr(self.controller.posxyr.gXpos,self.controller.posxyr.gYpos,self.controller.posxyr.grad)

		# self.handle = ht[0]
		# self.throttle = ht[1]
		
		# publish cmd_vel
		cmd.linear.x = ht[1]
		cmd.linear.y = 0
		cmd.linear.z = 0
		cmd.angular.x = 0
		cmd.angular.y = 0
		cmd.angular.z = ht[0]
		pub.publish(cmd)
		
		if bsrv.get_checkpoint_flg() == 1:
			cp = bsrv.get_checkpoint()
			goalMsg = PoseStamped()
			goalMsg.pose.position.x = cp[0]
			goalMsg.pose.position.y = cp[1]
			goalMsg.pose.position.z = 0.0
			q = quaternion_from_euler(0.0,0.0,cp[2])
			goalMsg.pose.orientation.x = q[0]
			goalMsg.pose.orientation.y = q[1]
			goalMsg.pose.orientation.z = q[2]
			goalMsg.pose.orientation.w = q[3]
			goalMsg.header.stamp= rospy.Time.now()
			goalMsg.header.frame_id = 'base_imu_link'
			goalMsg.header.seq = seq
			seq = seq + 1
			pubGoal.publish(goalMsg)
 
		
		rate.sleep()

if __name__ == '__main__':
	run()
