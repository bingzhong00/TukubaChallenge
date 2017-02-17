#!/usr/bin/env python  
import roslib
#roslib.load_manifest('learning_tf')
import rospy
import math
import tf
import geometry_msgs.msg
#import turtlesim.srv
 
if __name__ == '__main__':
    rospy.init_node('rosif_base_link_pub')

    listener = tf.TransformListener()

    #rospy.wait_for_service('spawn')
    #spawner = rospy.ServiceProxy('spawn', turtlesim.srv.Spawn)
    #spawner(4, 2, 0, 'turtle2')
 
    pubBaselink = rospy.Publisher('rosif/base_link', geometry_msgs.msg.Twist, queue_size=10)

    rate = rospy.Rate(10.0)
    while not rospy.is_shutdown():
        try:
            #(trans,rot) = listener.lookupTransform('base_link', 'odom', rospy.Time(0))
            (trans,rot) = listener.lookupTransform('odom', 'base_link', rospy.Time(0))
            #listener.lookupTransform('base_link', 'odom', rospy.Time(0), transform )
        except (tf.LookupException, tf.ConnectivityException, tf.ExtrapolationException):
            continue
 
        rospy.loginfo("base_link trans %f,%f,%f / rot %f,%f,%f", trans[0], trans[1], trans[2], rot[0], rot[1], rot[2]);
        cmd = geometry_msgs.msg.Twist()
        cmd.linear.x = trans[0]
        cmd.linear.y = trans[1]
        cmd.linear.z = trans[2]
        cmd.angular.x = rot[0]
        cmd.angular.y = rot[1]
        cmd.angular.z = rot[2]
        
        #rospy.loginfo("base_link ", transform);
        #cmd = geometry_msgs.msg.Twist()

        pubBaselink.publish(cmd)
  
        rate.sleep()

