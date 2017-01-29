#!/usr/bin/env python
# -*- coding: utf-8 -*-
import time
import rospy
import string
import math
import sys
import serial
import binascii

from std_msgs.msg import String
from geometry_msgs.msg import Vector3
from sensor_msgs.msg import Imu
from sensor_msgs.msg import Joy

# lsusb または ls /dev/input コマンドで確認のこと
default_port='/dev/input/js0'
port = rospy.get_param('~port', default_port)

# Check your COM port and baud rate
rospy.loginfo("Opening %s...", port)
#try:
#    ser = serial.Serial(port=port, baudrate=57600, timeout=1)
#except serial.serialutil.SerialException:
#    rospy.logerr("USB Serial not found at port "+port + ". Did you specify the correct port in the launch file?")
    #exit
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
pub = rospy.Publisher('sh2_encLR', Imu, queue_size=10)

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

	while not rospy.is_shutdown():
		accVal = joyY
		hdlVal = joyX
		accSH = int(1280 + ((accVal + 1.0) / 2.0) * 2048)
		hdlSH = int(1280 + ((hdlVal + 1.0) / 2.0) * 2048)
		# Handle accel
		sendByte[0] = ((hdlSH>>8) & 0xFF)
		sendByte[1] = (hdlSH & 0xFF)
		sendByte[2] = ((accSH>>8) & 0xFF)
		sendByte[3] = (accSH & 0xFF)
		# IO
		sendByte[4] = 0
		# checkSum
		sumcheck = 0
		for i in range(5):
			sumcheck = (sumcheck&0xFF) ^ (sendByte[i-1]&0xFF)
		sendByte[5] = sumcheck

		# send serial
		#for i in range(6):
		#	ser.write(chr(sendByte[i-1]))

		print "joyX:",joyX, " joyY:",joyY
		# sleep
		r.sleep()
				
		# receive serial
		#res = ser.read(10)
		
		# RotaryEncorder
		#vecMsg.x = float(res[0] << 8 | res[1])
		#vecMsg.y = float(res[2] << 8 | res[3])
		#vecMsg.z = 0.0
		# res[4], res[5] IR Sensor
		# res[6], res[7] Voltage
		# res[8] Switch bit

		#imuMsg.header.stamp= rospy.Time.now()
		#imuMsg.header.frame_id = 'base_imu_link'
		#imuMsg.header.seq = seq
		#seq = seq + 1
		#pub.publish(vecMsg)


if __name__ == "__main__":
	main()


