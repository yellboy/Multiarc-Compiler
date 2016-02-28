
// This is auto-generated code.
// Please, edit only method body.

public static void Cycle(Memory memory)
{
	memory.WaitForRisingEdge("RD_WR0");
	var address = memory.GetPort("ADDR").Val;
	byte[] binaryValue = Program.Mem[(uint)address];
	int intValue = ConversionHelper.ConvertFromByteArrayToInt(binaryValue);
	memory.Wait(12);
	memory.GetPort("ADDR").Val = intValue;
	Console.WriteLine("Memory giving the value on address {0}.", address);
}
