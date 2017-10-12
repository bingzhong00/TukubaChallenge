#!/usr/bin/env python
# -*- coding: utf-8 -*-
  
import roslib
import rospy
import math
import tf
import geometry_msgs.msg
from datetime import datetime

from nav_msgs.msg import MapMetaData

map_width = 0
map_height = 0
map_resolution = 0.0
map_dataGet = False
map_originX = 0.0
map_originY = 0.0
map_originZ = 0.0

# Mapdata CallBack
def cb_MapMetadata(dt):
	global map_width,map_height,map_resolution,map_dataGet
	map_width = dt.width
	map_height = dt.height
	map_resolution = dt.resolution
	# map_load_time
	# origin
	map_originX = dt.origin.position.x
	map_originY = dt.origin.position.y
	map_originZ = dt.origin.position.z
	map_dataGet = True
	rospy.loginfo("recive map_metadata width %d, height %d, resolution %f", map_width, map_height, map_resolution )

if __name__ == '__main__':
	rospy.init_node('rosif_base_link_checkpoint')

	listener = tf.TransformListener()
	subMapMetadata = rospy.Subscriber('/map_metadata', MapMetaData, cb_MapMetadata)

	# 5FPS
	rate = rospy.Rate(1.0/5.0)
	
	now = datetime.now()
	fname_head = "check_point{0:%Y%m%d_%H%M%S}".format(now)
	fname = fname_head + ".xml"
	f = open( fname, "w" )

	rospy.loginfo("create file %s", fname)

	mapname = "map{0:%Y%m%d_%H}".format(now)
	f.write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n")
	f.write("<MapData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n")
	f.write("\t<MapName>"+mapname+"</MapName>\n")
	f.write("\t<MapYamlFileName>"+fname_head + ".yaml"+"</MapYamlFileName>\n")

	while map_dataGet == False and rospy.is_shutdown() == False:
		rate.sleep()

	f.write("\t<RealWidth>"+"{:.2f}".format(map_width*map_resolution)+"</RealWidth>\n")
	f.write("\t<RealHeight>"+"{:.2f}".format(map_height*map_resolution)+"</RealHeight>\n")
	f.write("\t<Resolution>"+"{:.3f}".format(map_resolution)+"</Resolution>\n")
	f.write("\t<MapOrign>\n")
	f.write("\t\t<x>"+"{:.3f}".format(map_originX)+"</x>\n")
	f.write("\t\t<y>"+"{:.3f}".format(map_originY)+"</y>\n")
	f.write("\t\t<z>"+"{:.3f}".format(map_originZ)+"</z>\n")
	f.write("\t</MapOrign>\n")
	f.write("\t<startPosition>\n")
	f.write("\t\t<x>"+"{:.2f}".format(-map_originX/map_resolution)+"</x>\n")
	f.write("\t\t<y>"+"{:.2f}".format(-map_originY/map_resolution)+"</y>\n")
	f.write("\t\t<z>0</z>\n")
	f.write("\t</startPosition>\n")
	f.write("\t<startDir>0</startDir>\n")
	f.write("\t<checkPoint>\n")

	#rospy.loginfo("receive map data map width %d, height %d, resolution %f", map_width, map_height, map_resolution )
	rospy.loginfo("start output checkpoint")

	# receive point
	prevTrans = [0,0,0];
	try:
		while not rospy.is_shutdown():
			try:
				(trans,rot) = listener.lookupTransform('map', 'base_link', rospy.Time(0))
			except (tf.LookupException, tf.ConnectivityException, tf.ExtrapolationException):
				continue
		
			# 2.0mごとにポイントを打っていく
			if math.hypot(prevTrans[0] - trans[0], prevTrans[1] - trans[1]) > 2.0:
				rospy.loginfo("base_link trans %f,%f,%f / rot %f,%f,%f", trans[0], trans[1], trans[2], rot[0], rot[1], rot[2]);

				f.write("\t\t<Vector3>\n")
				f.write("\t\t\t<x>" + "{:.2f}".format((-map_originX/map_resolution)+(trans[0]/map_resolution)) + "</x>\n")
				f.write("\t\t\t<y>" + "{:.2f}".format((-map_originY/map_resolution)+(-trans[1]/map_resolution)) + "</y>\n")
				f.write("\t\t\t<z>" + "{:.2f}".format((-map_originZ/map_resolution)+(trans[2]/map_resolution)) + "</z>\n")
				f.write("\t\t</Vector3>\n")

				prevTrans[0] = trans[0];
				prevTrans[1] = trans[1];
				prevTrans[2] = trans[2];

			rate.sleep()
	except:
		pass
	# 終了

	# interruptException でも書き出す
	f.write("\t</checkPoint>\n")
	f.write("</MapData>\n")
	f.close()

