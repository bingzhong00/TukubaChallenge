#!/usr/bin/env python  
import roslib
import rospy
import math
import tf
import geometry_msgs.msg
#from tf.transformations import quaternion_from_euler
 
if __name__ == '__main__':
	rospy.init_node('rosif_base_link_pub')

	listener = tf.TransformListener() 
	pubBaselink = rospy.Publisher('rosif/base_link', geometry_msgs.msg.Twist, queue_size=10)

	rate = rospy.Rate(10.0)
	count = 0
	while not rospy.is_shutdown():
		try:
			#(trans,rot) = listener.lookupTransform('base_link', 'odom', rospy.Time(0))
			(trans,rot) = listener.lookupTransform('map', 'base_link', rospy.Time(0))
			#listener.lookupTransform('base_link', 'odom', rospy.Time(0), transform )
		except (tf.LookupException, tf.ConnectivityException, tf.ExtrapolationException):
			continue
 
		if count%200==0:
			rospy.loginfo("base_link trans %f,%f,%f / rot %f,%f,%f", trans[0], trans[1], trans[2], rot[0], rot[1], rot[2]);
        
		count=count+1
		euler = tf.transformations.euler_from_quaternion((rot[0],rot[1],rot[2],rot[3]))

		cmd = geometry_msgs.msg.Twist()
		cmd.linear.x = trans[0]
		cmd.linear.y = trans[1]
		cmd.linear.z = trans[2]
		cmd.angular.x = euler[0]
		cmd.angular.y = euler[1]
		cmd.angular.z = euler[2]

		pubBaselink.publish(cmd)

		rate.sleep()

