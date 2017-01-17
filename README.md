
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
**8M ** | 1002 | 4
**5M** | 1002 | 5
**3M** | 1002 | 6
**VGA** | 1002 | 7
