# Operations

````C#
// Creates a new array segment by taking a number of elements from the beginning of this array segment.
ArraySegment<T> ArraySegment<T>.Take(int count);

// Creates a new array segment by skipping a number of elements from the beginning of this array segment.
ArraySegment<T> ArraySegment<T>.Skip(int count);

// Creates a new array segment by skipping a number of elements and then taking a number of elements from this array segment.
ArraySegment<T> ArraySegment<T>.Slice(int skipCount, int takeCount);

// Creates a new array segment by taking a number of elements from the end of this array segment.
ArraySegment<T> ArraySegment<T>.TakeLast(int count);

// Creates a new array segment by skipping a number of elements from the end of this array segment.
ArraySegment<T> ArraySegment<T>.SkipLast(int count);

// Copies the elements in this array segment into a destination array segment.
void ArraySegment<T>.CopyTo(ArraySegment<T> destination);
````

# Dividing an array segment into other array segments

It's often useful to divide one array segment into a series of contiguous array segments. The `ArraySegmentReader<T>` type uses a stream-like abstraction, with a seekable `Position`.

````C#
ArraySegmentReader<T> ArraySegment<T>.CreateArraySegmentReader();

public sealed class ArraySegmentReader<T>
{
  // Gets or sets the position of this reader.
  public int Position { get; set; }

  // Sets the position of this reader. Returns the new position.
  public int Seek(int offset, SeekOrigin origin);

  // Creates a new array segment which starts at the current position and includes the specified number of elements.
  // Advances the position by the number of elements in the returned array segment.
  public ArraySegment<T> Read(int count);
}
````

# Operations on binary data

These methods are useful when you have an `ArraySegment<byte>` and need to treat it like binary data.

````C#
// Creates a MemoryStream over this array segment.
MemoryStream ArraySegment<byte>.CreateStream(bool writable = true);

// Creates a BinaryReader over this array segment.
BinaryReader ArraySegment<byte>.CreateBinaryReader();

// Creates a BinaryWriter over this array segment.
BinaryWriter ArraySegment<byte>.CreateBinaryWriter();
````

# Converting to/from arrays, and implementing `IList<T>`

````C#
// Creates an array segment referencing this array.
ArraySegment<T> T[].AsArraySegment(int offset = 0, int count = array.Length - offset);

// Creates a new array containing the elements in this array segment.
T[] ArraySegment<T>.ToArray();

// Copies the elements in this array segment into a destination array.
void ArraySegment<T>.CopyTo(T[] array, int arrayIndex = 0);

// Creates an IList<T> wrapper for this array segment. The wrapper also implements IList.
ArraySegmentListWrapper<T> ArraySegment<T>.AsIList();
````
