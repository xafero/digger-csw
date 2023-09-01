using DiggerClassic.Util;
using Xunit;

namespace Digger.Tests
{
	public class UtilTest
	{
		[Theory]
		[InlineData("/icons/digger.png", 21507)]
		public void TestFindRes(string name, int size)
		{
			var res = Resources.FindResource(name);
			Assert.NotNull(res);
			Assert.Equal(size, res.Length);
		}
	}
}