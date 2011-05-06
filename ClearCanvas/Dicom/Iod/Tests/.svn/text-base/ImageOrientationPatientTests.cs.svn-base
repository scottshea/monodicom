#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

#if	UNIT_TESTS

using ClearCanvas.Dicom.Iod;
using NUnit.Framework;

namespace ClearCanvas.Dicom.Iod.Tests
{
	[TestFixture]
	public class ImageOrientationPatientTests
	{
		public ImageOrientationPatientTests()
		{ 
		
		}

		[Test]
		public void TestCosines()
		{
			ImageOrientationPatient iop = new ImageOrientationPatient(0.1, 0.2, 0.7, 0.2, 0.3, 0.5);
			Assert.AreEqual(iop.RowX, 0.1);
			Assert.AreEqual(iop.RowY, 0.2);
			Assert.AreEqual(iop.RowZ, 0.7);
			Assert.AreEqual(iop.ColumnX, 0.2);
			Assert.AreEqual(iop.ColumnY, 0.3);
			Assert.AreEqual(iop.ColumnZ, 0.5);
		}
				
		[Test]
		public void TestEdges()
		{
			// See ImageOrientationPatient class for an explanation of the logic in the tests.

			//the fact that the row/column cosines aren't orthogonal doesn't matter here, we're just testing the algorithm.
			ImageOrientationPatient iop = new ImageOrientationPatient(0, 0, 0, 0, 0, 0);
			Assert.IsTrue(iop.GetPrimaryRowDirection(false) == ImageOrientationPatient.Directions.None);
			Assert.IsTrue(iop.GetSecondaryRowDirection(false) == ImageOrientationPatient.Directions.None);

			Assert.IsTrue(iop.GetPrimaryColumnDirection(false) == ImageOrientationPatient.Directions.None);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(false) == ImageOrientationPatient.Directions.None);


			iop = new ImageOrientationPatient(0.1, 0.2, 0.7, 0.2, 0.3, 0.5);

			Assert.IsTrue(iop.GetPrimaryRowDirection(false) == ImageOrientationPatient.Directions.Head);
			Assert.IsTrue(iop.GetSecondaryRowDirection(false) == ImageOrientationPatient.Directions.Posterior);
			Assert.IsTrue(iop.GetPrimaryRowDirection(true) == ImageOrientationPatient.Directions.Foot);
			Assert.IsTrue(iop.GetSecondaryRowDirection(true) == ImageOrientationPatient.Directions.Anterior);

