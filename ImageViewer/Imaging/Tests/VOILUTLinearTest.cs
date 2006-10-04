#if	UNIT_TESTS

using System;
using NUnit.Framework;
using ClearCanvas.ImageViewer.Imaging;

namespace ClearCanvas.ImageViewer.Tools.Standard.Tests
{
	[TestFixture]
	public class VOILUTLinearTest
	{
		public VOILUTLinearTest()
		{
		}

		[TestFixtureSetUp]
		public void Init()
		{
		}
		
		[TestFixtureTearDown]
		public void Cleanup()
		{
		}

		[Test]
		public void Unsigned1()
		{
			double windowWidth = 1;
			double windowLevel = 11;

			VOILUTLinear lut = new VOILUTLinear(10, 11);

			lut.WindowWidth = windowWidth;
			lut.WindowCenter = windowLevel;

			Assert.AreEqual(0, lut[10]);
			Assert.AreEqual(255, lut[11]);
		}

		[Test]
		public void Signed1()
		{
			double windowWidth = 1;
			double windowLevel = 0;

			VOILUTLinear lut = new VOILUTLinear(-1,	0);

			lut.WindowWidth = windowWidth;
			lut.WindowCenter = windowLevel;

			Assert.AreEqual(0, lut[-1]);
			Assert.AreEqual(255, lut[0]);
		}

		[Test]
		public void Unsigned12()
		{
			double windowWidth = 4096;
			double windowLevel = 2147;

			VOILUTLinear lut = new VOILUTLinear(100, 4195);

			lut.WindowWidth = windowWidth;
			lut.WindowCenter = windowLevel;

			Assert.AreEqual(0, lut[100]);
			Assert.AreEqual(255, lut[4195]);
			Assert.AreEqual(2147, lut.WindowCenter);
			Assert.AreEqual(4096, lut.WindowWidth);
		}

		[Test]
		public void Signed12()
		{
			double windowWidth = 4096;
			double windowLevel = 0;

			VOILUTLinear lut = new VOILUTLinear(-2048, 2047);

			lut.WindowWidth = windowWidth;
			lut.WindowCenter = windowLevel;

			Assert.AreEqual(0, lut[-2048]);
			Assert.AreEqual(127, lut[0]);
			Assert.AreEqual(255, lut[2047]);
		}

		[Test]
		public void AlterWindowLevel()
		{
			double windowWidth = 4096;
			double windowLevel = 2047;

			VOILUTLinear lut = new VOILUTLinear(0, 4095);

			lut.WindowWidth = windowWidth;
			lut.WindowCenter = windowLevel;

			Assert.AreEqual(0, lut[0]);
			Assert.AreEqual(127, lut[2047]);
			Assert.AreEqual(255, lut[4095]);

			lut.WindowWidth = 512;
			lut.WindowCenter = 1023;

			Assert.AreEqual(512, lut.WindowWidth);
			Assert.AreEqual(1023, lut.WindowCenter);
			Assert.AreEqual(0, lut[767]);
			Assert.AreEqual(255, lut[1279]);
		}

		[Test]
		public void Threshold()
		{
			double windowWidth = 1;
			double windowLevel = 0;

			VOILUTLinear lut = new VOILUTLinear(-2048, 2047);

			lut.WindowWidth = windowWidth;
			lut.WindowCenter = windowLevel;

			Assert.AreEqual(0, lut[-2]);
			Assert.AreEqual(0, lut[-1]);
			Assert.AreEqual(255, lut[0]);
			Assert.AreEqual(255, lut[1]);
		}

		[Test]
		public void WindowWiderThanInputRange()
		{
			double windowWidth = 8192;
			double windowLevel = 0;

			VOILUTLinear lut = new VOILUTLinear(-2048, 2047);

			lut.WindowWidth = windowWidth;
			lut.WindowCenter = windowLevel;

			Assert.AreEqual(63, lut[-2048]);
			Assert.AreEqual(127, lut[0]);
			Assert.AreEqual(191, lut[2047]);
		}

		[Test]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void IndexOutOfRangeTooLow()
		{
			double windowWidth = 4096;
			double windowLevel = 0;

			VOILUTLinear lut = new VOILUTLinear(-2048, 2047);

			lut.WindowWidth = windowWidth;
			lut.WindowCenter = windowLevel;

			int val = lut[-2049];
		}

		[Test]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void IndexOutOfRangeTooHigh()
		{
			double windowWidth = 4096;
			double windowLevel = 0;

			VOILUTLinear lut = new VOILUTLinear(-2048, 2047);

			lut.WindowWidth = windowWidth;
			lut.WindowCenter = windowLevel;

			int val = lut[2048];
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ZeroWidth()
		{
			double windowWidth = 0;
			double windowLevel = 0;

			VOILUTLinear lut = new VOILUTLinear(-2048, 2047);

			lut.WindowWidth = windowWidth;
			lut.WindowCenter = windowLevel;
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void MinGreaterThanEqualToMax()
		{
			double windowWidth = 1;
			double windowLevel = 0;

			VOILUTLinear lut = new VOILUTLinear(1, 0);

			lut.WindowWidth = windowWidth;
			lut.WindowCenter = windowLevel;
		}
	}
}

#endif