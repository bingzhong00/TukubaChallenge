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

joyX = 0.0
joyY = 0.0

#-------------------------------------------------------
# ジョイスティック受信処理
#-------------------------------------------------------
def joy_callback( data ):
	global joyX, joyY
	# axes 0,1 左スティック　左右、上下
	# axes 2,3 右スティック　左右、上下
	joyX = data.axes[0]
	joyY = data.axes[3]


rospy.init_node("comm_sh2")
pub = rospy.Publisher('sh2_encLR', Vector3, queue_size=10)

# joystick subscriber
rospy.Subscriber("/joy", Joy, joy_callback)


#-------------------------------------------------------
# メイン処理
#-------------------------------------------------------
def main():
	global joyX, joyY
	vecMsg = Vector3()
	sendByte = [0,0,0,0,0,0]
	r = rospy.Rate(10)
	encR = 0
	encL = 0
	encRold = 0
	encLold = 0
	encFirstFlg = True
	light = False
	
	while not rospy.is_shutdown():
		accVal = joyY
		hdlVal = joyX

		print "joyX:",joyX, " joyY:",joyY

		handle = max(-1.0, min(1.0, hdlVal))# Left = 1.0
		handle = int(interp(handle, (-1.0,1.0),(1280., 3328.)))
		throttle = max(-1.0, min(1.0, accVal))
		throttle = int(interp(throttle, (-1.0,1.0),(1280., 3328.)))
		data = struct.pack('>HHB', handle, throttle, light)
		sum = reduce(int.__xor__, [ord(c) for c in data])
		
		ser.write(data+chr(sum))
		r.sleep()
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

				encRold = encR
				encLold = encL

				# res[4], res[5] Voltage
				# res[6], res[7] IR Sensor
				# res[8] Switch bit
				print "serial:encR" ,encR,",encL",encL

				pub.publish(vecMsg)
				#r.sleep()
		else:
			print "serial no data"

#		accSH = int(1280 + ((accVal + 1.0) / 2.0) * 2048)
#		hdlSH = int(1280 + ((hdlVal + 1.0) / 2.0) * 2048)
#		# Handle accel
#		sendByte[0] = ((hdlSH>>8) & 0xFF)
#		sendByte[1] = (hdlSH & 0xFF)
#		sendByte[2] = ((accSH>>8) & 0xFF)
#		sendByte[3] = (accSH & 0xFF)
#		# IO
#		sendByte[4] = 0
#		# checkSum
#		sumcheck = 0
#		for i in range(5):
#			sumcheck = (sumcheck&0xFF) ^ (sendByte[i-1]&0xFF)
#		sendByte[5] = sumcheck
#
#		print "hdlSH:",hdlSH, " accSH:",accSH
#		print "sendByte:",sendByte[0],sendByte[1],sendByte[2],sendByte[3],sendByte[5]
#
#		# send serial
#		for i in range(6):
#			ser.write(chr(sendByte[i-1]))
#
#		print "joyX:",joyX, " joyY:",joyY
#		# sleep
#		r.sleep()
#				
#		# receive serial
#		res = ser.read(10)
#		
#		if len(res) > 8:
#			# RotaryEncorder
#			encL = ord(res[0]) << 8 | ord(res[1])
#			encR = ord(res[2]) << 8 | ord(res[3])
#
#			if encFirstFlg:
#				encLold = encL
#				encRold = encR
#				encFirstFlg = False
#			
#			if(abs(encR-encRold) > 65000):
#				if(encRold > 65000):
#					encRold -= 65536
#				else:
#					encRold += 65536
#
#			if(abs(encL-encLold) > 65000):
#				if(encLold > 65000):
#					encLold -= 65536
#				else:
#					encLold += 65536
#
#			vecMsg.x = -float(encR-encRold)
#			vecMsg.y = float(encL-encLold)
#			vecMsg.z = 0.0
#
#			encRold = encR
#			encLold = encL
#
#			# res[4], res[5] IR Sensor
#			# res[6], res[7] Voltage
#			# res[8] Switch bit
#			print "serial:encR" ,encR,",encL",encL, " IR:",ord(res[4]),ord(res[5]), " volt:",ord(res[6]), ord(res[7]), " bit:",ord(res[8])
#
#			pub.publish(vecMsg)
#			#r.sleep()
#		else:
#			print "serial no data"

if __name__ == "__main__":
	main()


