#!/usr/bin/env python  
# -*- coding: utf-8 -*-
import socket
import sys
import time
import threading
from Queue import Queue

class Bserver:

  def __init__(self):

    self.ans  = "no data"
    self.org = time.time()
    self.compasdt = "000"
    self.compasptn = "9"
    self.gpsdt = ""
    self.gx = 0.0
    self.gy = 0.0
    self.gr = 0.0
    self.rx = 0.0
    self.ry = 0.0
    self.rr = 0.0
    self.rxy_flg = 0
    self.rr_flg = 0
    self.startdt = 0
    self.restartdt = 0
    self.pulseL = 0.0
    self.pulseR = 0.0
    self.pulse_flg = 0

    self.handle = 0.0
    self.throttle = 0.0
    
    self.cpX = 0.0
    self.cpY = 0.0
    self.cpYaw = 0.0
    self.rcp_flg = 0

    self.tire1 = 0.0
    self.tire2 = 0.0
#    self.que = Queue()

    self.mvX = 0.0
    self.mvY = 0.0
    self.mvAng = 0.0


  def check_xyflg(self):
    ret = self.rxy_flg
    self.rxy_flg = 0
    return ret

  def get_xy( self):
    return ( self.rx, self.ry)

  def check_rflg(self):
    ret = self.rr_flg
    self.rr_flg = 0
    return ret

  def check_EncPulseflg(self):
    ret = self.pulse_flg
    self.pulse_flg = 0
    return ret

  def get_EncPulse(self):
    return ( self.pulseL, self.pulseR )

  def get_rr(self):
    return self.rr

  # plotÀ•W
  def set_globalxyr(self, dt1, dt2, dt3):
    self.gx = dt1
    self.gy = dt2
    self.gr = dt3

  # cmdvel_movebaseÀ•W
  def set_movebase(self, dt1, dt2, dt3):
    self.mvX = dt1
    self.mvY = dt2
    self.mvAng = dt3

#  def set_qEncorderdt(self, dt):
#    self.que.put(dt)

  # RE set
  def set_encorderdt(self, dt1, dt2):
    self.tire1 += dt1 
    self.tire2 += dt2 
  # Compus set
  def set_compasdt(self, dt):
    self.compasdt = dt
  
  # LED Pattern
  def get_compasptn(self):
    return self.compasptn

  def set_compasptn(self, dt):
    self.compasptn = dt

  def set_gpsdt(self, dt):
    self.gpsdt = dt

  def add_startCnt(self,dt):
    self.startdt = self.startdt + 1

  def add_restartCnt(self,dt):
    self.restartdt = self.restartdt + 1

  def get_handle_throttle(self):
    return (self.handle, self.throttle)

  def set_handle_throttle(self, h,t):
    self.handle = 0.0
    self.throttle = 0.0

  def get_checkpoint_flg(self):
    return self.rcp_flg

  def get_checkpoint(self):
    self.rcp_flg = 0
    return (self.cpX, self.cpY,self.cpYaw)


  # Command Return
  def mkans(self, cmd):
    self.ans = cmd + "," + str(round (time.time() - self.org, 4))

  def run(self):

    # Create a TCP/IP socket
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    # Bind the socket to the port
    server_address = ('', 50001)
    print >>sys.stderr, 'starting up on %s port %s' % server_address
    sock.bind(server_address)

    # Listen for incoming connections

    sock.listen(1)

    flg = 0
    cnt = 0
    while True:
# Wait for a connection
      print >>sys.stderr, 'waiting for a connection'
      connection, client_address = sock.accept()

      try:
        print >> sys.stderr, 'connection from', client_address

        # Receive the data in small chunks and retransmit it

        datas = ""
        bcs = ""
        cmd = ""
        while True:
          flg = 0

          wk = bcs + connection.recv(512)
          cn = wk.rfind("\n")
          data = ""

          if cn >= 0:
            if len(wk) == cn + 1:
              cmd = wk
              bcs = ""
            else:
              wcmd = wk
              bcs = wcmd[cn:]
              cmd = wcmd[:cn]

          ds = cmd.split("\n")

          for jj in range(len(ds)):
            data = ds[jj]

	    if data:
	      #print cnt, data, len(datas)
	      cnt = cnt + 1
	      try: 
		# RE pulse
		if "A1" in data:
		  self.mkans ("A1")          
		  self.ans = self.ans + "," + str(self.tire1) + "," +  str(self.tire2)
		  connection.sendall(self.ans+"$")
		  self.tire1 = 0.0
		  self.tire2 = 0.0
		  flg = 1
		# compus
		if "A2" in data:
		  self.mkans ("A2")          
		  self.ans = self.ans + "," + self.compasdt
		  connection.sendall(self.ans+"$")
		  flg = 1
		# GPS
		if "A3" in data:
		  self.mkans ("A3")          
		  self.ans = self.ans + "," + self.gpsdt
		  connection.sendall(self.ans+"$")
		  flg = 1
		# RE Plot x,y dir
		if "A4" in data:
		  self.mkans ("A4")          
		  self.ans = self.ans + "," + str(self.gx) + "," + str(self.gy) + "," + str(self.gr )
		  connection.sendall(self.ans+"$")
		  flg = 1
		# all  NotWorking
		if "A0" in data:
		  self.mkans ("A0")          
		  connection.sendall(self.ans+"$")
		  flg = 1
        # counter?
		if "A5" in data:
		  self.mkans ("A5")          
		  self.ans = self.ans + "," + str(self.startdt)
		  connection.sendall(self.ans+"$")
		  flg = 1
		# counter?
		if "A6" in data:
		  self.mkans ("A6")          
		  self.ans = self.ans + "," + str(self.restartdt)
		  connection.sendall(self.ans+"$")
		  flg = 1
		# MoveBase x,y dir
		if "M0" in data:
		  self.mkans ("M0")          
		  self.ans = self.ans + "," + str(self.mvX) + "," + str(self.mvY) + ",0,0,0," + str(self.mvAng )
		  connection.sendall(self.ans+"$")
		  flg = 1

        # Recive Handle, Accel
		if "AC" in data:
		    cmd = data[0:data.find("\n")]
		    dats = cmd.split(",")

		    if len(dats) >= 3:
		      self.handle = float(dats[1]) # Handle
		      self.throttle = float(dats[2]) # Throttle
		      self.mkans ("AC")          
		      connection.sendall(self.ans+","+str(self.handle)+","+str(self.throttle)+"$")
		      #print "        bsrv ac" + str(dats[1])+ "  "+str(dats[2])
		      flg = 1

        # Recive CheckPoint
		if "AG" in data:
		    cmd = data[0:data.find("\n")]
		    dats = cmd.split(",")

		    if len(dats) >= 4:
		      self.cpX = float(dats[1]) # x
		      self.cpY = float(dats[2]) # y
		      self.cpYaw = float(dats[3]) # dir yaw
		      self.mkans ("AG")          
		      connection.sendall(self.ans+","+str(self.cpX)+","+str(self.cpY)+","+str(self.cpYaw)+"$")
		      print "        bsrv AG, x:" + str(dats[1]) + "  y:" + str(dats[2]) + "  ang:" + str(dats[3])
		      flg = 1
		      self.rcp_flg = 1

        # Recive LED Pattern
		if "AL" in data:
		  cmd = data[0:data.find("\n")]
		  dats = cmd.split(",")
		  if len(dats) >= 2:
		    self.compasptn = dats[1]
		    self.mkans ("AL")          
		    flg = 1
		    connection.sendall(self.ans+"$")

		if "AD" in data:
		  cmd = data[0:data.find("\n")]
		  dats = cmd.split(",")
		  if len(dats) >= 3:
		    self.rx = float(dats[1])
		    self.ry = float(dats[2])
		    self.rxy_flg = 1
		    self.mkans ("AD")
		    flg = 1
		    connection.sendall(self.ans+"$")

		if "AR" in data:
		  cmd = data[0:data.find("\n")]
		  dats = cmd.split(",")
		  if len(dats) >= 2:
		    self.rr = float(dats[1])
		    self.rr_flg = 1
		    self.mkans ("AR")          
		    flg = 1
		    connection.sendall(self.ans+"$")

		if "EP" in data:
		  cmd = data[0:data.find("\n")]
		  dats = cmd.split(",")
		  if len(dats) >= 3:
		    self.pulseL = float(dats[1])
		    self.pulseR = float(dats[2])
		    self.pulse_flg = 1
		    self.mkans ("EP")          
		    flg = 1
		    connection.sendall(self.ans+"$")

		if flg == 0:
		  connection.sendall("E0,Cmd Err! :[ "+data+"]$")

	      except ValueError:
		  connection.sendall("E1,Cmd Err : ["+data+"]$")
		  datas=""
		  continue
     
	    #else:
	      #print >> sys.stderr, 'no more data from', client_address
              #break

      except socket.error, msg:
        connection.close()
        continue


      finally:
        # Clean up the connection
        connection.close()

  def start(self):
    th = threading.Thread(target=self.run)
    th.setDaemon(True)
    th.start()


if __name__=='__main__':
  bsrv = Bserver()
  bsrv.start() 
  time.sleep(7200)
