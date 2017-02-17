#!/usr/bin/env python  
import roslib
import rospy
import math
import tf
import geometry_msgs.msg
from datetime import datetime
 
if __name__ == '__main__':
	rospy.init_node('rosif_base_link_checkpoint')

	listener = tf.TransformListener() 
	
	now = datetime.now()
	fname = "check_point{0:%Y%m%d_%H%M%S}.txt".format(now)
	f = open( fname, "w" )

	rate = rospy.Rate(1.0/5.0)
	count = 0
	while not rospy.is_shutdown():
		try:
			#(trans,rot) = listener.lookupTransform('base_link', 'odom', rospy.Time(0))
			(trans,rot) = listener.lookupTransform('odom', 'base_link', rospy.Time(0))
			#listener.lookupTransform('base_link', 'odom', rospy.Time(0), transform )
		except (tf.LookupException, tf.ConnectivityException, tf.ExtrapolationException):
			continue
 
		#if count%200==0:
		rospy.loginfo("base_link trans %f,%f,%f / rot %f,%f,%f", trans[0], trans[1], trans[2], rot[0], rot[1], rot[2]);
        
		count=count+1

		cmd = geometry_msgs.msg.Twist()
		cmd.linear.x = trans[0]
		cmd.linear.y = trans[1]
		cmd.linear.z = trans[2]
		cmd.angular.x = rot[0]
		cmd.angular.y = rot[1]
		cmd.angular.z = rot[2]

		f.write("<Vector3>\n")
		f.write("\t<x>" + "{:.2f}".format(trans[0]) + "</x>\n")
		f.write("\t<y>" + "{:.2f}".format(trans[1]) + "</y>\n")
		f.write("\t<z>" + "{:.2f}".format(trans[2]) + "</z>\n")
		f.write("</Vector3>\n")

		rate.sleep()

	f.close()

