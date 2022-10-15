using WetPet.Infrastructure.Common.Utils;

namespace WetPet.Infrastructure.Test.Common.Utils;

public class ToSnakeCaseTests
{
    [Theory]
    [InlineData("Test", "test")]
    [InlineData("thisIsATest", "this_is_a_test")]
    [InlineData("this_is_a_test", "this_is_a_test")]

    public void ProperlyConverts(string input, string expected)
    {
        var actual = input.ToSnakeCase();
        Assert.Equal(expected, actual);
    }
}