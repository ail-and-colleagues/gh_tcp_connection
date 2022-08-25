import struct
import socket
import datetime
import numpy as np
from struct import pack, unpack
import datetime

size_of_float = 4

class TCPSvr():
    def __init__(self, cfg):
        self.addr = cfg["addr"]
        self.port = cfg["port"]
        self.wait = cfg["wait"]
        self.strc = struct.Struct(cfg["format"])
        self.sock = None
        

    def Open(self):
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.bind((self.addr, self.port))
        self.sock.settimeout(self.wait)
        self.sock.listen(5)
        return

    def ReceiveData(self, func):
        while 1:
            print(datetime.datetime.now())
            try:
                clientsocket, address = self.sock.accept()
                print("Connection from {} has been established!".format(address))
                res = clientsocket.recv(self.strc.size)
                data = unpack(self.strc.format, res)
                ret = func(data)
                print("ret: ", type(ret), ret)
                data = pack(str(ret.shape[0]) + "f", *ret)

                clientsocket.send(data)
                clientsocket.close()
            except socket.timeout:
                pass
        
class Callback():
    """
    a class for defining a callback function executed when the server gets data.  
    """
    def __init__(self):
        pass
    def func(self, arg):
        # print("type(arg): ", type(arg))
        arg = np.array(arg)
        # print("arg.shape: ", arg.shape)
        arg = arg.reshape([-1, 3])
        return np.mean(arg, axis=0)

if __name__ == "__main__":
    cfg = {
        "addr": "127.0.0.1",
        "port": 3141,
        "wait": 1,
        "format": "30f",
    }
    tcpSvr = TCPSvr(cfg)
    tcpSvr.Open()
    c = Callback()
    tcpSvr.ReceiveData(c.func)
