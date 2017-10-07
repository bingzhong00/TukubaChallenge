#!/usr/bin/env python  
# -*- coding: utf-8 -*-
import roslib
#roslib.load_manifest('learning_tf')
import rospy
import math
import tf
import geometry_msgs.msg
#import turtlesim.srv
from geometry_msgs.msg import Twist
from std_msgs.msg import *

onePluseLength = 0.15 * math.pi # タイヤ1周の長さ (m)
carWidth = 0.45 # シャーシ幅
carTurnLength = 4.0 #2.0 # 車の回転半径
cmdvel_ang = 0.0
cmdvel_move = 0.0
moveR = 0.0
moveL = 0.0

oneSecMove = ((4.0*1000.0 * 120.0) / (60.0*60.0))#((4.0*1000.0) / (60.0*60.0))


def cmdvel_callback(data):
	global cmdvel_move,cmdvel_ang

	cmdvel_move = data.linear.x
	cmdvel_ang = data.angular.z


rospy.init_node('odomemu')

#rospy.wait_for_service('spawn')
#spawner = rospy.ServiceProxy('spawn', turtlesim.srv.Spawn)
#spawner(4, 2, 0, 'turtle2')

rospy.Subscriber("/cmd_vel", Twist, cmdvel_callback)
pubEncLR = rospy.Publisher("/sh2_encLR", geometry_msgs.msg.Vector3, queue_size=40)
pubBenzEncR = rospy.Publisher("/benz/raspi/encR", Int64, queue_size=40)
pubBenzEncL = rospy.Publisher("/benz/raspi/encL", Int64, queue_size=40)
moveVec = geometry_msgs.msg.Vector3()
encL = Int64()
encR = Int64()

if __name__ == '__main__':
	# 
	while not rospy.is_shutdown():

		#ハンドル操作による差分
		dirdiff = 1.0 - (abs(cmdvel_ang)*((carTurnLength-carWidth)/carTurnLength))
		if dirdiff < 0.8:
			dirdiff = 0.8

		moveVel = cmdvel_move * oneSecMove * 0.1 # 100msの移動量
		diffR = 1.0
		diffL = 1.0
		if cmdvel_ang < 0.0:
			diffL = dirdiff
		elif cmdvel_ang > 0.0:
			diffR = dirdiff

		moveR += moveVel * diffR 
		moveL += moveVel * diffL

		# 移動量からREパルスに変更
		moveVec.x = 0.0
		moveVec.y = 0.0
		encL = 0
		encR = 0

		if abs(moveR) > onePluseLength:
			numPulse = round(moveR / onePluseLength)
			moveR = moveR - (numPulse * onePluseLength)
			moveVec.x = numPulse
			encL = numPulse
		
		if abs(moveL) > onePluseLength:
			numPulse = round(moveL / onePluseLength)
			moveL = moveL - (numPulse * onePluseLength)
			moveVec.y = numPulse
			encR = numPulse

		pubBenzEncL.publish(encL)
		pubBenzEncR.publish(encR)
		pubEncLR.publish(moveVec)
		
		rospy.sleep(0.1)

