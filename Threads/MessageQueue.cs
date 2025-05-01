using System.Threading.Channels;


namespace Threads;



public class MessageQueueSplit<T> where T : class {
	public readonly MessageReader<T> Reader;
	public readonly MessageWriter<T> Writer;

	public MessageQueueSplit(int capacity, BoundedChannelFullMode fullMode, bool isSingleReader, bool isSingleWriter) {
		var channel = Channel.CreateBounded<T>(new BoundedChannelOptions(capacity) {
			FullMode = fullMode,
			SingleReader = isSingleReader,
			SingleWriter = isSingleWriter
		});
		Reader = new MessageReader<T>(channel, capacity);
		Writer = new MessageWriter<T>(channel, capacity);
	}
}


public class MessageReader<T>(Channel<T> channel, int capacity) where T : class {
	private readonly ChannelReader<T> _reader = channel.Reader;

	public T? Read() {
		var success = _reader.TryRead(out var data);
		return success ? data : null;
	}

	public int Count() {
		return _reader.Count;
	}

	public int Capacity() {
		return capacity;
	}
}


public class MessageWriter<T>(Channel<T> channel, int capacity) where T : class {
	private readonly ChannelWriter<T> _writer = channel.Writer;

	public async Task WriteAsync(T items) {
		await _writer.WriteAsync(items);
	}

	public void WriterComplete() {
		_writer.Complete();
	}

	public int Count() {
		return channel.Reader.Count;
	}

	public int Capacity() {
		return capacity;
	}
}


public class MessageQueue<T> where T : class {
	private readonly Channel<T> _channel;

	public MessageQueue(int capacity, BoundedChannelFullMode fullMode, bool singleReader, bool singleWriter) {
		_channel = Channel.CreateBounded<T>(new BoundedChannelOptions(capacity) {
			FullMode = fullMode,
			SingleReader = singleReader,
			SingleWriter = singleWriter
		});
	}


	public async Task WriteAsync(T items) {
		await _channel.Writer.WriteAsync(items);
	}

	public T? Read() {
		var success = _channel.Reader.TryRead(out var data);
		return success ? data : null;
	}

	public int Count() {
		return _channel.Reader.Count;
	}

	public void WriterComplete() {
		_channel.Writer.Complete();
	}
}


// Thread 1       | Thread 2    | Description
//---------------------------------------------------
// Send()         |             | Thread 1, sends message.
//                | Receive()   | Thread 2, receives the message and acts on it.
//                | Reply()     | Thread 2, replies with result.
// Confirm()      |             | Thread 1, receives confirmation.
public class TwoWayMessageQueue<TSendData, TReplyData> where TSendData : class where TReplyData : class {
	private readonly MessageQueue<TSendData> _messageQueueOut;
	private readonly MessageQueue<TReplyData> _messageQueueIn;

	public TwoWayMessageQueue(int capacity, BoundedChannelFullMode fullMode, bool isSingleReader, bool isSingleWriter) {
		_messageQueueOut = new MessageQueue<TSendData>(capacity, fullMode, isSingleReader, isSingleWriter);
		_messageQueueIn = new MessageQueue<TReplyData>(capacity, fullMode, isSingleReader, isSingleWriter);
	}


	public async Task Send(TSendData item) {
		await _messageQueueOut.WriteAsync(item);
	}
	public TSendData? Receive() {
		var msg = _messageQueueOut.Read();
		return msg;
	}
	public async Task Reply(TReplyData item) {
		await _messageQueueIn.WriteAsync(item);
	}
	public TReplyData? Confirm() {
		var msg = _messageQueueIn.Read();
		return msg;
	}
}
