#!/usr/bin/env python
# -*- coding: utf-8 -*-
import socket

def main():
	soc = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	#soc.connect(("localhost", 11211))
	soc.connect(("192.168.1.4", 11211))
	
	while(1):
		reciveData = soc.recv(1024)
		print "Server>", reciveData
		data = raw_input("Client>")
		soc.send(data)

		if data == "q":
			soc.close()
			break

if __name__ == '__main__':
	main()
