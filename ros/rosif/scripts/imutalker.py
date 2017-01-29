#!/usr/bin/env python
# -*- coding: utf-8 -*-

import rospy
import string
import math
import sys

#from time import time
#from sensor_msgs.msg import LaserScan
from geometry_msgs.msg import Twist
from geometry_msgs.msg import Vector3
#from visualization_msgs.msg import Marker
from sensor_msgs.msg import Imu
from tf.transformations import quaternion_from_euler
from dynamic_reconfigure.server import Server
#from razor_imu_9dof.cfg import imuConfig
from diagnostic_msgs.msg import DiagnosticArray, DiagnosticStatus, KeyValue

degrees2rad = math.pi/180.0
imu_yaw_calibration = 0
imu_roll_calibration = 0
imu_pitch_calibration = 0

#各センサ値表示
x_acc = 0.0
y_acc = 0.0
z_acc = 0.0
x_jyr = 0.0
y_jyr = 0.0
z_jyr = 0.0
x_cmp = 0.0
y_cmp = 0.0
z_cmp = 0.0


imuMsg = Imu()

#rospy.loginfo("Opening %s...", port)
roll=0
pitch=0
yaw=0
seq=0
accel_factor = 9.806 / 256.0    # sensor reports accel as 256.0 = 1G (9.8m/s^2). Convert to m/s^2.

def cb_SensorAcc(dt):
	global x_acc,y_acc,z_acc
	x_acc = dt.linear.x
	y_acc = dt.linear.y
	z_acc = dt.linear.z

def cb_SensorJyr(dt):
	global x_jyr,y_jyr,z_jyr
	x_jyr = dt.x
	y_jyr = dt.y
	z_jyr = dt.z

def cb_SensorCmp(dt):
	global x_acc,y_acc,z_acc
	x_cmp = dt.x
	y_cmp = dt.y
	z_cmp = dt.z

rospy.init_node("imutalker_node")
#We only care about the most recent measurement, i.e. queue_size=1
#pub = rospy.Publisher('/torosif/scan', LaserScan, queue_size=1)
pub = rospy.Publisher('imu', Imu, queue_size=1)
#srv = Server(imuConfig, reconfig_callback)  # define dynamic_reconfigure callback
diag_pub = rospy.Publisher('diagnostics', DiagnosticArray, queue_size=1)
diag_pub_time = rospy.get_time();
subAcc = rospy.Subscriber('/Sacc', Twist, cb_SensorAcc)
subJyr = rospy.Subscriber('/Sjyr', Vector3, cb_SensorJyr)
subCmp = rospy.Subscriber('/Scmp', Vector3, cb_SensorCmp)

seq=0
rospy.loginfo("Start IMUSensor to Rosif ...")

while not rospy.is_shutdown():
    # Publish message
    # AHRS firmware accelerations are negated
    # This means y and z are correct for ROS, but x needs reversing
    imuMsg.linear_acceleration.x = -float(x_acc) * accel_factor
    imuMsg.linear_acceleration.y = float(y_acc) * accel_factor
    imuMsg.linear_acceleration.z = float(z_acc) * accel_factor

    imuMsg.angular_velocity.x = float(x_jyr)
    #in AHRS firmware y axis points right, in ROS y axis points left (see REP 103)
    imuMsg.angular_velocity.y = -float(y_jyr)
    #in AHRS firmware z axis points down, in ROS z axis points up (see REP 103) 
    imuMsg.angular_velocity.z = -float(z_jyr)
    
    #in AHRS firmware y axis points right, in ROS y axis points left (see REP 103)
    roll = math.atan2(y_acc, z_acc)
    roll = roll + imu_roll_calibration
    pitch = -math.atan2(- x_acc, (y_acc * math.sin(roll) + z_acc * math.cos(roll)))
    pitch = pitch + imu_pitch_calibration
    #in AHRS firmware z axis points down, in ROS z axis points up (see REP 103)
    #yaw = math.atan2((float(z_cmp) * math.sin(roll) - float(y_cmp) * math.cos(roll)), (float(x_cmp) * math.cos(pitch) + float(y_cmp) * math.sin(pitch) * math.sin(roll) + float(z_cmp) * math.sin(pitch) * math.cos(roll)))
    yaw = math.atan2((-float(z_cmp) * math.sin(roll) - float(x_cmp) * math.cos(roll)), (float(y_cmp) * math.cos(pitch) + float(x_cmp) * math.sin(pitch) * math.sin(roll) - float(z_cmp) * math.sin(pitch) * math.cos(roll)))
    yaw = yaw + imu_yaw_calibration
    if yaw > math.pi:
       yaw = yaw - 2 * math.pi
    if yaw < -math.pi:
       yaw = yaw + 2 * math.pi
    roll_deg = roll / degrees2rad
    pitch_deg = pitch / degrees2rad
    yaw_deg = yaw / degrees2rad

    #print 'roll = rad %f' % roll, ', pitch = rad %f' % pitch, ', yaw = rad %f' % yaw
    #print 'roll_deg = deg %f' % roll_deg, ', pitch_deg = deg %f' % pitch_deg, ', yaw_deg = deg %f' % yaw_deg
    #    # Publish message
    #    # AHRS firmware accelerations are negated
    #    # This means y and z are correct for ROS, but x needs reversing
    #    imuMsg.linear_acceleration.x = -float(words[3]) * accel_factor
    #    imuMsg.linear_acceleration.y = float(words[4]) * accel_factor
    #    imuMsg.linear_acceleration.z = float(words[5]) * accel_factor

    #    imuMsg.angular_velocity.x = float(words[6])
    #    #in AHRS firmware y axis points right, in ROS y axis points left (see REP 103)
    #    imuMsg.angular_velocity.y = -float(words[7])
    #    #in AHRS firmware z axis points down, in ROS z axis points up (see REP 103) 
    #    imuMsg.angular_velocity.z = -float(words[8])

    q = quaternion_from_euler(roll,pitch,yaw)
    imuMsg.orientation.x = q[0]
    imuMsg.orientation.y = q[1]
    imuMsg.orientation.z = q[2]
    imuMsg.orientation.w = q[3]
    imuMsg.header.stamp= rospy.Time.now()
    imuMsg.header.frame_id = 'base_imu_link'
    imuMsg.header.seq = seq
    seq = seq + 1
    pub.publish(imuMsg)

    if (diag_pub_time < rospy.get_time()) :
        diag_pub_time += 1
        diag_arr = DiagnosticArray()
        diag_arr.header.stamp = rospy.get_rostime()
        diag_arr.header.frame_id = '1'
        diag_msg = DiagnosticStatus()
        diag_msg.name = 'Razor_Imu'
        diag_msg.level = DiagnosticStatus.OK
        diag_msg.message = 'Received AHRS measurement'
        diag_msg.values.append(KeyValue('roll (deg)',
                                str(roll*(180.0/math.pi))))
        diag_msg.values.append(KeyValue('pitch (deg)',
                                str(pitch*(180.0/math.pi))))
        diag_msg.values.append(KeyValue('yaw (deg)',
                                str(yaw*(180.0/math.pi))))
        diag_msg.values.append(KeyValue('sequence number', str(seq)))
        diag_arr.status.append(diag_msg)
        diag_pub.publish(diag_arr)

    rospy.sleep(0.1)
