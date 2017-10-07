#!/usr/bin/env python
# -*- coding: utf-8 -*-
import roslib; #roslib.load_manifest('wheel_odometry')
import rospy
import time
from std_msgs.msg import *
from nav_msgs.msg import Odometry
from geometry_msgs.msg import TransformStamped
from numpy import *
from tf.transformations import quaternion_from_euler
import tf

#=========Initial Parameter=============
D_RIGHT=1.200 #0.245462	# 右パルスの移動量 m? Pulse ratio of the right wheel encoder
D_LEFT =1.198 #0.243768 	# 左パルスの移動量 m? (大きいほど移動量増える)Pulse ratio of the left wheel encoder
GR=1.0 #100.0			# パルス値を割る値1 Gear ratio
PR=100.0 #32.0 			# パルス値を割る値2 Pulse ratio of encoders
TREAD=1.0 #0.25 #1.002405	# 左右の移動量の差から角度に変換する値(小さいほど急激に変化) Tread

rs=[0,0]; #pluse store
u=array([0,0]);
count=0
odocount=0
encL = 0
encR = 0
encLold = 0
encRold = 0
encFirstR = True
encFirstL = True

# tfにも出力
bc = tf.TransformBroadcaster()
listener = tf.TransformListener() 

#result=open('odo.txt','w')

def talker():
	rospy.init_node('infant_odometry', anonymous=True)
	#rospy.Subscriber("/recover/odom", Odometry, odom_callback)
	rospy.Subscriber("/benz/raspi/encR", Int64, odom_callbackR)
	rospy.Subscriber("/benz/raspi/encL", Int64, odom_callbackL)
	pub = rospy.Publisher('/odom', Odometry, queue_size=10 )

	odo=Odometry()
	odo.header.frame_id='odom'
	odo.child_frame_id='base_link'

	current_time=time.time()
	last_time=time.time()

	#======= Parameter Setting with Parameter Server=======
	#	* If the parameter server didn't work, the following sequence would be passed.
	if rospy.has_param('infant_learning_odometry/right_wheel_p'):
		global D_RIGHT
		D_RIGHT=rospy.get_param('infant_learning_odometry/right_wheel_p')
	if rospy.has_param('infant_learning_odometry/left_wheel_p'):
		global D_LEFT
		PRIGHT=rospy.get_param('infant_learning_odometry/left_wheel_p')
	if rospy.has_param('infant_learning_odometry/Tread'):
		global TREAD
		TREAD=rospy.get_param('infant_learning_odometry/Tread')

	#print PRIGHT
	#print PLEFT
	#print TREAD
	#===========================================================


	odo.pose.pose.position.x=0
	odo.pose.pose.orientation.w=0
	odo.twist.twist.linear.x=0
	odo.twist.twist.angular.z=0

	r = rospy.Rate(10) 
	global dt
	global x
	global count
	x=array([0,0,toRadian(0.0)]);
	dt=0.1
	t=0;

	while not rospy.is_shutdown():
		count=count+1
		
		current_time=time.time()

		dt=current_time-last_time

		odo.header.seq=count
		odo.header.stamp = rospy.Time.now()
		odo.pose.pose.position.x=x[0]
		odo.pose.pose.position.y=x[1]
		
		roll=0.0
		pitch=0.0
		yaw = x[2]
		q = quaternion_from_euler(roll,pitch,yaw)
		odo.pose.pose.orientation.x = q[0]
		odo.pose.pose.orientation.y = q[1]
		odo.pose.pose.orientation.z = q[2]
		odo.pose.pose.orientation.w = q[3]
		#odo.pose.pose.orientation.w=x[2]
		odo.twist.twist.linear.x=u[0]
		odo.twist.twist.angular.z=u[1]

		pub.publish(odo)

		# odom tf
		if bc is not None:		
			bc.sendTransform((x[0],x[1],0.0),q,rospy.Time.now(),'base_link','odom')

		if count%2000==0:
			print "%8.2f" %x[0],"%8.2f" %x[1],"%8.2f" %toDegree(x[2])
			#print (last_time-current_time)
			#print t

		#result.write('%f,%f,%f,%f,%f\n' %(x[0],x[1],x[2],u[0],u[1]))

		last_time=time.time()
		t+=dt

		r.sleep()
	rospy.spin()

# u[0] move vec
# u[1] dir 
def calc_input(rs):
	u=array([0,0]);
	vr=rs[0]/(GR*PR)*pi*D_RIGHT
	vl=rs[1]/(GR*PR)*pi*D_LEFT
	v=(vr+vl)/2.0
	yawrate=(vr-vl)/(TREAD/2.0)
	u=array([v,yawrate])
	return u

# x[0] x
# x[1] y
# x[2] dir
def MotionModel(x,u,dt):
	F=eye(3)
	B=array([[dt*cos(x[2]),0],
			[dt*sin(x[2]),0],
			[0,dt]])
		
	x=dot(F,x)+dot(B,u)
	x[2]=PItoPI(x[2])
	return x

# Pi Limit
def PItoPI(angle):
	while angle>=pi:
		angle=angle-2*pi
	while angle<=-pi: 
		angle=angle+2*pi
	return angle

def odom_callback(data):
	global u
	global x
	global dt
	global odocount
	rs=[data.pose.pose.position.x,data.pose.pose.position.y]
	odocount=data.header.seq

	u=calc_input(rs)

	x=MotionModel(x,u,dt)

# --------------------------
def odom_callbackR(data):
	global encR
	global encFirstR
	if data.data < 1000:
		encR = data.data

def odom_callbackL(data):
	global u
	global x
	global dt
	#global odocount
	global encL
	global encFirstL
	global encLold
	global encRold
	global listener

	if data.data < 1000:
		encL = data.data

		rs=[float(encR),float(encL)]
		encRold = encR
		encLold = encL
		#odocount=data.header.seq

		u=calc_input(rs)

		x=MotionModel(x,u,dt)


def toDegree(angle):
	angle=angle*180.0/pi
	return angle

def toRadian(angle):
	angle=angle*pi/180.0
	return angle

if __name__ == '__main__':
	talker()

