using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public static class XmlTools 
{

    public static void Serialize(System.Object item, string path)
    {
        
        XmlSerializer serializer = new XmlSerializer(item.GetType());
        using (TextWriter writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, item);
        }
    }

    public static T Deserialize<T>(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using (TextReader reader = new StreamReader(path))
        {
            return (T)serializer.Deserialize(reader);
        }
    }


}