			Assert.IsTrue(iop.GetPrimaryColumnDirection(false) == ImageOrientationPatient.Directions.Head);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(false) == ImageOrientationPatient.Directions.Posterior);
			Assert.IsTrue(iop.GetPrimaryColumnDirection(true) == ImageOrientationPatient.Directions.Foot);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(true) == ImageOrientationPatient.Directions.Anterior);

			iop = new ImageOrientationPatient(0.1, -0.2, -0.7, 0.2, -0.3, -0.5);

			Assert.IsTrue(iop.GetPrimaryRowDirection(false) == ImageOrientationPatient.Directions.Foot);
			Assert.IsTrue(iop.GetSecondaryRowDirection(false) == ImageOrientationPatient.Directions.Anterior);
			Assert.IsTrue(iop.GetPrimaryRowDirection(true) == ImageOrientationPatient.Directions.Head);
			Assert.IsTrue(iop.GetSecondaryRowDirection(true) == ImageOrientationPatient.Directions.Posterior);

			Assert.IsTrue(iop.GetPrimaryColumnDirection(false) == ImageOrientationPatient.Directions.Foot);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(false) == ImageOrientationPatient.Directions.Anterior);
			Assert.IsTrue(iop.GetPrimaryColumnDirection(true) == ImageOrientationPatient.Directions.Head);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(true) == ImageOrientationPatient.Directions.Posterior);

			iop = new ImageOrientationPatient(0.7, -0.1, -0.2, 0.5, -0.2, -0.3);

			Assert.IsTrue(iop.GetPrimaryRowDirection(false) == ImageOrientationPatient.Directions.Left);
			Assert.IsTrue(iop.GetSecondaryRowDirection(false) == ImageOrientationPatient.Directions.Foot);
			Assert.IsTrue(iop.GetPrimaryRowDirection(true) == ImageOrientationPatient.Directions.Right);
			Assert.IsTrue(iop.GetSecondaryRowDirection(true) == ImageOrientationPatient.Directions.Head);

			Assert.IsTrue(iop.GetPrimaryColumnDirection(false) == ImageOrientationPatient.Directions.Left);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(false) == ImageOrientationPatient.Directions.Foot);
			Assert.IsTrue(iop.GetPrimaryColumnDirection(true) == ImageOrientationPatient.Directions.Right);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(true) == ImageOrientationPatient.Directions.Head);

			iop = new ImageOrientationPatient(-0.2, -0.7, 0.1, -0.3, -0.5, -0.2);

			Assert.IsTrue(iop.GetPrimaryRowDirection(false) == ImageOrientationPatient.Directions.Anterior);
			Assert.IsTrue(iop.GetSecondaryRowDirection(false) == ImageOrientationPatient.Directions.Right);
			Assert.IsTrue(iop.GetPrimaryRowDirection(true) == ImageOrientationPatient.Directions.Posterior);
			Assert.IsTrue(iop.GetSecondaryRowDirection(true) == ImageOrientationPatient.Directions.Left);

			Assert.IsTrue(iop.GetPrimaryColumnDirection(false) == ImageOrientationPatient.Directions.Anterior);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(false) == ImageOrientationPatient.Directions.Right);
			Assert.IsTrue(iop.GetPrimaryColumnDirection(true) == ImageOrientationPatient.Directions.Posterior);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(true) == ImageOrientationPatient.Directions.Left);

			iop = new ImageOrientationPatient(-0.2, -0.7, 0.1, -0.3, -0.5, -0.2);

			Assert.IsTrue(iop.GetPrimaryRowDirection(false) == ImageOrientationPatient.Directions.Anterior);
			Assert.IsTrue(iop.GetSecondaryRowDirection(false) == ImageOrientationPatient.Directions.Right);
			Assert.IsTrue(iop.GetPrimaryRowDirection(true) == ImageOrientationPatient.Directions.Posterior);
			Assert.IsTrue(iop.GetSecondaryRowDirection(true) == ImageOrientationPatient.Directions.Left);

			Assert.IsTrue(iop.GetPrimaryColumnDirection(false) == ImageOrientationPatient.Directions.Anterior);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(false) == ImageOrientationPatient.Directions.Right);
			Assert.IsTrue(iop.GetPrimaryColumnDirection(true) == ImageOrientationPatient.Directions.Posterior);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(true) == ImageOrientationPatient.Directions.Left);

			iop = new ImageOrientationPatient(-0.2, -0.6, 0.2, -0.4, -0.3, -0.3);

			Assert.IsTrue(iop.GetPrimaryRowDirection(false) == ImageOrientationPatient.Directions.Anterior);
			Assert.IsTrue(iop.GetSecondaryRowDirection(false) == ImageOrientationPatient.Directions.Right);
			Assert.IsTrue(iop.GetPrimaryRowDirection(true) == ImageOrientationPatient.Directions.Posterior);
			Assert.IsTrue(iop.GetSecondaryRowDirection(true) == ImageOrientationPatient.Directions.Left);

			Assert.IsTrue(iop.GetPrimaryColumnDirection(false) == ImageOrientationPatient.Directions.Right);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(false) == ImageOrientationPatient.Directions.Anterior);
			Assert.IsTrue(iop.GetPrimaryColumnDirection(true) == ImageOrientationPatient.Directions.Left);
			Assert.IsTrue(iop.GetSecondaryColumnDirection(true) == ImageOrientationPatient.Directions.Posterior);
		}
	}
}

#endif