using MyVidLib.Domain.Common;

namespace MyVidLib.Domain.Tests;

public class AssemblyMarkerTests
{
    [Fact]
    public void Name_ReturnsExpectedValue()
    {
        // Arrange / Act
        var name = AssemblyMarker.Name;

        // Assert
        Assert.Equal("MyVidLib.Domain", name);
    }
}
