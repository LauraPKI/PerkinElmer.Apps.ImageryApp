using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PerkinElmer.Signals.Analytics.ComponentsApp")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyCulture("")]

// -----------------------------------------------------------------------
// <copyright file="AssemblyVersionInfo.cs" company="PerkinElmer Inc.">
// Copyright (c) 2014 PerkinElmer Inc., 
// Viscount Center 2, Milburn Hill Road, Coventry, CV4 7HS
// All rights reserved. 
// This software is the confidential and proprietary information 
// of PerkinElmer Inc. ("Confidential Information"). You shall not 
// disclose such Confidential Information and may not use it in any way, 
// absent an express written license agreement between you and PerkinElmer Inc. 
// that authorizes such use.
// </copyright>
// -----------------------------------------------------------------------
// This file holds all the assembly information that is shared among all
// C# projects of the Plates solution

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#elif UNIT_TEST
[assembly: AssemblyConfiguration("Release (Debug information)")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif


[assembly: AssemblyCopyright("Copyright \x00a9 2018 PerkinElmer Inc. All rights reserved.")]
[assembly: AssemblyProduct("Sample App")]
[assembly: AssemblyTrademark("PerkinElmer")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version \ These should be set to match the major.minor version of
//      Minor Version / the Spotfire sdk assemblies this gets bundled with.
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// .
