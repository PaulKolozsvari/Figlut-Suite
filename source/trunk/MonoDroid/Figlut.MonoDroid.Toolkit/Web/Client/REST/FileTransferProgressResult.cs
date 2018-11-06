namespace Figlut.MonoDroid.Toolkit
{
	#region Using Directives

	using System;

	#endregion //Using Directives

	public class FileTransferProgressResult : EventArgs
	{
		public FileTransferProgressResult (string fileName, long transferredBytes, long totalBytes)
		{
			_fileName = fileName;
			_transferredBytes = transferredBytes;
			_totalBytes = totalBytes;
			_percentageCompleted = Convert.ToInt32 ((((double)_transferredBytes) / ((double)_totalBytes)) * 100);
		}

		#region Fields

		private string _fileName;
		private long _transferredBytes;
		private long _totalBytes;
		private int _percentageCompleted;

		#endregion //Fields

		#region Properties

		public string FileName
		{
			get{ return _fileName; }
		}

		public long TransferredBytes
		{
			get{ return _transferredBytes; }
		}

		public long TotalBytes
		{
			get{ return _totalBytes; }
		}

		public int PercentageCompleted
		{
			get{ return _percentageCompleted; }
		}

		#endregion //Properties
	}
}