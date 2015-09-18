using System;
using ObjCRuntime;

[assembly: LinkWith ("libPayPalMobile.a", 
	LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, 
//	SmartLink = true, 
	ForceLoad = true,
	Frameworks="AVFoundation AudioToolbox CoreMedia CoreVideo SystemConfiguration Security MessageUI OpenGLES MobileCoreServices",
	LinkerFlags="-lz -lxml2 -lc++ -lstdc++")]
