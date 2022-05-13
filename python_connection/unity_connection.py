import random
import socket
import time
import re
import numpy as np


# def calculate_pos():
#     pos_x = random.uniform(1, 9)
#     pos_z = random.uniform(1, 9)
#     rotation = random.uniform(0, 360)
#     # Because of scale of the map UI return 4 times of x axis and 3 times of y axis
#     return pos_x, pos_z, rotation

def calculate_pos(index):
    coords_webot = np.loadtxt("coords.txt").reshape(376, 3)
    rot = np.loadtxt("rot.txt").reshape(376, 3)

    return coords_webot[index][0] + 5, coords_webot[index][1] + 5, rot[index][2] * - 180 / np.pi


def set_connection(port):
    # set port 49152 if receiving starting pos, set 25001 if sending calculated pos of the AGV
    # set argument as data which will be sent to unity
    host, port = "127.0.0.1", port
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((host, port))
    return sock


has_started = True
start_clicked = False
starting_pos = []
key = "next connection"
i = 0
sock = set_connection(49152)

while True:
    time.sleep(1)  # sleep 2 sec
    posString = ','.join(map(str, "."))  # Converting Vector3 to a string, example "0,0,0"
    print("Sended data to unity ...", posString)
    sock.sendall(posString.encode("UTF-8"))  # Converting string to Byte, and sending it to C#
    receivedData = sock.recv(1024).decode("UTF-8")  # receiving data in Byte from C#, and converting it to String
    print("Activation key recieved :", receivedData)
    starting_pos = [float(x) for x in re.findall('(\d+)', receivedData)]
    if len(starting_pos) > 0:
        starting_pos = [starting_pos[0] + starting_pos[1] / 10, starting_pos[2] + starting_pos[3] / 10,
                        starting_pos[4] + starting_pos[5] / 10]
        print("starting pos : ", starting_pos)

    if "next connection" in receivedData:
        print("acquired next connection")
        sock.sendall(key.encode("UTF-8"))
        break

sock = set_connection(25001)

while has_started:

    if start_clicked:  # Unless start button clicked in unity program takes initial position of the AGV as an input
        starting_pos = calculate_pos(i)

        i += 1
        print("i", i)

    # time.sleep(0.1)  # sleep 2 sec
    posString = ','.join(map(str, starting_pos))  # Converting Vector3 to a string, example "0,0,0"
    print("Sended data to unity ...", posString)
    sock.sendall(posString.encode("UTF-8"))  # Converting string to Byte, and sending it to C#
    receivedData = sock.recv(1024).decode("UTF-8")  # receiving data in Byte from C#, and converting it to String
    print("Activation key recieved :", receivedData)

    if "1" in receivedData:
        start_clicked = True
        print("starting the User Interface")

    elif "0" in receivedData:
        print("Stop")
        sock.sendall(key.encode("UTF-8"))
        has_started = False
