
# SJCAM
Universal Windows 10 App (UWP) for SJCAM action camera

## Introduction

As I owned a SJCAM action camera recently (M20), I started to write an Universal app for windows 10 family. The idea is to support all the parameters from the camera. Currently I can only test on the M20 camera, as this is the only one I own.

## Features

- Auto connect to camera, the idea is to avoid the user that has to quit the app to connect
- Snap pictures and takes videos
- Adjust resolution, brightness, sharpness etc etc, well, to support all the camera settings
- Access to camera storage through network, and be able to retrieve photos and videos
- Sexy design (Important!)
- No ads

## Commands

Everything is based on GET Request to the camera, which has a Base URL on http://192.168.1.254/
Get request is build such as http://192.168.1.254/?custom=1&cmd={COMMAND}&par={PARAM}

### Change modes

Mode | Command | Param
-----|---------|------
**Photo** | 3001 | 0
**Video** | 3001 | 1
**Replay** | 3001 | 2
**Video Time Lapse** | 3001 | 3
**Photo Time Lapse** | 3001 | 4

### Live Preview
rtsp live protocol which could be found on rtsp://192.168.1.254/sjcam.{FORMAT}. Two formats are allowed, mov or mp4

### Image Resolution

Resolution | Command | Param
-----------|---------|------
**16M** | 1002 | 0
**14M** | 1002 | 1
**12M** | 1002 | 2
**10M** | 1002 | 3
**8M** | 1002 | 4
**5M** | 1002 | 5
**3M** | 1002 | 6
**VGA** | 1002 | 7

### Video Resolution

Resolution | Command | Param
-----------|---------|------
**2K 30fps** | 2002 | 0
**1080p 60fps** | 2002 | 1
**1080p 30fps** | 2002 | 2
**720p 120fps** | 2002 | 3
**720p 60fps** | 2002 | 4
**720p 30fps** | 2002 | 5
**480p 240fps** | 2002 | 6

### Photo Timelapse Capture interval


Interval | Command | Param
---------|---------|------
**3s** | 1012 | 0
**5s** | 1012 | 1
**10s** | 1012 | 2
**20s** | 1012 | 3

### Video Timelapse Capture interval

Interval | Command | Param
---------|---------|------
**1s** | 2019 | 0
**2s** | 2019 | 1
**5s** | 2019 | 2
**10s** | 2019 | 3
**30s** | 2019 | 4
**60s** | 2019 | 5

### Exposure

Exposure | Command | Param
---------|---------|------
**+2** | 2005 | 0
**+5/3** | 2005 | 1
**+4/3** | 2005 | 2
**+1** | 2005 | 3
**+2/3** | 2005| 4
**+1/3** | 2005 | 5
**+0** | 2005 | 6
**-1/3** | 2005 | 7
**-2/3** | 2005| 8
**-1** | 2005| 9
**-4/3** | 2005| 10
**-5/3** | 2005| 11
**-2** | 2005| 12

### White balance

Balance | Command | Param
--------|---------|------
**Auto** | 1007 | 0
**Daylight** | 1007 | 1
**Cloudy** | 1007 | 2
**Tungsten** | 1007 | 3
**Fluorescent** | 1007 | 4

### Audio

Status | Command | Param
-------|---------|------
**off** | 2007 | 0
**on** | 2007 | 1

### WDR (HDR equivalent)

Status | Command | Param
-------|---------|------
**off** | 2004 | 0
**on** | 2004 | 1

### Gyro

Status | Command | Param
-------|---------|------
**off** | 9001 | 0
**on** | 9001 | 1

### Date stamp

Status | Command | Param
-------|---------|------
**off** | 2008 | 0
**on** | 2008 | 1

### Frequency

Status | Command | Param
-------|---------|------
**50Hz** | 3025 | 0
**60Hz** | 3025 | 1

### Cyclic Record


Time | Command | Param
-----|---------|------
**off** | 2003 | 0
**3 minutes** | 2003 | 1
**5 minutes** | 2003 | 2
**10 minutes** | 2003 | 3

### Auto power off

Time | Command | Param
-----|---------|------
**Auto** | 3007 | 0
**3 minutes** | 3007 | 1
**5 minutes** | 3007 | 2
**10 minutes** | 3007 | 3
**No turn off** | 3007 | 4

### Memory

#### Number of photos left


Returns | Command | Param
--------|---------|------
<Value>Double</Value> | 1003 | None

#### Video record (seconds left)

Returns | Command | Param
--------|---------|------
<Value>double</Value> | 2009 | None

#### Remaining space in bytes

Returns | Command | Param
--------|---------|------
<Value>double</Value> | 3017 | None

### Wifi

#### Disconnect 

http://192.168.1.254/?custom=1&cmd=3013

#### Set Wifi Password

http://192.168.1.254/?custom=1&cmd=3004&str=newPassword

#### Set Wifi SSID

http://192.168.1.254/?custom=1&cmd=3003&str=newSSID

### Snap picture

Command = 1001

### Record Video

Action | Command | Param
-------|---------|------
Stop | 2001 | 0
Start | 2001 | 1
