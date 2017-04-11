# ArraySegments

Lightweight extension methods for ArraySegment<T>, particularly useful for byte arrays.

Supports .NET 4.0 (client and full), .NET 4.5, Metro/WinRT, Silverlight 4 and 5, Windows Phone 7 and 7.5, all portable library profiles, and XBox.

## Getting Started

Download the [NuGet package](https://nuget.org/packages/Nito.ArraySegments).

All extension methods and helper types are in the `ArraySegments` namespace.

## Quick Start

`ArraySegment<T>` is a BCL type, but it doesn't get a lot of love. The ArraySegments library provides the love.

Let's say you've got a huge array of binary data that you'd rather not be copying around a lot:

````C#
byte[] myData = ...;
````

ArraySegments can help by creating `ArraySegment<T>` instances that reference the original array without copying it. First, import the namespace:

````C#
using ArraySegments;
````

Now you can make an array segment that refers to the entire array:

````C#
var data = myData.AsArraySegment();
````

Once it's an array segment, you can slice and dice to your heart's content. E.g., if we need to read a header that is 1024 bytes:

````C#
var header = data.Take(1024); // Look ma, no copying!
````

`Take`, `Skip`, `TakeLast`, `SkipLast`, and `Slice` are all variations on this theme.

You can also use an "ArraySegment Reader" to treat any array segment as though it was a stream of smaller array segments:

````C#
var reader = data.CreateArraySegmentReader();
var record0 = reader.Read(1024); // Still no copying!
reader.Seek(2048, SeekOrigin.Current);
var record3 = reader.Read(1024);
var remainingData = data.Skip(reader.Position);
````

There are also convenience methods defined for binary readers (and writers, and MemoryStreams):

````C#
using (var binaryReader = record0.CreateBinaryReader())
{
  var littleEndianInt32 = binaryReader.ReadInt32();
}
````

Unfortunately, many APIs do not accept `ArraySegment<T>` types. ArraySegments provides an `IList<T>` wrapper, which is supported by some APIs:

````C#
IList<byte> headerDataAsIList = header.AsIList(); // Still no copying!
````

Other APIs support an array/offset/count triplet, which is easy to do with array segments:

````C#
otherApi(header.Array, header.Offset, header.Count); // And still, no copying!
````

But too many APIs are built around arrays. In this case, you do have to do some copying when you pass your data off. The easy way is to just create a new array or copy it into an existing array:

````C#
byte[] headerDataAsArray = header.ToArray(); // We do have to copy here.
header.CopyTo(data); // We do have to copy here.
````

[See more docs](documentation.md)
