using System;
using System.Collections.Generic;

public class ChunkTable
{
	private ushort tableSize;
	private byte chunkSize;
	private Dictionary<ushort, LinkedList<ushort>> table;

    #region constructors
    public ChunkTable()
	{
		chunkSize = 32;
		tableSize = (ushort)(chunkSize * chunkSize);
		table = new Dictionary<ushort, LinkedList<ushort>>(tableSize);
	}
	public ChunkTable (int chunkSize)
    {
		if (chunkSize < 0 || chunkSize > 255)
			throw new ArgumentOutOfRangeException();

		this.chunkSize = (byte)chunkSize;
		tableSize = (ushort)(chunkSize * chunkSize);
		table = new Dictionary<ushort, LinkedList<ushort>>(tableSize);
    }
    #endregion

    #region helper methods
    private ushort XYToKey (byte xVal, byte yVal)
    {
		ushort key = (ushort)(xVal + (yVal * chunkSize));
		return key;
    }
	private byte[] KeyToXY (ushort key)
    {
		byte[] coordPair = new byte[2];
		coordPair[0] = (byte)(key % chunkSize);
		coordPair[1] = (byte)((key - coordPair[0]) % 64);
		return coordPair;
    }
	private bool isUShort (int[] list)
	{
		foreach (int i in list)
			if (i < 0 || i > 65535)
				return false;
	
		return true;
	}
	private bool isUShort (int value)
	{
		int[] list = { value };
		return isUShort(list);
	}
	private bool isByte (int[] list)
	{
		foreach (int i in list)
			if (i < 0 || i > 255)
				return false;

		return true;
	}
	private bool isByte (int value)
	{
		int[] list = { value };
		return isByte(list);
	}
    #endregion

    public void AddTile (int xVal, int yVal, int zVal)
    {

    }
	public bool ContainsCell (int xVal, int yVal)
    {
		ushort cellKey = XYToKey((byte)xVal, (byte)yVal);
		return table.ContainsKey(cellKey);
    }
	public bool ContainsValue (int xVal, int yVal, int zVal)
	{
		if (!ContainsCell(xVal, yVal))
			return false;

		ushort cellKey = XYToKey((byte)xVal, (byte)yVal);
		table.TryGetValue(cellKey, out LinkedList<ushort> temp);
		return temp.Contains((ushort)zVal);
	}
	
	public static void Main (string[] args)
    {
		ChunkTable testChunk = new ChunkTable(64);
    }
}