namespace Figlut.Mobile.Toolkit.Tools.IM
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;
    using System.Collections;
    using System.IO;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Utilities.Serialization;

    #endregion //Using Directives

    /// <summary>
    /// A wrapper class for holding a dictionary of hotspots.
    /// All hotspots are indexed according to their keys.
    /// </summary>
    public class HotspotsCache : IEnumerable<Hotspot>
    {
        #region Constants

        /// <summary>
        /// A wrapper class for holding a dictionary of hotspots.
        /// All hotspots are indexed according to their keys.
        /// </summary>
        public HotspotsCache()
        {
            _hotspots = new Dictionary<string, Hotspot>();
        }

        #endregion //Constants

        #region Fields

        protected Dictionary<string, Hotspot> _hotspots;

        #endregion //Fields

        #region Indexers

        /// <summary>
        /// Attempts to get a hotspot with the provided key.
        /// If one does not exist it returns null.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Hotspot this[string key]
        {
            get
            {
                if (!_hotspots.ContainsKey(key))
                {
                    return null;
                }
                return _hotspots[key];
            }
        }

        #endregion //Indexers

        #region Methods

        /// <summary>
        /// Adds a hotspot to this cache. 
        /// If a hotspot with the same key has already been added, an exception will be thrown.
        /// </summary>
        /// <param name="hotspot">The hotspot to add.</param>
        public void Add(Hotspot hotspot)
        {
            if (_hotspots.ContainsKey(hotspot.Key))
            {
                throw new ArgumentException(string.Format("Hotspot with key {0} already added.", hotspot.Key));
            }
            if (hotspot == null)
            {
                throw new NullReferenceException("Hotspot to be added to cache may not be null.");
            }
            _hotspots.Add(hotspot.Key, hotspot);
        }

        /// <summary>
        /// Determines whether a hotspot with the provided key already exists in this cache.
        /// </summary>
        /// <param name="key">The hotspot key to search this cache by.</param>
        /// <returns>Returns true if a hotspot with a key matching the provided key exists.</returns>
        public bool Exists(string key)
        {
            return _hotspots.ContainsKey(key);
        }

        /// <summary>
        /// Removes a hotspot from this cache that has a key matching the provided key.
        /// If a hotspot with the same key does not exist in this cache, an exception will be thrown.
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            if (!_hotspots.ContainsKey(key))
            {
                throw new ArgumentException(string.Format("Hotspot with key {0} does not exist in cache.", key));
            }
            _hotspots.Remove(key);
        }

        /// <summary>
        /// Exports all the hotspots that have been added to this cache to
        /// an XML file. All the types of hotspots that have been added need 
        /// to be included in order for all the hotspots to be serialized to XML successfully.
        /// </summary>
        /// <param name="filePath">The file path to the XML file to where the hotspots will be exported to.</param>
        /// <param name="hotspotDerivedTypes">The hotspot types that have been added to this cache e.g. RectangleHotspot/CircleHotspot etc.</param>
        public void Export(string filePath, Type[] hotspotDerivedTypes)
        {
            GOC.Instance.GetSerializer(SerializerType.XML).SerializeToFile(this, hotspotDerivedTypes, filePath);
        }

        /// <summary>
        /// Imports hotspots from an XML file into this cache. Existing hotspot are overwritten with the new ones. 
        /// All the types (e.g. RectangleHotspot/CircleHotspot etc.) 
        /// of hotspots that are in the XML file need to be specified in order
        /// for all the hotspots to be deserialized from XML successfully.
        /// </summary>
        /// <param name="filePath">The file path to the XML file from where the hotspots should be imported from.</param>
        /// <param name="hotspotDerivedTypes">The hotspot types that exist in the XML file to be imported e.g. RectangleHotspot/CircleHotspot etc.</param>
        /// <returns></returns>
        public static HotspotsCache Import(string filePath, Type[] hotspotDerivedTypes)
        {
            FileSystemHelper.ValidateFileExists(filePath);
            return (HotspotsCache)GOC.Instance.GetSerializer(SerializerType.XML).DeserializeFromFile(
                typeof(HotspotsCache), hotspotDerivedTypes, filePath);
        }

        /// <summary>
        /// Clears this cache of all the hotspots it holds. Any hotspots that have not
        /// already been exported to file will be lost.
        /// </summary>
        public void Clear()
        {
            _hotspots.Clear();
        }

        /// <summary>
        /// Gets an enumerator for the hotspots in this cache.
        /// </summary>
        /// <returns>Returns an enumerator for the hotspots in this cache.</returns>
        public IEnumerator<Hotspot> GetEnumerator()
        {
            return _hotspots.Values.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the hotspots in this cache.
        /// </summary>
        /// <returns>Returns an enumerator for the hotspots in this cache.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion //Methods
    }
}