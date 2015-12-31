/**
 *  SingleEntry 
 *  ViewController.cs
 * 
 * 	This file implement the main controller for SingleEntry app.
 *  SingleEntry app is a very simple demo app to show how to get
 *  the scanner decoded data into a simple edit box.
 * 
 *  Order of things
 *  The App uses ScanApiHelper that handles most of the work for using
 *  ScanAPI.
 *  ScanApiHelper provides a delegate interface that the application controller
 *  should derive from and implement each of the delegates.
 *  Here is the sequence:
 *  1) Pass the controller reference to ScanApiHelper, to it can call the delegates
 *  2) Open ScanApiHelper, this will initialize ScanAPI, and the onScanApiInitialized is called once it's done
 *  3) Create a timer that will consumme the events from ScanAPI
 *  4) From the timer call the ScanApiHelper doScanApiReceive, in order to send commands or receive event from
 *     ScanAPI. It is recommended to call this function in the MainUIThread context, so the ScanApiHelper delegates
 *     can directly and safely update the UI.
 *
 *
 *	(c) 2015 Socket Mobile, Inc. all rigth reserved.
 */
using System;

using UIKit;
using ScanAPI;
using System.Timers;
using System.Runtime.InteropServices;
using System.Text;

namespace SingleEntry
{
	public partial class ViewController : UIViewController,ScanApiHelper.ScanApiHelperNotification
	{
		ScanApiHelper _scanApi = new ScanApiHelper();
		Timer _timer = new Timer();
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			_scanApi.SetNotification(this);
			_scanApi.Open ();
			_timer.Elapsed+= (object sender, ElapsedEventArgs e) => InvokeOnMainThread (() => _scanApi.DoScanAPIReceive ());
			_timer.Interval = 200;
			_timer.Start ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		#region ScanApiHelperNotification implementation

		public void OnDeviceArrival (long result, DeviceInfo newDevice)
		{
			status.Text = "Scanner Connected: " + newDevice.Name;
		}

		public void OnDeviceRemoval (DeviceInfo deviceRemoved)
		{
			status.Text = "No Scanner connected";
		}

		public void OnError (long result, string errMsg)
		{
			throw new NotImplementedException ();
		}

		public void OnDecodedData (DeviceInfo device, ISktScanDecodedData decodedData)
		{
			this.decodedData.Text = Marshal.PtrToStringAuto( decodedData.Data);
		}

		public void OnScanApiInitializeComplete (long result)
		{
			InvokeOnMainThread(() => {
				status.Text = "ScanAPI Ready!";
				_scanApi.PostGetScanAPIVersion(onGetScanApiVersion);
			});
		}

		public void OnScanApiTerminated ()
		{
			status.Text = "ScanAPI Terminated!";
		}

		public void OnErrorRetrievingScanObject (long result)
		{
			throw new NotImplementedException ();
		}

		#endregion

		void onGetScanApiVersion (long result, ISktScanObject scanObj)
		{
			if(SktScanErrors.SKTSUCCESS(result)){
				StringBuilder version = new StringBuilder("ScanAPI Version: ");
				version.AppendFormat ("{0:x}.{1:x}.{2:x}.{3:x} {4:x}/{5:x2}/{6:x2} {7:x}:{8:x}",  
					scanObj.Property.Version.Major,
					scanObj.Property.Version.Middle,
					scanObj.Property.Version.Minor,
					scanObj.Property.Version.Build,
					scanObj.Property.Version.Month,
					scanObj.Property.Version.Day,
					scanObj.Property.Version.Year,
					scanObj.Property.Version.Hour,
					scanObj.Property.Version.Minute);
				info.Text = version.ToString();
			}
		}

	}
}

