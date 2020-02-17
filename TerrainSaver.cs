using System;
using Godot;

namespace Subterrania.Core.Terrain
{
	/*
		* Class for handling the saving and loading of altered chunk data to and from a text save file.
		* Filename format should be savename.terraindata.
		*/
	public class TerrainSaver
	{
		private string saveName;

        #region constructors
        public TerrainSaver() => saveName = "default";

        public TerrainSaver(string saveName) => this.saveName = saveName;
        #endregion

        #region save methods
        public static void SaveChunkData (ChunkTable data)
        {

        }

		public static void SaveChunkList ()
        {

        }
        #endregion

        #region load methods
        public static ChunkTable LoadChunkData (string saveName)
        {

        }

		public static ArrayList LoadChunkList (string saveName)
        {

        }
        #endregion
        // Should these stay static or is it better to make the TerrainSaver be loaded in as an object?
    }
}