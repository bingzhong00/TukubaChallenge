#!/usr/bin/env python  
import roslib
# roslib.load_manifest('learning_tf')
import rospy
import math
import tf
import geometry_msgs.msg
import turtlesim.srv

if __name__ == '__main__':
    rospy.init_node('amcltalker')

    listener = tf.TransformListener()

    turtle_vel = rospy.Publisher('rosif/amcl_result', geometry_msgs.msg.Twist, queue_size=10)

    rate = rospy.Rate(10.0)
    while not rospy.is_shutdown():
        try:
            (trans,rot) = listener.lookupTransform('/map', '/base_footprint', rospy.Time(0))
        except (tf.LookupException, tf.ConnectivityException, tf.ExtrapolationException):
            continue

        #angular = 4 * math.atan2(trans[1], trans[0])
        #linear = 0.5 * math.sqrt(trans[0] ** 2 + trans[1] ** 2)
        cmd = geometry_msgs.msg.Twist()
        #cmd.linear.x = linear
        #cmd.angular.z = angular
	cmd.linear.x = trans[0]
	cmd.linear.y = trans[1]
	cmd.linear.z = trans[2]
	eul = tf.transformations.euler_from_quaternion((rot[0], rot[1], rot[2], rot[3]))
	cmd.angular.x = eul[0]
	cmd.angular.y = eul[1]
	cmd.angular.z = eul[2]
        turtle_vel.publish(cmd)

        rate.sleep()
