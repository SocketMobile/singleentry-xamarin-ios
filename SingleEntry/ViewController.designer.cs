// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SingleEntry
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField decodedData { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel info { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel status { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (decodedData != null) {
				decodedData.Dispose ();
				decodedData = null;
			}
			if (info != null) {
				info.Dispose ();
				info = null;
			}
			if (status != null) {
				status.Dispose ();
				status = null;
			}
		}
	}
}
