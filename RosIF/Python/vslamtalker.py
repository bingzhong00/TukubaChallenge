#!/usr/bin/env python

# Copyright (c) 2012, Tang Tiong Yew
# All rights reserved.
#
# Redistribution and use in source and binary forms, with or without
# modification, are permitted provided that the following conditions are met:
#
#    * Redistributions of source code must retain the above copyright
#      notice, this list of conditions and the following disclaimer.
#    * Redistributions in binary form must reproduce the above copyright
#      notice, this list of conditions and the following disclaimer in the
#      documentation and/or other materials provided with the distribution.
#    * Neither the name of the Willow Garage, Inc. nor the names of its
#      contributors may be used to endorse or promote products derived from
#       this software without specific prior written permission.
#
# THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
# AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
# IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
# ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
# LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
# CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
# SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
# INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
# CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
# ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
# POSSIBILITY OF SUCH DAMAGE.

import rospy
import string
import math
import sys

#from time import time
#from sensor_msgs.msg import Imu
from geometry_msgs.msg import Twist
#from tf.transformations import quaternion_from_euler
#from dynamic_reconfigure.server import Server
#from razor_imu_9dof.cfg import imuConfig
#from diagnostic_msgs.msg import DiagnosticArray, DiagnosticStatus, KeyValue
from visualization_msgs.msg import Marker

degrees2rad = math.pi/180.0
rsvPosX = 0.0
rsvPosY = 0.0
rsvPosZ = 0.0

def cb_visualmaker(dt):
	global rsvPosX, rsvPosY, rsvPosZ
	if dt.color.r == 0.0 and dt.color.g == 0.0 and dt.color.b == 0.5 and dt.color.a == 1.0:
		rsvPosX = dt.pose.position.x
		rsvPosY = dt.pose.position.y
		rsvPosZ = dt.pose.position.z
	


rospy.init_node("vslamtalker_node")
#We only care about the most recent measurement, i.e. queue_size=1
pub = rospy.Publisher('/torosif/vslam', Twist, queue_size=1)
#srv = Server(imuConfig, reconfig_callback)  # define dynamic_reconfigure callback
#diag_pub = rospy.Publisher('diagnostics', DiagnosticArray, queue_size=1)
#diag_pub_time = rospy.get_time();
sub = rospy.Subscriber('/svo/points', Marker, cb_visualmaker)

pubMsg = Twist()

seq=0
rospy.loginfo("Start Vslam to Rosif ...")

while not rospy.is_shutdown():

    # Publish message
    # AHRS firmware accelerations are negated
    # This means y and z are correct for ROS, but x needs reversing
    pubMsg.linear.x = rsvPosX
    pubMsg.linear.y = rsvPosY
    pubMsg.linear.z = rsvPosZ

    pubMsg.angular.x = 0.0
    pubMsg.angular.y = 0.0
    pubMsg.angular.z = 0.0

    #pubMsg.header.stamp= rospy.Time.now()
    #pubMsg.header.frame_id = 'base_imu_link'
    #pubMsg.header.seq = seq
    #seq = seq + 1
    pub.publish(pubMsg)
    rospy.sleep(0.1)
