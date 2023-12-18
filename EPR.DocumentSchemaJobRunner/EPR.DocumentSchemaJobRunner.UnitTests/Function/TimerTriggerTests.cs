namespace EPR.DocumentSchemaJobRunner.UnitTests.Function;

using DocumentSchemaJobRunner.Application.Jobs.Interfaces;
using DocumentSchemaJobRunner.Function;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class TimerTriggerTests
{
    private Mock<IDocumentSchemaJob> _schemaJobOneMock;
    private Mock<IDocumentSchemaJob> _schemaJobTwoMock;
    private TimerTrigger _systemUnderTest;

    [TestInitialize]
    public void TestInitialize()
    {
        _schemaJobOneMock = new Mock<IDocumentSchemaJob>();
        _schemaJobTwoMock = new Mock<IDocumentSchemaJob>();
        _systemUnderTest = new TimerTrigger(new List<IDocumentSchemaJob> { _schemaJobOneMock.Object, _schemaJobTwoMock.Object });
    }

    [TestMethod]
    public async Task RunAsync_CallsSchemaJobs()
    {
        // Arrange / Act
        await _systemUnderTest.RunAsync(null, null);

        // Assert
        _schemaJobOneMock.Verify(x => x.RunAsync(), Times.Once);
        _schemaJobTwoMock.Verify(x => x.RunAsync(), Times.Once);
    }
}