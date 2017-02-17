#!/usr/bin/env python
# -*- coding: utf-8 -*-

import rospy
import socket
from geometry_msgs.msg import Twist

rospy.init_node('rosif_pub_cmdvel')

reciveStr = "nodata"
pub = rospy.Publisher('rosif/cmd_vel', Twist, queue_size=10)
rospy.loginfo("Start cmdvel to Ros ...")

def run_server():
	s = socket.socket( socket.AF_INET, socket.SOCK_STREAM)
	s.setsockopt( socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
	s.bind(("192.168.1.32", 11211))
	s.listen(1)
	
	# for clientMode
	#soc = socket.socket( socket.AF_INET, socket.SOCK_STREAM)
	#soc.connect(("192.168.1.4", 11211))
	rate = rospy.Rate(10.0)
	cmd = geometry_msgs.msg.Twist()
	
	while not rospy.is_shutdown():
		# 接続待ち
		print 'Waiting Connections..'
		soc, addr = s.accept()
		print("Connected by"+str(addr))

		while not rospy.is_shutdown():
			soc.send(reciveStr)
			print "Send>", reciveStr
			try:
				data = soc.recv(1024)
			except:
				print "Recive Disconnect"
				break
				
			print "Client>",data
			if len(data) == 0:
				print "Recive Disconnect"
				break

			cmd.linear.x = trans[0]
			cmd.linear.y = trans[1]
			cmd.linear.z = trans[2]
			cmd.angular.x = eul[0]
			cmd.angular.y = eul[1]
			cmd.angular.z = eul[2]
			pub.publish(cmd)
			r.sleep()
		soc.close()


if __name__ == '__main__':
	run_server()
