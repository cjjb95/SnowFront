/*
Function: 		Provide generic map to load and store game content AND allow dispose() to be called on all content
Author: 		NMCG
Version:		1.0
Date Updated:	11/9/17
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace GDLibrary
{
    public class ContentDictionary<V> : IDisposable
    {
        #region Fields
        private string name;
        private Dictionary<string, V> dictionary;
        private ContentManager content;
        #endregion

        #region Properties
        protected Dictionary<string, V> Dictionary
        {
            get
            {
                return dictionary;
            }
        }

        public V this[string key]
        {
            get
            {
                if (!this.Dictionary.ContainsKey(key))
                    throw new Exception(key + " resource was not found in dictionary. Have you loaded it?");

                return this.dictionary[key];
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        #endregion

        public ContentDictionary(string name, ContentManager content)
        {
            this.name = name;
            this.content = content;
            this.dictionary = new Dictionary<string, V>();
        }

        public virtual bool Load(string assetPath, string key)
        {
            if (!this.dictionary.ContainsKey(key))
            {
                this.dictionary.Add(key, this.content.Load<V>(assetPath));
                return true;
            }
            return false;
        }

        //same as Load() above but uses assetPath to form key string from regex
        public virtual bool Load(string assetPath)
        {
            return Load(assetPath, StringUtility.ParseNameFromPath(assetPath));
        }

        public virtual bool Unload(string key)
        {
            if (this.dictionary.ContainsKey(key))
            {
                //unload from RAM
                Dispose(this.dictionary[key]);
                //remove from dictionary
                this.dictionary.Remove(key);
                return true;
            }
            return false;
        }

        public virtual int Count()
        {
            return this.dictionary.Count;
        }

        public virtual void Dispose()
        {
            //copy values from dictionary to list
            List<V> list = new List<V>(dictionary.Values);

            for (int i = 0; i < list.Count; i++)
                Dispose(list[i]);

            //empty the list
            list.Clear();

            //clear the dictionary
            this.dictionary.Clear();
        }

        public virtual void Dispose(V value)
        {
            //if this is a disposable object (e.g. model, sound, font, texture) then call its dispose
            if (value is IDisposable)
                ((IDisposable)value).Dispose();
            //if it's just a user-defined or C# object, then set to null for garbage collection
            else
                value = default(V); //null
        }
    }
}
