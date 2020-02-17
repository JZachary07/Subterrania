using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace Subterrania.Core.Terrain
{
	public class ChunkTable
	{
		private ushort tableSize;
		private byte chunkSize;
		private Dictionary<ushort, ArrayList> table;
		// Is this the correct data structure?
		// ArrayList[] or ArrayList[][] could both work instead.
		// Both would be simpler and just as fast, if not faster.

		#region constructors
		public ChunkTable() // default constructor - assumes 32x32 default chunk size
		{
			chunkSize = 32;
			tableSize = (ushort)(chunkSize * chunkSize);
			table = new Dictionary<ushort, ArrayList>(tableSize);
		}


		public ChunkTable (int chunkSize) // loaded constructor - takes chunk size as a parameter
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

		private bool isByte (int value) // checks single int is in range for int --> byte casting (invokes sister method)
		{
			int[] list = { value };
			return isByte(list);
		}
		#endregion
	
		#region data methods
		public void AddTile (int xVal, int yVal, int zVal) // adds a new Z value to an existing cell at X-Y, or calls AddCell() if no cell yet exists
		{
			int[] temp = { xVal, yVal };
			if (!isByte(temp) && !isByte(zVal))
				throw new ArgumentOutOfRangeException("A value is out of range for data minimization");

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

		private void AddCell (int xVal, int yVal, int zVal) // creates new cell at X-Y with a single Z value
		{
			int[] temp = { xVal, yVal };
			if (!isByte(temp) && !isByte(zVal))
				throw new ArgumentOutOfRangeException();
			
			ArrayList cell = new ArrayList(1);
			cell.Add((ushort)zVal);
			table.Add(XYToKey((byte)xVal, (byte)yVal), cell);
			return;
		}

		public bool ContainsCell (int xVal, int yVal) // checks if a cell exists at X-Y
		{
			int[] temp = { xVal, yVal };
			if (!isByte(temp))
				throw new ArgumentOutOfRangeException();

			ushort cellKey = XYToKey((byte)xVal, (byte)yVal);
			return table.ContainsKey(cellKey);
		}

		public bool ContainsValue (int xVal, int yVal, int zVal) // checks if a cell exists at X-Y and if that cell contains the specified Z value
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

		public void ModifyValue (int xVal, int yVal, int oldZVal, int newZVal) // locates a Z value in cell X-Y and replaces it with a new value
		{
			int[] temp = { xVal, yVal };
			if (!isByte(temp) && !isByte(oldZVal) && !isByte(newZVal))
				throw new ArgumentOutOfRangeException();
			if (!ContainsValue(xVal,yVal,oldZVal))
				throw new ArgumentOutOfRangeException();

			ushort key = XYToKey((byte)xVal, (byte)yVal);
			table.TryGetValue(key, out ArrayList list);
			int tileIndex = list.IndexOf((byte)oldZVal);
			list.RemoveAt(tileIndex);
			list.Insert(tileIndex, (byte)newZVal);
			list.TrimToSize();
			table.Remove(key);
			table.Add(key, list);
		}
		#endregion
	}
}