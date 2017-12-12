#!/usr/bin/env python

import sys
import rospy
import math
import numpy as np
#import scipy as sp
import matplotlib
import matplotlib.pyplot as plt
from sensor_msgs.msg import Imu
from geometry_msgs.msg import Vector3
from tf.transformations import quaternion_from_euler
import tf



rospy.init_node('movement_from_imu')
pubMov = rospy.Publisher('/Movements', Vector3, queue_size = 1)

# tf broad cast
bc = tf.TransformBroadcaster()

# constant
accel_factor = 9.806 / 256.0 # to dimension of acceleration



def callbackImu(dtImu):
    global acceleration
    global mean_acc
    global time_imuCounter
    global time_accCounter
    acceleration['accX'][time_accCounter-1] = dtImu.linear_acceleration.x # m/s^2
    acceleration['accY'][time_accCounter-1] = dtImu.linear_acceleration.y
    acceleration['accZ'][time_accCounter-1] = dtImu.linear_acceleration.z
    time = dtImu.header.stamp	#get time stamp with acceleration



#class Movements:
#    def __init__(self):
#        imuMsg = Imu()
#        self.time = np.arange(0, 100, 1)
#        self.accelaration = {'accX': np.zeros(self.time.size), 'accY': np.zeros(self.time.size), 'accZ': np.zeros(self.time.size)}
#        self.velocity = {'velX': np.zeros(self.time.size/10), 'velY': np.zeros(self.time.size/10), 'velZ': np.zeros(self.time.size/10)}
#        self.movements = np.array([0, 0, 0])
#        self.time_accCounter = 1
#        self.time_velCounter = 1


#    def Integrate(self, imuMsg):
#        if time_accCounter%time.size == 0:
#            self.velocity['velX'][time_velCounter-1] = np.trapz(self.accelaration['accX'], dx = 1.0)
 #           self.velocity['velY'][time_velCounter-1] = np.trapz(self.accelaration['accY'], dx = 1.0)
#            self.velocity['velZ'][time_velCounter-1] = np.trapz(self.accelaration['accZ'], dx = 1.0)
#            time_accCounter = 1
    
#        if time_velCounter%time.size == 0:
#            self.movements[0] = np.trapz(self.velocity['velX'], dx = 1.0)
#            self.movements[1] = np.trapz(self.velocity['velY'], dx = 1.0)
#            self.movements[2] = np.trapz(self.velocity['velZ'], dx = 1.0)
#            time_velCounter = 1
        
#        self.time_accCounter += 1
#        self.time_velCounter += 1
#        print(self.movements)
        
#        return self.movements

#mov = Movements()
sub = rospy.Subscriber('imu', Imu, callbackImu)



movMsg = Vector3()
time = np.arange(0, 100, 1)	# time.size = 100
#print(time.size)
acceleration = {'accX': np.zeros(10), 'accY': np.zeros(10), 'accZ': np.zeros(10)}
mean_acc = { 'm_accX': np.zeros(time.size), 'm_accY': np.zeros(time.size), 'm_accZ': np.zeros(time.size) }
velocity = {'velX': np.zeros(time.size), 'velY': np.zeros(time.size), 'velZ': np.zeros(time.size)}
movements = np.array([0, 0, 0])
time_imuCounter = 1
time_accCounter = 1
time_velCounter = 1

while not rospy.is_shutdown():    
    if time_accCounter%10 == 0:    
        mean_acc['m_accX'][time_imuCounter-1] = round(np.mean(acceleration['accX']), 2)
        mean_acc['m_accY'][time_imuCounter-1] = round(np.mean(acceleration['accY']), 2)
        mean_acc['m_accZ'][time_imuCounter-1] = round(np.mean(acceleration['accZ']), 2)
        time_accCounter = 1
        time_imuCounter += 1
    
    if time_imuCounter%time.size == 0:
        velocity['velX'][time_velCounter-1] = np.trapz(mean_acc['m_accX'], dx = 1.0)
        velocity['velY'][time_velCounter-1] = np.trapz(mean_acc['m_accY'], dx = 1.0)
        velocity['velZ'][time_velCounter-1] = np.trapz(mean_acc['m_accZ'], dx = 1.0)
        time_imuCounter = 1
        time_velCounter += 1
    
    if time_velCounter%(time.size) == 0:
        movMsg.x += np.trapz(velocity['velX'], dx = 1.0)
        movMsg.y += np.trapz(velocity['velY'], dx = 1.0)
        movMsg.z += np.trapz(velocity['velZ'], dx = 1.0)
        time_velCounter = 1
        #plt.scatter(time_movCounter, movMsg.x)
        #plt.scatter(time_movCounter, movMsg.y)
        #plt.scatter(time_movCounter, movMsg.z)
        #plt.show()
        
        print(movMsg)
        pubMov.publish(movMsg)

        q = quaternion_from_euler(0.0,0.0,0.0)
        # publish tf
        if bc is not None:
            bc.sendTransform((movMsg.x,movMsg.y,movMsg.z),q,rospy.Time.now(), 'base_link', 'map')
    
    time_accCounter += 1
