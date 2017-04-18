#!/usr/bin/env python
# -*- coding: utf-8 -*-
import time
import rospy
import string
import math
import sys
import serial
import binascii
import struct
from numpy import *

from std_msgs.msg import String
from geometry_msgs.msg import Vector3
from sensor_msgs.msg import Imu
from sensor_msgs.msg import Joy
from geometry_msgs.msg import Twist

# lsusb または ls /dev/input コマンドで確認のこと
default_port='/dev/ttyUSB0'
port = rospy.get_param('~port', default_port)

# Check your COM port and baud rate
print "Opening...", port

try:
	ser = serial.Serial(port=port, baudrate=57600, timeout=1)
except serial.serialutil.SerialException:
	rospy.logerr("USB Serial not found at port "+port + ". Did you specify the correct port in the launch file?")
	exit
#    sys.exit(0)


RUN_HZ = 20

joyX = 0.0
joyY = 0.0
cmdHandle = 0.0
cmdAccel = 0.0

joyStopCnt = 0
cmdStopCnt = 0

#-------------------------------------------------------
# ジョイスティック受信処理
#-------------------------------------------------------
def joy_callback( data ):
	global joyX, joyY
	global joyStopCnt
	global RUN_HZ
	# axes 0,1 左スティック　左右、上下
	# axes 2,3 右スティック　左右、上下
	joyX = data.axes[0]
	joyY = data.axes[3]
	joyStopCnt = RUN_HZ*3

#-------------------------------------------------------
# cmdvel受信処理
#-------------------------------------------------------
def cmdvel_callback( data ):
	global cmdHandle, cmdAccel
	global cmdStopCnt
	global RUN_HZ
	# angular Z Handle
	cmdHandle = data.angular.z
	# linear X Accel
	cmdAccel = data.linear.x
	cmdStopCnt = RUN_HZ/2


rospy.init_node("comm_sh2")
pub = rospy.Publisher('sh2_encLR', Vector3, queue_size=RUN_HZ)
pubD = rospy.Publisher('sh2_encLR_Direct', Vector3, queue_size=RUN_HZ)

# joystick subscriber
rospy.Subscriber("/joy", Joy, joy_callback)
rospy.Subscriber("/cmd_vel", Twist, cmdvel_callback)


#-------------------------------------------------------
# メイン処理
#-------------------------------------------------------
def main():
	global joyX, joyY
	global cmdHandle, cmdAccel
	global cmdStopCnt
	global joyStopCnt
	global RUN_HZ
	
	vecMsg = Vector3()
	vecDMsg = Vector3()
	sendByte = [0,0,0,0,0,0]
	r = rospy.Rate(RUN_HZ)
	encR = 0
	encL = 0
	encRold = 0
	encLold = 0
	encFirstFlg = True
	light = False
	
	while not rospy.is_shutdown():
		# Subscribe Error Accel Stop
		if (cmdStopCnt<=0):
			cmdAccel = 0
			cmdHandle = 0
		else:
			cmdStopCnt = cmdStopCnt - 1

		if (joyStopCnt<=0):
			joyX = 0
			joyY = 0
		else:
			joyStopCnt = joyStopCnt - 1
		
		# JoyStick & cmdVel to Motor Serbo Order
		accVal = joyY + cmdAccel
		hdlVal = -joyX + cmdHandle

		print "joyX(Handle):{0:6.2f} joyY(Accel):{1:6.2f}".format(joyX,joyY)
		print "cmdHandle:{0:6.2f} cmdAccel:{1:6.2f} ".format(cmdHandle,cmdAccel)

		handle = max(-1.0, min(1.0, hdlVal))# Left = 1.0
		#handle = int(interp(handle, (-1.0,1.0),(1280., 3328.)))
		handle = int(interp(handle, (-1.0,1.0),(1700., 2700.)))
		throttle = max(-1.0, min(1.0, accVal))
		#throttle = int(interp(throttle, (-1.0,1.0),(1280., 3328.)))
		throttle = int(interp(throttle, (-1.0,1.0),(1250., 3400.)))
		data = struct.pack('>HHB', handle, throttle, light)
		sum = reduce(int.__xor__, [ord(c) for c in data])
		
		# SH2 Serial Write
		ser.write(data+chr(sum))
		# Sleep
		r.sleep()
		# SH2 Serial Read
		reply = ser.read(10)
		
		if reply\
			and len(reply)==10\
			and reduce(int.__xor__, [ord(c) for c in reply])==0:
				# cersio original
				#cnt = fromstring(reply[:4], int16).byteswap()
				#pulse_delta = (cnt - self.last_count)*array((-1,1))
				#self.tire += pulse_delta
				#self.angle = (self.tire[0]-self.tire[1])*self.K2
				#dist = average(pulse_delta)*self.K1
				#self.position += array((
				#	sin(self.angle),
				#	0.0,
				#	cos(self.angle)))*dist
				#self.linear_delta = pulse_delta*self.K1
				#self.last_count = cnt
				#
				#results = struct.unpack('BBBBB', reply[4:9])
				#self.power12v  = interp(results[0],(0, 200),(0.0, 1.0))
				#self.power6v = interp(results[1],(0, 120),(0.0, 1.0))
				#self.psd_left  = interp(results[2],(150, 0),(0.0, 1.0))
				#self.psd_right = interp(results[3],(150, 0),(0.0, 1.0))
				#left_button = results[4]&0x08==0
				#right_button = results[4]&0x10==0
				#yellow_button = results[4]&0x04==0
				#blue_button = results[4]&0x02==0
				#self.left_down = not self.left_button and left_button
				#self.right_down = not self.right_button and right_button
				#self.yellow_down = not self.yellow_button and yellow_button
				#self.blue_down = not self.blue_button and blue_button
				#self.left_button = left_button
				#self.right_button = right_button
				#self.yellow_button = yellow_button
				#self.blue_button = blue_button

				# RotaryEncorder
				encLR = fromstring(reply[:4], int16).byteswap()
				encL = encLR[0]
				encR = encLR[1]

				if encFirstFlg:
					encLold = encL
					encRold = encR
					encFirstFlg = False
			
				if(abs(encR-encRold) > 65000):
					if(encRold > 65000):
						encRold -= 65536
					else:
						encRold += 65536

				if(abs(encL-encLold) > 65000):
					if(encLold > 65000):
						encLold -= 65536
					else:
						encLold += 65536

				vecMsg.x = -float(encR-encRold)
				vecMsg.y = float(encL-encLold)
				vecMsg.z = 0.0
				
				vecDMsg.x = -float(encR)
				vecDMsg.y = float(encL)
				vecDMsg.z = 0.0

				encRold = encR
				encLold = encL

				# res[4], res[5] Voltage
				# res[6], res[7] IR Sensor
				# res[8] Switch bit
				print "serial:encR" ,-encR,",encL",encL

				pub.publish(vecMsg)
				pubD.publish(vecDMsg)
				#r.sleep()
		else:
			print "serial no data"

if __name__ == "__main__":
	main()


