using System;

namespace OpenDialogs
{
	public struct OFNOTIFY
	{
		public NMHDR hdr;

		public IntPtr OPENFILENAME;

		public IntPtr fileNameShareViolation;
	}
}
