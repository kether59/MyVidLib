using MyVidLib.Application.Common;

namespace MyVidLib.Application.Tests;

public class AssemblyMarkerTests
{
    [Fact]
    public void Name_ReturnsExpectedValue()
    {
        // Arrange / Act
        var name = AssemblyMarker.Name;

        // Assert
        Assert.Equal("MyVidLib.Application", name);
    }
}
