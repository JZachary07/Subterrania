using System;
using System.Collections;
using System.Collections.Generic;

public class ChunkTable
{
	private ushort tableSize;
	private byte chunkSize;
	private Dictionary<ushort, ArrayList> table;

    #region constructors
    public ChunkTable()
	{
		chunkSize = 32;
		tableSize = (ushort)(chunkSize * chunkSize);
		table = new Dictionary<ushort, ArrayList>(tableSize);
	}
	public ChunkTable (int chunkSize)
    {
		if (chunkSize < 0 || chunkSize > 255)
			throw new ArgumentOutOfRangeException();

		this.chunkSize = (byte)chunkSize;
		tableSize = (ushort)(chunkSize * chunkSize);
		table = new Dictionary<ushort, ArrayList>(tableSize);
    }
    #endregion

    #region helper methods
    private ushort XYToKey (byte xVal, byte yVal) // converts X-Y pair into key value
    {
		ushort key = (ushort)(xVal + (yVal * chunkSize));
		return key;
    }
	private byte[] KeyToXY (ushort key) // converts key value back into X-Y pair
    {
		byte[] coordPair = new byte[2];
		coordPair[0] = (byte)(key % chunkSize);
		coordPair[1] = (byte)((key - coordPair[0]) % 64);
		return coordPair;
    }
	private bool isUShort (int[] list) // checks list of ints are in range for int --> ushort casting
	{
		foreach (int i in list)
			if (i < 0 || i > 65535)
				return false;
	
		return true;
	}
	private bool isUShort (int value) // checks single int is in range for int --> ushort casting (invokes sister method)
	{
		int[] list = { value };
		return isUShort(list);
	}
	private bool isByte (int[] list) // checks list of ints are in range for int --> byte casting
	{
		foreach (int i in list)
			if (i < 0 || i > 255)
				return false;

		return true;
	}
	private bool isByte (int value) // checks single int is in range for int --> byte csating (invokes sister method)
	{
		int[] list = { value };
		return isByte(list);
	}
    #endregion
	
    public void AddTile (int xVal, int yVal, int zVal)
    {
		int[] temp = { xVal, yVal };
		if (!isByte(temp) && !isByte(zVal))
			throw new ArgumentOutOfRangeException("A value is out of range for minimization");

		if (!ContainsCell(xVal, yVal))
		{
			this.AddCell(xVal, yVal, zVal);
			return;
		}
		ushort key = XYToKey((byte)xVal, (byte)yVal);
		table.TryGetValue(key, out ArrayList list);
		if (list.Contains((byte)zVal))
			throw new ArgumentException("Duplicate tile in cell [" + xVal + "," + "]");

		list.Add(zVal);
		list.Sort();
		list.TrimToSize();
		table.Remove(key);
		table.Add(key, list);
		return;
    }
	private void AddCell (int xVal, int yVal, int zVal)
	{
		int[] temp = { xVal, yVal };
		if (!isByte(temp) && !isByte(zVal))
			throw new ArgumentOutOfRangeException();
			
		ArrayList cell = new ArrayList(1);
		cell.Add((ushort)zVal);
		table.Add(XYToKey((byte)xVal, (byte)yVal), cell);
		return;
	}
	public bool ContainsCell (int xVal, int yVal)
    {
		int[] temp = { xVal, yVal };
		if (!isByte(temp))
			throw new ArgumentOutOfRangeException();

		ushort cellKey = XYToKey((byte)xVal, (byte)yVal);
		return table.ContainsKey(cellKey);
    }
	public bool ContainsValue (int xVal, int yVal, int zVal)
	{
		int[] temp = { xVal, yVal };
		if (!isByte(temp) && !isByte(zVal))
			throw new ArgumentOutOfRangeException();

		if (!ContainsCell(xVal, yVal))
			return false;

		ushort cellKey = XYToKey((byte)xVal, (byte)yVal);
		table.TryGetValue(cellKey, out ArrayList list);
		return list.Contains((ushort)zVal);
	}
	public void ModifyValue (int xVal, int yVal, int zVal)
	{
		int[] temp = { xVal, yVal };
		if (!isByte(temp) && !isByte(zVal))
			throw new ArgumentOutOfRangeException();

		ushort key = XYToKey((byte)xVal, (byte)yVal);
		table.TryGetValue(key, out ArrayList list);
		int tileIndex = list.IndexOf((byte)zVal);
		list.RemoveAt(tileIndex);
		list.Insert(tileIndex, (byte)zVal);
		list.TrimToSize();
		table.Remove(key);
		table.Add(key, list);
	}

	//public static void Main(string[] args)
	//{
	//	ChunkTable testChunk = new ChunkTable(64);
	//}
}