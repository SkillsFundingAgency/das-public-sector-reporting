using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace SFA.DAS.PSRService.Application.UnitTests.FileInfo;

public class StringFileInfo : IFileInfo
{
    private readonly string _contents;
    private readonly Lazy<long> _lazyLength;

    private static readonly Encoding Encoding = Encoding.UTF8;

    public StringFileInfo(string contents, string name)
    {
        _contents = contents;
        LastModified = DateTimeOffset.UtcNow;
        Name = name;
        _lazyLength = new Lazy<long>(() => Encoding.GetByteCount(_contents));
    }

    public Stream CreateReadStream() => new MemoryStream(Encoding.GetBytes(_contents ?? ""));

    public bool Exists => true;
    public long Length => _lazyLength.Value;
    public string PhysicalPath => null;
    public string Name { get; }
    public DateTimeOffset LastModified { get; }
    public bool IsDirectory => false;
}