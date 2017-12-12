#!/usr/bin/env python
# -*- coding: utf-8 -*-

import rospy
import socket
from geometry_msgs.msg import Twist
from geometry_msgs.msg import Vector3


bsrv = bserver.Bserver()
bsrv.start()
bsrv.set_handle_throttle(0.0, 0.0)

rospy.init_node('rosif_bserver')

pub = rospy.Publisher('rosif/cmd_vel', Twist, queue_size=40)
rospy.loginfo("Start bserver")

rospy.Subscriber("/sh2_encLR", Vector3, odom_callback)

def odom_callback(data):
	self.bsrv.set_encorderdt(data.x,data.y)

def run():
	rate = rospy.Rate(40.0)
	cmd = geometry_msgs.msg.Twist()
	
	while not rospy.is_shutdown():
		ht = bsrv.get_handle_throttle()
		
		#self.bsrv.set_encorderdt(self.controller.tire[0],self.controller.tire[1])
		#self.bsrv.set_globalxyr(self.controller.posxyr.gXpos,self.controller.posxyr.gYpos,self.controller.posxyr.grad)

		# self.handle = ht[0]
		# self.throttle = ht[1]
		
		# send cmd_vel
		cmd.linear.x = ht[1]
		cmd.linear.y = 0
		cmd.linear.z = 0
		cmd.angular.x = 0
		cmd.angular.y = 0
		cmd.angular.z = ht[0]
		pub.publish(cmd)
		
		r.sleep()
		
	soc.close()

if __name__ == '__main__':
	run()
