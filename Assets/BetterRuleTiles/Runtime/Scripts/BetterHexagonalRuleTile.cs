using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using VinTools.BetterRuleTiles.Internal;
using System;

namespace VinTools.BetterRuleTiles
{
    [CreateAssetMenu(menuName = "VinTools/Custom Tiles/Better Hexagonal Rule Tile")]
    public class BetterHexagonalRuleTile : HexagonalRuleTile<Neighbor>
    {
        public int UniqueID;
        public BetterHexagonalRuleTile[] otherTiles;
        public List<TileBase> variations = new List<TileBase>();
        public List<CustomTileProperty> customProperties = new List<CustomTileProperty>();

        public List<ExtendedTilingRule> m_ExtendedTilingRules = new List<ExtendedTilingRule>();
        public List<ExtraTilingRule> m_ExtraTilingRules = new List<ExtraTilingRule>();

        public int UniqueIdentifier { get => UniqueID; set => UniqueID = value; }
        public UnityEngine.Object TileObject { get => this; }

        public override bool RuleMatch(int neighbor, TileBase tile)
        {
            if (tile is RuleOverrideTile ot)
                tile = ot.m_InstanceTile;

            switch (neighbor)
            {
                case Neighbor.This: return tile == this || variations.Contains(tile);
                case Neighbor.NotThis: return tile != this && !variations.Contains(tile);
                case Neighbor.Any: return tile != null;
                case Neighbor.Empty: return tile == null;
                case Neighbor.Ignore: return true;
                default:
                    if (neighbor > 0) return tile == otherTiles[neighbor - 1] || otherTiles[neighbor - 1].variations.Contains(tile);
                    break;
            }
            return true;
        }

        /// <summary>
        /// Gets the integer with the specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value if the key was not found</param>
        /// <returns></returns>
        public int GetInt(string key, int defaultValue = default)
        {
            if (GetProperty(key, out var p)) return p.GetInt();
            else return defaultValue;
        }
        /// <summary>
        /// Gets the float with the specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value if the key was not found</param>
        /// <returns></returns>
        public float GetFloat(string key, float defaultValue = default)
        {
            if (GetProperty(key, out var p)) return p.GetFloat();
            else return defaultValue;
        }
        /// <summary>
        /// Gets the double with the specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value if the key was not found</param>
        /// <returns></returns>
        public double GetDouble(string key, double defaultValue = default)
        {
            if (GetProperty(key, out var p)) return p.GetDouble();
            else return defaultValue;
        }
        /// <summary>
        /// Gets the character with the specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value if the key was not found</param>
        /// <returns></returns>
        public char GetChar(string key, char defaultValue = default)
        {
            if (GetProperty(key, out var p)) return p.GetChar();
            else return defaultValue;
        }
        /// <summary>
        /// Gets the string with the specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value if the key was not found</param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue = default)
        {
            if (GetProperty(key, out var p)) return p.GetString();
            else return defaultValue;
        }
        /// <summary>
        /// Gets the boolean with the specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value if the key was not found</param>
        /// <returns></returns>
        public bool GetBool(string key, bool defaultValue = default)
        {
            if (GetProperty(key, out var p)) return p.GetBool();
            else return defaultValue;
        }

        private bool GetProperty(string key, out CustomTileProperty prop)
        {
            prop = customProperties.Find(t => t._key == key);
            if (prop == null) return false;
            else return true;
        }
    }
}