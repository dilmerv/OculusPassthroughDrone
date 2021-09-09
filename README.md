# Oculus Passthrough Drone
Oculus Passthrough Drone Demos by using Oculus Quest 2, Unity, Tello, and Brain Computer Interfaces.

This repo aims to teach you how to build you own Drone FPV by using the following technologies:

* Unity 2020.3.16f1 or greater
* Oculus Integration + Oculus Passthrough API ([Tutorial About This Tech](https://github.com/dilmerv/OculusPassthroughDemos))
* Tello DJI Drone which communicates via UDP ([Get the Tello DJI Drone](https://amzn.to/3hdtSHD))

Two main scenes to look at for your own reference:
* StandaloneDroneClient.unity - the uses all components shown from the "Architecture" diagram except that OVR Passthrough Layer reference is not supported.
* OculusDroneClient.unity - all components of "Architecture" diagram are used.

Architecture:

<img src="https://github.com/dilmerv/BCIDroneDemos/blob/master/docs/architecture.png" width="500">

Demos:

<img src="https://github.com/dilmerv/BCIDroneDemos/blob/master/docs/images/demo_1.gif" width="250">
