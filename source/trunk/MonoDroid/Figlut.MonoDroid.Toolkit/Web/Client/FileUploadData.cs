namespace Figlut.MonoDroid.Toolkit
{
	#region Using Directives

	using System;

	#endregion //Using Directives

	public class FileUploadData
	{
		#region Properties

		public string FileName { get; set; }

		public int Offset { get; set; }

		public byte[] Buffer { get; set; }

		#endregion //Properties
	}
}