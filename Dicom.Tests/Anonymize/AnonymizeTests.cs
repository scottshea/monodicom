using NUnit.Framework;
using System;
using Dicom.Anonymize;
namespace Dicom.Tests
{
	[TestFixture]
	public class AnonymizeTests
	{
		[Test]
		public void testActionMethod ()
		{
			pickUpFilesAndAnonymizeThem anon = new pickUpFilesAndAnonymizeThem();
			anon.anonymizeFiles();
		}
	}
}

