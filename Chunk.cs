using System;
using Godot;

namespace Subterrania.Core.Terrain
{
	public class Chunk
	{
		private ChunkTable data;

		public Chunk()
		{
			data = new ChunkTable();
		}
	}
}
