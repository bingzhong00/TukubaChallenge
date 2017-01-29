#!/usr/bin/env python
# -*- coding: utf-8 -*-
import rospy
from visualization_msgs.msg import Marker

def callback_recive(message):
	rospy.loginfo("msg: %s",message)


def main():
	rospy.init_node('listenser')
	sub = rospy.Subscriber('/svo/keyframes', Marker, callback_recive)
	rospy.spin()

if __name__ == '__main__':
	main()
