using FluentAssertions;

namespace PersistableWindows.IntegrationTests;

public class TestViewModelTests
{
    public const string TestingGuid = "8AB6D9BD-F834-4C9E-AD6E-E88C7AA88F58";
    
    private TestViewModel _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new TestViewModel();
        
        // TODO: Delete file in appdata for GUID
    }

    [Test]
    public void TestViewModel_WhenInstantiated_ShouldNotBeNull()
    {
        _sut.Should().NotBeNull();
    }
    
    [Test]
    public void TestViewModel_WhenInstantiated_ShouldHaveLeftProperty()
    {
        _sut.Left.Should().Be(0.0);
    }
    
    [Test]
    public void TestViewModel_WhenInstantiated_ShouldHaveTopProperty()
    {
        _sut.Width.Should().Be(0.0);
    }
    
    [Test]
    public void TestViewModel_WhenInstantiated_ShouldHaveCorrectWindowGuid()
    {
        _sut.WindowGuid.Should().Be(TestingGuid);
    }
    
    [Test]
    public void TestViewModel_WhenSaveWindowStateCalled_ShouldSaveWindowState()
    {
        // TODO: Delete file in appdata for GUID
    }
}

[PersistableWindow(TestViewModelTests.TestingGuid)]
public partial class TestViewModel
{
        
}